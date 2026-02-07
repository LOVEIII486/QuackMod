using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Duckov.Utilities;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;

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
        
        // 游戏原生预设 Key 映射表
        private Dictionary<string, CharacterRandomPreset> _gameNativePresetMap = new Dictionary<string, CharacterRandomPreset>();
        
        // 已生成的自定义预设缓存
        private Dictionary<string, CharacterRandomPreset> _generatedPresetsCache = new Dictionary<string, CharacterRandomPreset>();
        
        // 维护一个克隆列表，用于销毁时清理内存
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

        private void OnDestroy()
        {
            foreach (var preset in _clonedPresets)
            {
                if (preset != null) Destroy(preset);
            }
            _clonedPresets.Clear();
            _generatedPresetsCache.Clear();
        }

        private IEnumerator InitializeRoutine()
        {
            while (CharacterMainControl.Main == null) yield return null;
            while (GameplayDataSettings.CharacterRandomPresetData == null) yield return null;

            var allPresets = GameplayDataSettings.CharacterRandomPresetData.presets;
            _gameNativePresetMap.Clear();
            foreach (var preset in allPresets)
            {
                if (preset != null && !string.IsNullOrEmpty(preset.nameKey))
                {
                    if (!_gameNativePresetMap.ContainsKey(preset.nameKey))
                        _gameNativePresetMap.Add(preset.nameKey, preset);
                }
            }

            _isInitialized = true;
            ModLogger.Log("QuackSpawner 初始化完成：已同步原生预设库。");
        }

        /// <summary>
        /// 基于 QuackNPCConfig 异步生成一个 NPC
        /// </summary>
        /// <param name="config">NPC 配置实例</param>
        /// <param name="position">生成位置</param>
        /// <param name="team">可选：覆盖配置中的阵营</param>
        /// <returns>返回生成的 CharacterMainControl 实例</returns>
        public async UniTask<CharacterMainControl> SpawnNPC(QuackNPCConfig config, Vector3 position, Teams? team = null)
        {
            if (!_isInitialized)
            {
                ModLogger.LogError("尚未初始化，无法生成 NPC。");
                return null;
            }

            if (!_gameNativePresetMap.TryGetValue(config.BasePresetKey, out var sourcePreset))
            {
                ModLogger.LogError($"找不到基底预设: {config.BasePresetKey}");
                return null;
            }

            CharacterRandomPreset finalPreset = GetOrCreateCustomPreset(sourcePreset, config);
            
            // 如果外部传入了阵营，则临时覆盖
            if (team.HasValue) finalPreset.team = team.Value;

            try
            {
                // 确定场景 BuildIndex
                int sceneIndex = MultiSceneCore.MainScene.HasValue 
                    ? MultiSceneCore.MainScene.Value.buildIndex 
                    : UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

                // 2. 调用官方原生异步创建流程
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
                    
                    // 3. 处理跟随逻辑
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

                    ModLogger.Log($"成功生成 NPC: {character.name} (Base: {config.BasePresetKey})");
                    return character;
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"生成 NPC 时发生异常: {ex}");
            }

            return null;
        }

        private CharacterRandomPreset GetOrCreateCustomPreset(CharacterRandomPreset source, QuackNPCConfig config)
        {
            string uniqueId = string.IsNullOrEmpty(config.CustomName) ? "Default" : config.CustomName;
            string cacheKey = $"{config.BasePresetKey}_Quack_{uniqueId}";

            if (_generatedPresetsCache.TryGetValue(cacheKey, out var cached)) return cached;

            CharacterRandomPreset preset = Instantiate(source);
            preset.name = source.name + "_Quack_" + uniqueId;
            preset.nameKey = cacheKey;

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