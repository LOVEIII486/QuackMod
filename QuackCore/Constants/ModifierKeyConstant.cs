namespace QuackCore.Constants
{
    public static class ModifierKeyConstant
    {
        public static class Stat
        {
            public const string MaxHealth = "MaxHealth"; // 最大生命值
            public const string Stamina = "Stamina"; // 体力
            public const string StaminaDrainRate = "StaminaDrainRate"; // 体力消耗
            public const string StaminaRecoverRate = "StaminaRecoverRate"; // 体力恢复
            public const string StaminaRecoverTime = "StaminaRecoverTime"; // 体力恢复时间
            public const string MaxEnergy = "MaxEnergy"; // 最大能量
            public const string EnergyCost = "EnergyCost"; // 能量消耗速率
            public const string MaxWater = "MaxWater"; // 最大水分
            public const string WaterCost = "WaterCost"; // 水分消耗速率
            public const string MaxWeight = "MaxWeight"; // 最大负重
            public const string FoodGain = "FoodGain"; // 食物效率
            public const string HealGain = "HealGain"; // 治疗效率
            public const string HungerDurability = "HungerDurability"; // 最大能量 (耐饿度)

            public const string WalkSpeed = "WalkSpeed"; // 行走速度
            public const string WalkAcc = "WalkAcc"; // 行走加速度
            public const string RunSpeed = "RunSpeed"; // 奔跑速度
            public const string RunAcc = "RunAcc"; // 奔跑加速度
            public const string TurnSpeed = "TurnSpeed"; // 转向速度
            public const string AimTurnSpeed = "AimTurnSpeed"; // 瞄准转向速度
            public const string DashSpeed = "DashSpeed"; // 翻滚速度
            public const string Moveability = "Moveability"; // 移动能力
            public const string DashCanControl = "DashCanControl"; // 翻滚方向控制

            public const string GunDamageMultiplier = "GunDamageMultiplier"; // 枪械伤害倍率
            public const string GunShootSpeedMultiplier = "GunShootSpeedMultiplier"; // 枪械射速倍率
            public const string ReloadSpeedGain = "ReloadSpeedGain"; // 换弹速率
            public const string GunCritRateGain = "GunCritRateGain"; // 枪械暴击率
            public const string GunCritDamageGain = "GunCritDamageGain"; // 枪械爆头伤害
            public const string BulletSpeedMultiplier = "BulletSpeedMultiplier"; // 子弹速度倍率
            public const string RecoilControl = "RecoilControl"; // 后座力控制
            public const string GunScatterMultiplier = "GunScatterMultiplier"; // 枪械散布
            public const string GunDistanceMultiplier = "GunDistanceMultiplier"; // 枪械射程系数

            public const string MeleeDamageMultiplier = "MeleeDamageMultiplier"; // 近战伤害倍率
            public const string MeleeCritRateGain = "MeleeCritRateGain"; // 近战暴击率
            public const string MeleeCritDamageGain = "MeleeCritDamageGain"; // 近战暴击伤害
            public const string Attack = "Attack"; // 攻击力

            public const string HeadArmor = "HeadArmor"; // 头部护甲
            public const string BodyArmor = "BodyArmor"; // 身体护甲
            public const string GasMask = "GasMask"; // 毒气防护
            public const string ElementFactor_Physics = "ElementFactor_Physics"; // 物理承伤倍率
            public const string ElementFactor_Fire = "ElementFactor_Fire"; // 火承伤倍率
            public const string ElementFactor_Poison = "ElementFactor_Poison"; // 毒承伤倍率
            public const string ElementFactor_Electricity = "ElementFactor_Electricity"; // 电承伤倍率
            public const string ElementFactor_Space = "ElementFactor_Space"; // 空间承伤倍率
            public const string ElementFactor_Ghost = "ElementFactor_Ghost"; // 灵承伤倍率
            public const string ElementFactor_Ice = "ElementFactor_Ice"; // 冰承伤倍率

            public const string StormProtection = "StormProtection"; // 风暴防护
            public const string ColdProtection = "ColdProtection"; // 寒冷防护
            public const string HeatProtection = "HeatProtection"; // 炎热防护
            public const string FireProtection = "FireProtection"; // 火防护
            public const string ElecProtection = "ElecProtection"; // 电防护

            public const string ViewDistance = "ViewDistance"; // 视野距离
            public const string ViewAngle = "ViewAngle"; // 视野角度
            public const string HearingAbility = "HearingAbility"; // 听力
            public const string SenseRange = "SenseRange"; // 感知距离
            public const string WalkSoundRange = "WalkSoundRange"; // 行走声音距离
            public const string RunSoundRange = "RunSoundRange"; // 奔跑声音距离
            public const string VisableDistanceFactor = "VisableDistanceFactor"; // 被发现距离系数
            public const string SoundVisable = "SoundVisable"; // 听声辨位
            public const string NightVisionAbility = "NightVisionAbility"; // 夜视能力
            public const string NightVisionType = "NightVisionType"; // 夜视类型

            public const string InventoryCapacity = "InventoryCapacity"; // 背包空间
            public const string PetCapcity = "PetCapcity"; // 宠物背包容量
            public const string WaterEnergyRecoverMultiplier = "WaterEnergyRecoverMultiplier"; // 基地中水分和能量恢复速率
            public const string WeaponRepairLossFactor = "WeaponRepairLossFactor"; // 武器维修损耗
            public const string EquipmentRepairLossFactor = "EquipmentRepairLossFactor"; // 装备维修损耗

            public const string FishingTime = "FishingTime"; // 垂钓能力
            public const string FishingQualityFactor = "FishingQualityFactor"; // 垂钓运气

            public const string HackRangeFactor = "HackRangeFactor"; // 入侵距离系数
            public const string HackTimeFactor = "HackTimeFactor"; // 入侵时间系数
            public const string HackSkillLevel = "HackSkillLevel"; // 入侵装置等级
        }

        public static class AIField
        {
            public const string ReactionTime = "reactionTime";
            public const string BaseReactionTime = "baseReactionTime";
            public const string UpdateValueTimer = "updateValueTimer";

            public const string PatrolTurnSpeed = "patrolTurnSpeed";
            public const string CombatTurnSpeed = "combatTurnSpeed";

            public const string ShootDelay = "shootDelay";
            public const string ShootCanMove = "shootCanMove";

            public const string ShootTimeRange = "shootTimeRange";
            public const string ShootTimeMin = "shootTimeRange.x";
            public const string ShootTimeMax = "shootTimeRange.y";
            public const string ShootSpaceRange = "shootTimeSpaceRange";
            public const string ShootSpaceMin = "shootTimeSpaceRange.x";
            public const string ShootSpaceMax = "shootTimeSpaceRange.y";

            public const string CanDash = "canDash";
            public const string DashCDRange = "dashCoolTimeRange";
            public const string DashCDMin = "dashCoolTimeRange.x";
            public const string DashCDMax = "dashCoolTimeRange.y";

            public const string SightDistance = "sightDistance";
            public const string SightAngle = "sightAngle";
            public const string HearingAbility = "hearingAbility";

            public const string PatrolRange = "patrolRange";
            public const string CombatMoveRange = "combatMoveRange";
            public const string ForgetTime = "forgetTime";

            public const string HasSkill = "hasSkill";
            public const string SkillChance = "skillSuccessChance";
            public const string SkillCoolTimeRange = "skillCoolTimeRange";
            public const string SkillCoolTimeMin = "skillCoolTimeRange.x";
            public const string SkillCoolTimeMax = "skillCoolTimeRange.y";

            public const string DefaultWeaponOut = "defaultWeaponOut";
        }
    }
}