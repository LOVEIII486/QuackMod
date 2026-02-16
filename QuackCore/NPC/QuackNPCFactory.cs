using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Duckov.Utilities;

namespace QuackCore.NPC
{
    public static class QuackNPCFactory
    {
        private static readonly Dictionary<string, CharacterRandomPreset> _templateCache = new Dictionary<string, CharacterRandomPreset>();

        /// <summary>
        /// 创建并缓存 NPC 预设模板
        /// </summary>
        public static CharacterRandomPreset CreateTemplate(CharacterRandomPreset source, QuackNPCDefinition def)
        {
            if (source == null) return null;
            
            var config = def.Config;
            if (_templateCache.TryGetValue(def.ID, out var cached)) return cached;

            CharacterRandomPreset preset = UnityEngine.Object.Instantiate(source);
            preset.name = $"{source.name}_QuackNPC_{config.CustomPresetName}";

            if (!string.IsNullOrEmpty(config.DisplayNameKey))
            {
                preset.nameKey = config.DisplayNameKey;
            }

            ApplyConfigToPreset(preset, config);

            _templateCache.Add(def.ID, preset);
            ModLogger.LogDebug($"成功创建 NPC 模板: {def.ID} ({def.ModID})");
            return preset;
        }

        public static CharacterRandomPreset GetTemplate(string cacheKey)
        {
            _templateCache.TryGetValue(cacheKey, out var preset);
            return preset;
        }

        public static void DestroyTemplate(string id)
        {
            if (_templateCache.TryGetValue(id, out var preset))
            {
                _templateCache.Remove(id);
                if (preset != null) UnityEngine.Object.Destroy(preset);
            }
        }
        
        public static void ApplyConfigToPreset(CharacterRandomPreset preset, QuackNPCConfig config)
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
            if (config.IsVehicle.HasValue) preset.isVehicle = config.IsVehicle.Value;
            if (config.OnlyMoveForward.HasValue) preset.onlyMoveForward = config.OnlyMoveForward.Value;

            // --- 10. 背包初始物品注入 ---
            if (config.CustomItemIDs != null && config.CustomItemIDs.Count > 0)
            {
                SetupInventory(preset, config.CustomItemIDs);
            }
        }
        
        private static void SetupInventory(CharacterRandomPreset preset, List<int> itemIDs)
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