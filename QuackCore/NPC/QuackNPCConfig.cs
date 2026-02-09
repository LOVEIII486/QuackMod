using System;
using System.Collections.Generic;
using Duckov;
using UnityEngine;

namespace QuackCore.NPC
{
    [Serializable]
    public class QuackNPCConfig
    {
        [Header("--- 基础身份 Identity ---")]
        public string BasePresetName = "EnemyPreset_Scav"; 
        public string CustomName = ""; 
        public Teams Team = Teams.all;
        public CharacterIconTypes IconType = CharacterIconTypes.none;
        public AudioManager.VoiceType VoiceType = AudioManager.VoiceType.Duck;
        public AudioManager.FootStepMaterialType FootstepType = AudioManager.FootStepMaterialType.organic;
        
        [Header("--- 核心属性 Stats ---")]
        public float Health = 100f;
        public int Exp = 100;
        public bool HasSoul = true;
        public bool ShowName = true;
        public bool ShowHealthBar = true;
        public bool CanDieIfNotRaidMap = false;
        public bool PushCharacter = true;
        public float MoveSpeedFactor = 1f;

        [Header("--- AI 感知 Perception ---")]
        public bool SetActiveByPlayerDistance = true;
        public float SightDistance = 17f;
        public float SightAngle = 100f;
        public float HearingAbility = 1f;
        public float NightVisionAbility = 0.5f;
        public float ForgetTime = 8f;
        public float ForceTracePlayerDistance = 0f;
        [Range(0f, 1f)] public float MinTraceTargetChance = 1f;
        [Range(0f, 1f)] public float MaxTraceTargetChance = 1f;

        [Header("--- 远程战斗 Shoot ---")]
        public float DamageMultiplier = 1f;
        public float BulletSpeedMultiplier = 1f;
        [Range(1f, 2f)] public float GunDistanceMultiplier = 1f;
        public float GunScatterMultiplier = 1f;
        public float ScatterMultiIfTargetRunning = 3f;
        public float ScatterMultiIfOffScreen = 4f;
        public float GunCritRateGain = 0f;
        public float ShootDelay = 0.2f;
        public Vector2 ShootTimeRange = new Vector2(0.4f, 1.5f);
        public Vector2 ShootTimeSpaceRange = new Vector2(2f, 3f);
        public bool ShootCanMove = true;
        public bool DefaultWeaponOut = true;

        [Header("--- 近战与战术 Melee & Tactics ---")]
        public bool SetMeleeDamageMultiplier = false;
        public float MeleeDamageMultiplier = 1f;
        public float AiCombatFactor = 1f;
        public float PatrolRange = 8f;
        public float CombatMoveRange = 8f;
        public Vector2 CombatMoveTimeRange = new Vector2(1f, 3f);
        public float PatrolTurnSpeed = 180f;
        public float CombatTurnSpeed = 1200f;
        public bool CanDash = false;
        public Vector2 DashCoolTimeRange = new Vector2(2f, 4f);
        public bool CanTalk = true;

        [Header("--- 技能设定 Skills ---")]
        public bool HasSkill = false;
        [Range(0.01f, 1f)] public float HasSkillChance = 1f;
        public Vector2 SkillCoolTimeRange = Vector2.one;
        [Range(0.01f, 1f)] public float SkillSuccessChance = 1f;
        [Range(0f, 1f)] public float ItemSkillChance = 0.3f;
        public float ItemSkillCoolTime = 6f;

        [Header("--- 元素抗性 Resistances ---")]
        public float ResistPhysics = 1f;
        public float ResistFire = 1f;
        public float ResistIce = 1f;
        public float ResistPoison = 1f;
        public float ResistElectricity = 1f;
        public float ResistSpace = 1f;
        public float ResistGhost = 1f;

        [Header("--- 掉落物品 Items ---")]
        public bool DropBoxOnDead = true;
        public int WantItem = -1;
        public float HasCashChance = 0f;
        public Vector2Int CashRange = Vector2Int.zero;
        public List<int> CustomItemIDs = new List<int>();
        public Vector2 BulletCountRange = Vector2.one;

        [Header("--- 特殊状态 Special ---")]
        public bool IsVehicle = false;
        public bool OnlyMoveForward = false;
    }
}