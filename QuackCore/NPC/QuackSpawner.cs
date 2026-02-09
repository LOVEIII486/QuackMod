using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using Duckov.Utilities;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using QuackCore.Constants;
using SodaCraft.Localizations;

namespace QuackCore.NPC
{
    /// <summary>
    /// 通用 NPC 生成工具类。
    /// 负责 CharacterRandomPreset 的动态克隆、参数注入及异步生成。
    /// </summary>
    public class QuackSpawner : MonoBehaviour
    {
        public static QuackSpawner Instance { get; private set; }

        private bool _isInitialized = false;
        
        private Dictionary<string, CharacterRandomPreset> _gameNativePresetMap = new Dictionary<string, CharacterRandomPreset>();
        private Dictionary<string, CharacterRandomPreset> _generatedPresetsCache = new Dictionary<string, CharacterRandomPreset>();
        private List<CharacterRandomPreset> _clonedPresets = new List<CharacterRandomPreset>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartCoroutine(InitializeRoutine());
        }

        private IEnumerator InitializeRoutine()
        {
            while (CharacterMainControl.Main == null) yield return null;
            while (GameplayDataSettings.CharacterRandomPresetData == null) yield return null;

            var allPresets = GameplayDataSettings.CharacterRandomPresetData.presets;
            _gameNativePresetMap.Clear();
            foreach (var preset in allPresets)
            {
                // 参考 EliteEnemies：使用 preset.name 作为唯一 Key
                if (preset != null && !string.IsNullOrEmpty(preset.name))
                {
                    if (!_gameNativePresetMap.ContainsKey(preset.name))
                        _gameNativePresetMap.Add(preset.name, preset);
                }
            }

            _isInitialized = true;
            ModLogger.Log("QuackSpawner 初始化完成：已同步原生预设库 (使用 Asset Name 索引)。");
        }

        public async UniTask<CharacterMainControl> SpawnNPC(QuackNPCConfig config, Vector3 position, Teams? team = null)
        {
            if (!_isInitialized)
            {
                ModLogger.LogError("[QuackSpawner] 尚未初始化。");
                return null;
            }

            // 使用 config.BasePresetName 查找
            if (!_gameNativePresetMap.TryGetValue(config.BasePresetName, out var sourcePreset))
            {
                ModLogger.LogError($"[QuackSpawner] 找不到基底资源: {config.BasePresetName}");
                return null;
            }

            CharacterRandomPreset finalPreset = GetOrCreateCustomPreset(sourcePreset, config);
            if (team.HasValue) finalPreset.team = team.Value;

            try
            {
                int sceneIndex = MultiSceneCore.MainScene.HasValue 
                    ? MultiSceneCore.MainScene.Value.buildIndex 
                    : UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

                CharacterMainControl character = await finalPreset.CreateCharacterAsync(
                    position + Vector3.down * 0.25f,
                    Vector3.forward,
                    sceneIndex,
                    null,
                    false
                );

                if (character != null)
                {
                    character.SetPosition(position);
                    if (character.Team == Teams.player)
                    {
                        AICharacterController ai = character.aiCharacterController;
                        if (ai != null)
                        {
                            ai.leader = CharacterMainControl.Main;
                            var pet = ai.GetComponent<PetAI>();
                            if (pet != null) pet.SetMaster(CharacterMainControl.Main);
                        }
                    }
                    return character;
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"[QuackSpawner] 生成异常: {ex}");
            }
            return null;
        }

        private CharacterRandomPreset GetOrCreateCustomPreset(CharacterRandomPreset source, QuackNPCConfig config)
        {
            string uniqueId = string.IsNullOrEmpty(config.CustomName) ? "Default" : config.CustomName;
            // Cache Key 同样基于 BasePresetName
            string cacheKey = $"{config.BasePresetName}_Quack_{uniqueId}";

            if (_generatedPresetsCache.TryGetValue(cacheKey, out var cached)) return cached;

            CharacterRandomPreset preset = Instantiate(source);
            
            // 参考 EliteEnemies 的命名习惯：
            // preset.name 决定了该 Asset 的身份
            preset.name = source.name + "_Quack_" + uniqueId;
            
            // 重要：nameKey 决定了该 NPC 在 UI 上显示的翻译文本
            // 如果用户指定了 CustomName，我们需要覆盖本地化系统
            if (!string.IsNullOrEmpty(config.CustomName))
            {
                string overrideKey = "QuackNPC_" + uniqueId;
                preset.nameKey = overrideKey;
                LocalizationManager.SetOverrideText(overrideKey, config.CustomName);
            }
            else
            {
                // 如果没有自定义名字，保留原版的本地化 Key
                preset.nameKey = source.nameKey;
            }

            ApplyConfigToPreset(preset, config);
            
            _clonedPresets.Add(preset);
            _generatedPresetsCache.Add(cacheKey, preset);
            return preset;
        }

        private void ApplyConfigToPreset(CharacterRandomPreset preset, QuackNPCConfig config)
        {
            // --- 1. 身份表现  ---
            QuackReflectionHelper.SetPrivateField(preset, "characterIconType", config.IconType);
            preset.voiceType = config.VoiceType;
            preset.footstepMaterialType = config.FootstepType;
            preset.team = config.Team;

            // --- 2. 核心属性 ---
            preset.health = config.Health;
            preset.exp = config.Exp;
            preset.hasSoul = config.HasSoul;
            preset.showName = config.ShowName;
            preset.showHealthBar = config.ShowHealthBar;
            preset.canDieIfNotRaidMap = config.CanDieIfNotRaidMap;
            preset.pushCharacter = config.PushCharacter;
            preset.moveSpeedFactor = config.MoveSpeedFactor;

            // --- 3. AI 感知与追踪 ---
            preset.setActiveByPlayerDistance = config.SetActiveByPlayerDistance;
            preset.sightDistance = config.SightDistance;
            preset.sightAngle = config.SightAngle;
            preset.hearingAbility = config.HearingAbility;
            preset.nightVisionAbility = config.NightVisionAbility;
            preset.forgetTime = config.ForgetTime;
            preset.forceTracePlayerDistance = config.ForceTracePlayerDistance;
            preset.minTraceTargetChance = config.MinTraceTargetChance;
            preset.maxTraceTargetChance = config.MaxTraceTargetChance;

            // --- 4. 远程战斗数值 ---
            preset.damageMultiplier = config.DamageMultiplier;
            preset.bulletSpeedMultiplier = config.BulletSpeedMultiplier;
            preset.gunDistanceMultiplier = config.GunDistanceMultiplier;
            preset.gunScatterMultiplier = config.GunScatterMultiplier;
            preset.scatterMultiIfTargetRunning = config.ScatterMultiIfTargetRunning;
            preset.scatterMultiIfOffScreen = config.ScatterMultiIfOffScreen;
            preset.gunCritRateGain = config.GunCritRateGain;
            preset.shootDelay = config.ShootDelay;
            preset.shootTimeRange = config.ShootTimeRange;
            preset.shootTimeSpaceRange = config.ShootTimeSpaceRange;
            preset.shootCanMove = config.ShootCanMove;
            preset.defaultWeaponOut = config.DefaultWeaponOut;

            // --- 5. 近战与战术移动 ---
            preset.setMeleeDamageMultiplier = config.SetMeleeDamageMultiplier;
            preset.meleeDamageMultiplier = config.MeleeDamageMultiplier;
            preset.aiCombatFactor = config.AiCombatFactor;
            preset.patrolRange = config.PatrolRange;
            preset.combatMoveRange = config.CombatMoveRange;
            preset.combatMoveTimeRange = config.CombatMoveTimeRange;
            preset.patrolTurnSpeed = config.PatrolTurnSpeed;
            preset.combatTurnSpeed = config.CombatTurnSpeed;
            preset.canDash = config.CanDash;
            preset.dashCoolTimeRange = config.DashCoolTimeRange;
            preset.canTalk = config.CanTalk;

            // --- 6. 技能设定 ---
            preset.hasSkill = config.HasSkill;
            preset.hasSkillChance = config.HasSkillChance;
            preset.skillCoolTimeRange = config.SkillCoolTimeRange;
            preset.skillSuccessChance = config.SkillSuccessChance;
            preset.itemSkillChance = config.ItemSkillChance;
            preset.itemSkillCoolTime = config.ItemSkillCoolTime;

            // --- 7. 元素抗性 ---
            preset.elementFactor_Physics = config.ResistPhysics;
            preset.elementFactor_Fire = config.ResistFire;
            preset.elementFactor_Ice = config.ResistIce;
            preset.elementFactor_Poison = config.ResistPoison;
            preset.elementFactor_Electricity = config.ResistElectricity;
            preset.elementFactor_Space = config.ResistSpace;
            preset.elementFactor_Ghost = config.ResistGhost;

            // --- 8. 物品与掉落 ---
            preset.dropBoxOnDead = config.DropBoxOnDead;
            preset.wantItem = config.WantItem;
            preset.hasCashChance = config.HasCashChance;
            preset.cashRange = config.CashRange;
            QuackReflectionHelper.SetPrivateField(preset, "bulletCountRange", config.BulletCountRange);

            // --- 9. 特殊载具 ---
            preset.isVehicle = config.IsVehicle;
            preset.onlyMoveForward = config.OnlyMoveForward;

            // --- 10. 背包初始物品注入 ---
            if (config.CustomItemIDs != null && config.CustomItemIDs.Count > 0)
            {
                SetupInventory(preset, config.CustomItemIDs);
            }
        }
        
        public void ExportNativePresets()
        {
            if (!_isInitialized)
            {
                ModLogger.LogWarning("尚未初始化，无法导出预设。");
                return;
            }

            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "QuackNativePresets.txt");
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Asset Name\tLocalization Key\tProperty Name\tProperty DisplayName");

                foreach (var kvp in _gameNativePresetMap)
                {
                    CharacterRandomPreset p = kvp.Value;
            
                    string assetName = p.name;
                    string locKey = p.nameKey;
                    string propName = p.Name;
                    string propDisplayName = p.DisplayName;

                    sb.AppendLine($"{assetName}\t{locKey}\t{propName}\t{propDisplayName}");
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                ModLogger.Log($"预设库导出成功！文件路径: {filePath}");
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"导出预设库失败: {ex.Message}");
            }
        }
        
        public void SimpleCheck()
        {
            var defined = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Action<Type> scan = null;
            scan = (t) => {
                foreach (var f in t.GetFields()) if (f.IsLiteral) defined.Add(f.GetValue(null).ToString());
                foreach (var nt in t.GetNestedTypes()) scan(nt);
            };
            scan(typeof(NPCPresetNames));

            int missing = 0;
            foreach (var assetName in _gameNativePresetMap.Keys)
            {
                if (!defined.Contains(assetName))
                {
                    ModLogger.LogWarning($"[遗漏] 游戏中有但常量表没写: {assetName}");
                    missing++;
                }
            }

            int redundant = 0;
            foreach (var constantValue in defined)
            {
                if (!_gameNativePresetMap.ContainsKey(constantValue))
                {
                    ModLogger.LogError($"[冗余] 常量表里有但实际游戏没这个资源: {constantValue}");
                    redundant++;
                }
            }

            ModLogger.Log($"比对完成。遗漏: {missing}, 冗余: {redundant}");
        }
        
        private void SetupInventory(CharacterRandomPreset preset, List<int> itemIDs)
        {
            var itemsList = QuackReflectionHelper.GetPrivateField<IList>(preset, "itemsToGenerate");
            if (itemsList != null)
            {
                itemsList.Clear();
                foreach (int id in itemIDs)
                {
                    var desc = new RandomItemGenerateDescription
                    {
                        chance = 1f,
                        randomCount = new Vector2Int(1, 1),
                        randomFromPool = true,
                        itemPool = new RandomContainer<RandomItemGenerateDescription.Entry>(),
                        tags = new RandomContainer<Tag>(),
                        addtionalRequireTags = new List<Tag>(),
                        excludeTags = new List<Tag>(),
                        qualities = new RandomContainer<int>(),
                    };
                    
                    desc.itemPool.AddEntry(new RandomItemGenerateDescription.Entry { itemTypeID = id }, 100f);
                    itemsList.Add(desc);
                }
            }
        }
    }
 
    internal static class QuackReflectionHelper
    {
        public static void SetPrivateField(object obj, string fieldName, object value)
        {
            if (obj == null) return;
            var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null) field.SetValue(obj, value);
        }

        public static T GetPrivateField<T>(object obj, string fieldName) where T : class
        {
            if (obj == null) return null;
            var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return field?.GetValue(obj) as T;
        }
    }
}