using System;
using System.Collections.Generic;
using Duckov;
using UnityEngine;

namespace QuackCore.NPC
{
    /// <summary>
    /// NPC 配置类。
    /// 字段使用可空类型（?），未赋值时将自动继承原版预设（BasePreset）的属性。
    /// </summary>
    [Serializable]
    public class QuackNPCConfig
    {
        [Header("基础信息")]
        public string BasePresetName = "EnemyPreset_Scav";
        public string CustomPresetName = null;
        public string DisplayNameKey = null;
        public Teams? Team = null;
        public CharacterIconTypes? IconType = null;
        public AudioManager.VoiceType? VoiceType = null;
        public AudioManager.FootStepMaterialType? FootstepType = null;
        
        [Header("核心")]
        public float? Health = null;
        public int? Exp = null;
        public bool? HasSoul = null;
        public bool? ShowName = null;
        public bool? ShowHealthBar = null;
        public bool? CanDieIfNotRaidMap = null;
        public bool? PushCharacter = null;
        public float? MoveSpeedFactor = null;

        [Header("感知")]
        public bool? SetActiveByPlayerDistance = null;
        public float? SightDistance = null;
        public float? SightAngle = null;
        public float? HearingAbility = null;
        public float? NightVisionAbility = null;
        public float? ForgetTime = null;
        public float? ForceTracePlayerDistance = null;
        public float? MinTraceTargetChance = null;
        public float? MaxTraceTargetChance = null;

        [Header("枪械")]
        public float? DamageMultiplier = null;
        public float? BulletSpeedMultiplier = null;
        public float? GunDistanceMultiplier = null;
        public float? GunScatterMultiplier = null;
        public float? ScatterMultiIfTargetRunning = null;
        public float? ScatterMultiIfOffScreen = null;
        public float? GunCritRateGain = null;
        public float? ShootDelay = null;
        public Vector2? ShootTimeRange = null;
        public Vector2? ShootTimeSpaceRange = null;
        public bool? ShootCanMove = null;
        public bool? DefaultWeaponOut = null;

        [Header("AI相关")]
        public bool? SetMeleeDamageMultiplier = null;
        public float? MeleeDamageMultiplier = null;
        public float? AiCombatFactor = null;
        public float? PatrolRange = null;
        public float? CombatMoveRange = null;
        public Vector2? CombatMoveTimeRange = null;
        public float? PatrolTurnSpeed = null;
        public float? CombatTurnSpeed = null;
        public bool? CanDash = null;
        public Vector2? DashCoolTimeRange = null;
        public bool? CanTalk = null;

        [Header("技能")]
        public bool? HasSkill = null;
        public float? HasSkillChance = null;
        public Vector2? SkillCoolTimeRange = null;
        public float? SkillSuccessChance = null;
        public float? ItemSkillChance = null;
        public float? ItemSkillCoolTime = null;

        [Header("元素抗性")]
        public float? ResistPhysics = null;
        public float? ResistFire = null;
        public float? ResistIce = null;
        public float? ResistPoison = null;
        public float? ResistElectricity = null;
        public float? ResistSpace = null;
        public float? ResistGhost = null;

        [Header("物品与掉落")]
        public bool? DropBoxOnDead = null;
        public int? WantItem = null;
        public float? HasCashChance = null;
        public Vector2Int? CashRange = null;
        public Vector2? BulletCountRange = null;

        public List<int> CustomItemIDs = null;

        [Header("载具？")]
        public bool? IsVehicle = null;
        public bool? OnlyMoveForward = null;
    }
}