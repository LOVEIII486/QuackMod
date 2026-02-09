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

        private Dictionary<string, CharacterRandomPreset> _gameNativePresetMap =
            new Dictionary<string, CharacterRandomPreset>();

        private Dictionary<string, CharacterRandomPreset> _generatedPresetsCache =
            new Dictionary<string, CharacterRandomPreset>();

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
                if (preset != null && !string.IsNullOrEmpty(preset.name))
                {
                    if (!_gameNativePresetMap.ContainsKey(preset.name))
                        _gameNativePresetMap.Add(preset.name, preset);
                }
            }

            _isInitialized = true;
            ModLogger.Log("QuackSpawner 初始化完成：已同步原生预设库。");
        }

        public async UniTask<CharacterMainControl> SpawnNPC(QuackNPCConfig config, Vector3 position, Teams? team = null)
        {
            if (!_isInitialized)
            {
                ModLogger.LogError("QuackSpawner 尚未初始化。");
                return null;
            }

            if (!_gameNativePresetMap.TryGetValue(config.BasePresetName, out var sourcePreset))
            {
                ModLogger.LogError($"找不到基底资源: {config.BasePresetName}");
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
                    
                    ModLogger.LogDebug($"[QuackSpawner] 已生成自定义 NPC: {config.BasePresetName}");
                    return character;
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"生成异常: {ex}");
            }

            return null;
        }

        public async UniTask<CharacterMainControl> SpawnVanillaNPC(string assetName, Vector3 position,
            Teams? team = null)
        {
            if (!_isInitialized)
            {
                ModLogger.LogError("[QuackSpawner] 尚未初始化。");
                return null;
            }

            if (!_gameNativePresetMap.TryGetValue(assetName, out var sourcePreset))
            {
                ModLogger.LogError($"[QuackSpawner] 找不到原版资源: {assetName}");
                return null;
            }

            Teams originalTeam = sourcePreset.team;
            if (team.HasValue) sourcePreset.team = team.Value;

            try
            {
                int sceneIndex = MultiSceneCore.MainScene.HasValue
                    ? MultiSceneCore.MainScene.Value.buildIndex
                    : UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

                CharacterMainControl character = await sourcePreset.CreateCharacterAsync(
                    position + Vector3.down * 0.25f,
                    Vector3.forward,
                    sceneIndex,
                    null,
                    false
                );

                sourcePreset.team = originalTeam;

                if (character != null)
                {
                    character.SetPosition(position);
                    ModLogger.LogDebug($"[QuackSpawner] 已生成纯原版 NPC: {assetName}");
                    return character;
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"[QuackSpawner] 原生路径生成异常: {ex}");
            }

            return null;
        }

        private CharacterRandomPreset GetOrCreateCustomPreset(CharacterRandomPreset source, QuackNPCConfig config)
        {
            string uniqueId = string.IsNullOrEmpty(config.CustomName) ? "Default" : config.CustomName;
            string cacheKey = $"{config.BasePresetName}_Quack_{uniqueId}";

            if (_generatedPresetsCache.TryGetValue(cacheKey, out var cached)) return cached;

            CharacterRandomPreset preset = Instantiate(source);

            preset.name = source.name + "_Quack_" + uniqueId;

            if (!string.IsNullOrEmpty(config.CustomName))
            {
                string overrideKey = "QuackNPC_" + uniqueId;
                preset.nameKey = overrideKey;
                LocalizationManager.SetOverrideText(overrideKey, config.CustomName);
            }
            else
            {
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
            if (config.IconType.HasValue)
                QuackReflectionHelper.SetPrivateField(preset, "characterIconType", config.IconType.Value);
            if (config.VoiceType.HasValue) preset.voiceType = config.VoiceType.Value;
            if (config.FootstepType.HasValue) preset.footstepMaterialType = config.FootstepType.Value;
            if (config.Team.HasValue) preset.team = config.Team.Value;

            // --- 2. 核心属性 ---
            if (config.Health.HasValue) preset.health = config.Health.Value;
            if (config.Exp.HasValue) preset.exp = config.Exp.Value;
            if (config.HasSoul.HasValue) preset.hasSoul = config.HasSoul.Value;
            if (config.ShowName.HasValue) preset.showName = config.ShowName.Value;
            if (config.ShowHealthBar.HasValue) preset.showHealthBar = config.ShowHealthBar.Value;
            if (config.CanDieIfNotRaidMap.HasValue) preset.canDieIfNotRaidMap = config.CanDieIfNotRaidMap.Value;
            if (config.PushCharacter.HasValue) preset.pushCharacter = config.PushCharacter.Value;
            if (config.MoveSpeedFactor.HasValue) preset.moveSpeedFactor = config.MoveSpeedFactor.Value;

            // --- 3. AI 感知与追踪 ---
            if (config.SetActiveByPlayerDistance.HasValue)
                preset.setActiveByPlayerDistance = config.SetActiveByPlayerDistance.Value;
            if (config.SightDistance.HasValue) preset.sightDistance = config.SightDistance.Value;
            if (config.SightAngle.HasValue) preset.sightAngle = config.SightAngle.Value;
            if (config.HearingAbility.HasValue) preset.hearingAbility = config.HearingAbility.Value;
            if (config.NightVisionAbility.HasValue) preset.nightVisionAbility = config.NightVisionAbility.Value;
            if (config.ForgetTime.HasValue) preset.forgetTime = config.ForgetTime.Value;
            if (config.ForceTracePlayerDistance.HasValue)
                preset.forceTracePlayerDistance = config.ForceTracePlayerDistance.Value;
            if (config.MinTraceTargetChance.HasValue) preset.minTraceTargetChance = config.MinTraceTargetChance.Value;
            if (config.MaxTraceTargetChance.HasValue) preset.maxTraceTargetChance = config.MaxTraceTargetChance.Value;

            // --- 4. 远程战斗数值 ---
            if (config.DamageMultiplier.HasValue) preset.damageMultiplier = config.DamageMultiplier.Value;
            if (config.BulletSpeedMultiplier.HasValue)
                preset.bulletSpeedMultiplier = config.BulletSpeedMultiplier.Value;
            if (config.GunDistanceMultiplier.HasValue)
                preset.gunDistanceMultiplier = config.GunDistanceMultiplier.Value;
            if (config.GunScatterMultiplier.HasValue) preset.gunScatterMultiplier = config.GunScatterMultiplier.Value;
            if (config.ScatterMultiIfTargetRunning.HasValue)
                preset.scatterMultiIfTargetRunning = config.ScatterMultiIfTargetRunning.Value;
            if (config.ScatterMultiIfOffScreen.HasValue)
                preset.scatterMultiIfOffScreen = config.ScatterMultiIfOffScreen.Value;
            if (config.GunCritRateGain.HasValue) preset.gunCritRateGain = config.GunCritRateGain.Value;
            if (config.ShootDelay.HasValue) preset.shootDelay = config.ShootDelay.Value;
            if (config.ShootTimeRange.HasValue) preset.shootTimeRange = config.ShootTimeRange.Value;
            if (config.ShootTimeSpaceRange.HasValue) preset.shootTimeSpaceRange = config.ShootTimeSpaceRange.Value;
            if (config.ShootCanMove.HasValue) preset.shootCanMove = config.ShootCanMove.Value;
            if (config.DefaultWeaponOut.HasValue) preset.defaultWeaponOut = config.DefaultWeaponOut.Value;

            // --- 5. 近战与战术移动 ---
            if (config.SetMeleeDamageMultiplier.HasValue)
                preset.setMeleeDamageMultiplier = config.SetMeleeDamageMultiplier.Value;
            if (config.MeleeDamageMultiplier.HasValue)
                preset.meleeDamageMultiplier = config.MeleeDamageMultiplier.Value;
            if (config.AiCombatFactor.HasValue) preset.aiCombatFactor = config.AiCombatFactor.Value;
            if (config.PatrolRange.HasValue) preset.patrolRange = config.PatrolRange.Value;
            if (config.CombatMoveRange.HasValue) preset.combatMoveRange = config.CombatMoveRange.Value;
            if (config.CombatMoveTimeRange.HasValue) preset.combatMoveTimeRange = config.CombatMoveTimeRange.Value;
            if (config.PatrolTurnSpeed.HasValue) preset.patrolTurnSpeed = config.PatrolTurnSpeed.Value;
            if (config.CombatTurnSpeed.HasValue) preset.combatTurnSpeed = config.CombatTurnSpeed.Value;
            if (config.CanDash.HasValue) preset.canDash = config.CanDash.Value;
            if (config.DashCoolTimeRange.HasValue) preset.dashCoolTimeRange = config.DashCoolTimeRange.Value;
            if (config.CanTalk.HasValue) preset.canTalk = config.CanTalk.Value;

            // --- 6. 技能设定 ---
            if (config.HasSkill.HasValue) preset.hasSkill = config.HasSkill.Value;
            if (config.HasSkillChance.HasValue) preset.hasSkillChance = config.HasSkillChance.Value;
            if (config.SkillCoolTimeRange.HasValue) preset.skillCoolTimeRange = config.SkillCoolTimeRange.Value;
            if (config.SkillSuccessChance.HasValue) preset.skillSuccessChance = config.SkillSuccessChance.Value;
            if (config.ItemSkillChance.HasValue) preset.itemSkillChance = config.ItemSkillChance.Value;
            if (config.ItemSkillCoolTime.HasValue) preset.itemSkillCoolTime = config.ItemSkillCoolTime.Value;

            // --- 7. 元素抗性 ---
            if (config.ResistPhysics.HasValue) preset.elementFactor_Physics = config.ResistPhysics.Value;
            if (config.ResistFire.HasValue) preset.elementFactor_Fire = config.ResistFire.Value;
            if (config.ResistIce.HasValue) preset.elementFactor_Ice = config.ResistIce.Value;
            if (config.ResistPoison.HasValue) preset.elementFactor_Poison = config.ResistPoison.Value;
            if (config.ResistElectricity.HasValue) preset.elementFactor_Electricity = config.ResistElectricity.Value;
            if (config.ResistSpace.HasValue) preset.elementFactor_Space = config.ResistSpace.Value;
            if (config.ResistGhost.HasValue) preset.elementFactor_Ghost = config.ResistGhost.Value;

            // --- 8. 物品与掉落 ---
            if (config.DropBoxOnDead.HasValue) preset.dropBoxOnDead = config.DropBoxOnDead.Value;
            if (config.WantItem.HasValue) preset.wantItem = config.WantItem.Value;
            if (config.HasCashChance.HasValue) preset.hasCashChance = config.HasCashChance.Value;
            if (config.CashRange.HasValue) preset.cashRange = config.CashRange.Value;
            if (config.BulletCountRange.HasValue)
                QuackReflectionHelper.SetPrivateField(preset, "bulletCountRange", config.BulletCountRange.Value);

            // --- 9. 特殊载具 ---
            // 关键点：如果 config 为空，则保留原版 preset.isVehicle
            if (config.IsVehicle.HasValue) preset.isVehicle = config.IsVehicle.Value;
            if (config.OnlyMoveForward.HasValue) preset.onlyMoveForward = config.OnlyMoveForward.Value;

            // --- 10. 背包初始物品注入 ---
            // 只有当传入了有效的 ID 列表时才重写背包
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
            scan = (t) =>
            {
                foreach (var f in t.GetFields())
                    if (f.IsLiteral)
                        defined.Add(f.GetValue(null).ToString());
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
            var field = obj.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null) field.SetValue(obj, value);
        }

        public static T GetPrivateField<T>(object obj, string fieldName) where T : class
        {
            if (obj == null) return null;
            var field = obj.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return field?.GetValue(obj) as T;
        }
    }
}