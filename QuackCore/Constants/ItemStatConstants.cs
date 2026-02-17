namespace QuackCore.Constants
{
    /// <summary>
    /// 物品属性统计常量定义。
    /// </summary>
    public static class ItemStatsConstants
    {
        #region 1. 基础消耗与回复 (Usage & Recovery)
        public const string DurabilityUsage = "durabilityUsage";
        public const string HealValue = "HealValue";
        public const string UseTime = "useTime";
        public const string WaterValue = "WaterValue";
        public const string EnergyValue = "EnergyValue";
        public const string CurrentEnergy = "CurrentEnergy";
        public const string CurrentWater = "CurrentWater";
        public const string FoodGain = "FoodGain";
        public const string HealGain = "HealGain";
        public const string WaterEnergyRecoverMultiplier = "WaterEnergyRecoverMultiplier";
        #endregion

        #region 2. 角色移动与生存 (Movement & Survival)
        public const string WalkSpeed = "WalkSpeed";
        public const string RunSpeed = "RunSpeed";
        public const string WalkAcc = "WalkAcc";
        public const string RunAcc = "RunAcc";
        public const string Moveability = "Moveability";
        public const string MoveSpeedMultiplier = "MoveSpeedMultiplier";
        public const string MaxHealth = "MaxHealth";
        public const string MaxEnergy = "MaxEnergy";
        public const string MaxWater = "MaxWater";
        public const string MaxWeight = "MaxWeight";
        public const string HungerDurability = "HungerDurability";
        public const string EnergyCost = "EnergyCost";
        public const string WaterCost = "WaterCost";
        #endregion

        #region 3. 体力系统 (Stamina System)
        public const string Stamina = "Stamina";
        public const string StaminaDrainRate = "StaminaDrainRate";
        public const string StaminaRecoverRate = "StaminaRecoverRate";
        public const string StaminaRecoverTime = "StaminaRecoverTime";
        public const string StaminaCost = "StaminaCost";
        #endregion

        #region 4. 武器与战斗基础 (Combat & Weapon Base)
        public const string Damage = "Damage";
        public const string DamageMultiplier = "damageMultiplier";
        public const string Attack = "Attack";
        public const string CritRate = "CritRate";
        public const string CritRateGain = "CritRateGain";
        public const string CritDamageFactor = "CritDamageFactor";
        public const string CritDamageFactorGain = "CritDamageFactorGain";
        public const string ArmorPiercing = "ArmorPiercing";
        public const string ArmorPiercingGain = "ArmorPiercingGain";
        public const string ArmorBreak = "ArmorBreak";
        public const string ArmorBreakGain = "ArmorBreakGain";
        public const string BleedChance = "BleedChance";
        public const string BuffChance = "BuffChance";
        public const string BuffChanceMultiplier = "buffChanceMultiplier";
        public const string BleedChanceInternal = "bleedChance";
        #endregion

        #region 5. 枪械特定 (Gun Specific)
        public const string GunDamageMultiplier = "GunDamageMultiplier";
        public const string GunDistanceMultiplier = "GunDistanceMultiplier";
        public const string GunShootSpeedMultiplier = "GunShootSpeedMultiplier";
        public const string GunCritRateGain = "GunCritRateGain";
        public const string GunCritDamageGain = "GunCritDamageGain";
        public const string GunScatterMultiplier = "GunScatterMultiplier";
        public const string ShootSpeed = "ShootSpeed";
        public const string ShootSpeedGainEachShoot = "ShootSpeedGainEachShoot";
        public const string ShootSpeedGainByShootMax = "ShootSpeedGainByShootMax";
        public const string Capacity = "Capacity";
        public const string ReloadTime = "ReloadTime";
        public const string ReloadSpeedGain = "ReloadSpeedGain";
        public const string BurstCount = "BurstCount";
        public const string BulletSpeed = "BulletSpeed";
        public const string BulletSpeedMultiplier = "BulletSpeedMultiplier";
        public const string BulletDistance = "BulletDistance";
        public const string BulletCount = "BulletCount";
        public const string Penetrate = "Penetrate";
        public const string ShotCount = "ShotCount";
        public const string ShotAngle = "ShotAngle";
        public const string Caliber = "Caliber";
        public const string TraceAbility = "TraceAbility";
        public const string DmgOverDistance = "DmgOverDistance";
        public const string BlockBullet = "BlockBullet";
        #endregion

        #region 6. 后坐力与散布 (Recoil & Scatter)
        public const string RecoilControl = "RecoilControl";
        public const string RecoilScaleV = "RecoilScaleV";
        public const string RecoilScaleH = "RecoilScaleH";
        public const string RecoilVMin = "RecoilVMin";
        public const string RecoilVMax = "RecoilVMax";
        public const string RecoilHMin = "RecoilHMin";
        public const string RecoilHMax = "RecoilHMax";
        public const string RecoilTime = "RecoilTime";
        public const string RecoilRecover = "RecoilRecover";
        public const string RecoilRecoverTime = "RecoilRecoverTime";
        public const string DefaultScatter = "DefaultScatter";
        public const string DefaultScatterADS = "DefaultScatterADS";
        public const string MaxScatter = "MaxScatter";
        public const string MaxScatterADS = "MaxScatterADS";
        public const string ScatterFactor = "ScatterFactor";
        public const string ScatterFactorADS = "ScatterFactorADS";
        public const string ScatterGrow = "ScatterGrow";
        public const string ScatterGrowADS = "ScatterGrowADS";
        public const string ScatterRecover = "ScatterRecover";
        public const string ScatterRecoverADS = "ScatterRecoverADS";
        #endregion

        #region 7. 近战特定 (Melee Specific)
        public const string MeleeDamageMultiplier = "MeleeDamageMultiplier";
        public const string MeleeCritRateGain = "MeleeCritRateGain";
        public const string MeleeCritDamageGain = "MeleeCritDamageGain";
        public const string AttackRange = "AttackRange";
        public const string AttackSpeed = "AttackSpeed";
        public const string DealDamageTime = "DealDamageTime";
        #endregion

        #region 8. 护甲与防护 (Armor & Protection)
        public const string BodyArmor = "BodyArmor";
        public const string HeadArmor = "HeadArmor";
        public const string GasMask = "GasMask";
        public const string StormProtection = "StormProtection";
        public const string FireProtection = "FireProtection";
        public const string ElecProtection = "ElecProtection";
        public const string ColdProtection = "ColdProtection";
        public const string HeatProtection = "HeatProtection";
        #endregion

        #region 9. 元素承伤倍率 (Element Factors)
        public const string ElementFactor_Physics = "ElementFactor_Physics";
        public const string ElementFactor_Fire = "ElementFactor_Fire";
        public const string ElementFactor_Poison = "ElementFactor_Poison";
        public const string ElementFactor_Electricity = "ElementFactor_Electricity";
        public const string ElementFactor_Space = "ElementFactor_Space";
        public const string ElementFactor_Ghost = "ElementFactor_Ghost";
        public const string ElementFactor_Ice = "ElementFactor_Ice";
        #endregion

        #region 10. 感知与隐蔽 (Sense & Stealth)
        public const string ViewAngle = "ViewAngle";
        public const string ViewDistance = "ViewDistance";
        public const string SenseRange = "SenseRange";
        public const string SoundVisable = "SoundVisable";
        public const string HearingAbility = "HearingAbility";
        public const string WalkSoundRange = "WalkSoundRange";
        public const string RunSoundRange = "RunSoundRange";
        public const string SoundRange = "SoundRange";
        public const string VisableDistanceFactor = "VisableDistanceFactor";
        #endregion

        #region 11. 功能性与工具 (Functional & Tools)
        public const string NightVisionAbility = "NightVisionAbility";
        public const string NightVisionType = "NightVisionType";
        public const string FlashLight = "FlashLight";
        public const string InventoryCapacity = "InventoryCapacity";
        public const string PetCapcity = "PetCapcity";
        public const string FishingTime = "FishingTime"; // 垂钓能力
        public const string FishingQualityFactor = "FishingQualityFactor"; // 垂钓运气
        public const string FishingDifficulty = "FishingDifficulty";  // 决定鱼的稀有度
        public const string WeaponRepairLossFactor = "WeaponRepairLossFactor";
        public const string EquipmentRepairLossFactor = "EquipmentRepairLossFactor";
        public const string Performance = "Performance";
        public const string HackRangeFactor = "HackRangeFactor";
        public const string HackTimeFactor = "HackTimeFactor";
        public const string HackSkillLevel = "HackSkillLevel";
        public const string ControlMindType = "ControlMindType";
        public const string ControlMindTime = "ControlMindTime";
        #endregion

        #region 12. 动画与辅助 (Animation & Misc)
        public const string AimTurnSpeed = "AimTurnSpeed";
        public const string TurnSpeed = "TurnSpeed";
        public const string ADSAimDistanceFactor = "ADSAimDistanceFactor";
        public const string ADSTime = "ADSTime";
        public const string AdsWalkSpeedMultiplier = "AdsWalkSpeedMultiplier";
        public const string DashSpeed = "DashSpeed";
        public const string DashCanControl = "DashCanControl";
        public const string ExplosionRange = "ExplosionRange";
        public const string ExplosionDamage = "ExplosionDamage";
        public const string ExplosionDamageMultiplier = "ExplosionDamageMultiplier";
        public const string MaxDurability = "MaxDurability";
        public const string Durability = "Durability";
        public const string DurabilityCost = "DurabilityCost";
        public const string DurabilityLoss = "DurabilityLoss";
        public const string RepairLossRatio = "RepairLossRatio";
        public const string OverrideTriggerMode = "OverrideTriggerMode";
        public const string SFX_Put = "SFX_Put";
        public const string ShowMouth = "ShowMouth";
        public const string ShowHair = "ShowHair";
        public const string GameID = "GameID";
        public const string Count = "Count";
        #endregion
    }
}