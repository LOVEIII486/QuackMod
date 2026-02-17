namespace QuackCore.Constants
{
    public static class ItemPropertyConstants
    {
        /// <summary>
        /// ItemStatsSystem 中的 Stat 属性
        /// 支持动态计算和修正
        /// </summary>
        public static class Stats
        {
            /// <summary>
            /// 瞄准距离
            /// <para>Range: [1, 3.68]</para>
            /// </summary>
            public const string ADSAimDistanceFactor = "ADSAimDistanceFactor";
            /// <summary>
            /// 瞄准时间
            /// <para>Range: [0.1, 1]</para>
            /// </summary>
            public const string ADSTime = "ADSTime";
            /// <summary>
            /// 瞄准移动系数
            /// <para>Range: [0, 0.75]</para>
            /// </summary>
            public const string AdsWalkSpeedMultiplier = "AdsWalkSpeedMultiplier";
            /// <summary>
            /// 瞄准转向速度
            /// <para>Range: [1200, 1200]</para>
            /// </summary>
            public const string AimTurnSpeed = "AimTurnSpeed";
            /// <summary>
            /// 护甲破坏
            /// <para>Range: [0, 0.1]</para>
            /// </summary>
            public const string ArmorBreak = "ArmorBreak";
            /// <summary>
            /// 穿甲等级
            /// <para>Range: [-0.5, 9]</para>
            /// </summary>
            public const string ArmorPiercing = "ArmorPiercing";
            /// <summary>
            /// 攻击力
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string Attack = "Attack";
            /// <summary>
            /// 攻击距离
            /// <para>Range: [1.1, 2.1]</para>
            /// </summary>
            public const string AttackRange = "AttackRange";
            /// <summary>
            /// 攻击速度
            /// <para>Range: [0.2, 4]</para>
            /// </summary>
            public const string AttackSpeed = "AttackSpeed";
            /// <summary>
            /// 流血概率
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string BleedChance = "BleedChance";
            /// <summary>
            /// *Stat_BlockBullet*
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string BlockBullet = "BlockBullet";
            /// <summary>
            /// 身体护甲
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string BodyArmor = "BodyArmor";
            /// <summary>
            /// 负面效果概率
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string BuffChance = "BuffChance";
            /// <summary>
            /// 射程
            /// <para>Range: [8, 42.6]</para>
            /// </summary>
            public const string BulletDistance = "BulletDistance";
            /// <summary>
            /// 子弹速度
            /// <para>Range: [14, 220]</para>
            /// </summary>
            public const string BulletSpeed = "BulletSpeed";
            /// <summary>
            /// 子弹速度倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string BulletSpeedMultiplier = "BulletSpeedMultiplier";
            /// <summary>
            /// 连发数
            /// <para>Range: [1, 3]</para>
            /// </summary>
            public const string BurstCount = "BurstCount";
            /// <summary>
            /// 弹匣容量
            /// <para>Range: [1, 9999]</para>
            /// </summary>
            public const string Capacity = "Capacity";
            /// <summary>
            /// 寒冷防护
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ColdProtection = "ColdProtection";
            /// <summary>
            /// *Stat_ControlMindTime*
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ControlMindTime = "ControlMindTime";
            /// <summary>
            /// *Stat_ControlMindType*
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ControlMindType = "ControlMindType";
            /// <summary>
            /// 暴击伤害倍率
            /// <para>Range: [1, 5]</para>
            /// </summary>
            public const string CritDamageFactor = "CritDamageFactor";
            /// <summary>
            /// 暴击率
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string CritRate = "CritRate";
            /// <summary>
            /// 伤害
            /// <para>Range: [1, 100]</para>
            /// </summary>
            public const string Damage = "Damage";
            /// <summary>
            /// 翻滚方向控制
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string DashCanControl = "DashCanControl";
            /// <summary>
            /// 翻滚速度
            /// <para>Range: [10, 10]</para>
            /// </summary>
            public const string DashSpeed = "DashSpeed";
            /// <summary>
            /// *Stat_DealDamageTime*
            /// <para>Range: [0.12, 0.16]</para>
            /// </summary>
            public const string DealDamageTime = "DealDamageTime";
            /// <summary>
            /// 腰射基础散布
            /// <para>Range: [0, 10]</para>
            /// </summary>
            public const string DefaultScatter = "DefaultScatter";
            /// <summary>
            /// 瞄准初始扩散
            /// <para>Range: [0.143, 0.5]</para>
            /// </summary>
            public const string DefaultScatterADS = "DefaultScatterADS";
            /// <summary>
            /// 超距离伤害系数
            /// <para>Range: [0.3, 0.7]</para>
            /// </summary>
            public const string DmgOverDistance = "DmgOverDistance";
            /// <summary>
            /// 电防护
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ElecProtection = "ElecProtection";
            /// <summary>
            /// 电承伤倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string ElementFactor_Electricity = "ElementFactor_Electricity";
            /// <summary>
            /// 火承伤倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string ElementFactor_Fire = "ElementFactor_Fire";
            /// <summary>
            /// 灵承伤倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string ElementFactor_Ghost = "ElementFactor_Ghost";
            /// <summary>
            /// 冰承伤倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string ElementFactor_Ice = "ElementFactor_Ice";
            /// <summary>
            /// 物理承伤倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string ElementFactor_Physics = "ElementFactor_Physics";
            /// <summary>
            /// 毒承伤倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string ElementFactor_Poison = "ElementFactor_Poison";
            /// <summary>
            /// 空间承伤倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string ElementFactor_Space = "ElementFactor_Space";
            /// <summary>
            /// 能量消耗速率
            /// <para>Range: [8, 8]</para>
            /// </summary>
            public const string EnergyCost = "EnergyCost";
            /// <summary>
            /// 装备维修损耗
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string EquipmentRepairLossFactor = "EquipmentRepairLossFactor";
            /// <summary>
            /// *Stat_ExplosionDamage*
            /// <para>Range: [0, 17]</para>
            /// </summary>
            public const string ExplosionDamage = "ExplosionDamage";
            /// <summary>
            /// 爆炸伤害系数
            /// <para>Range: [0.6, 4]</para>
            /// </summary>
            public const string ExplosionDamageMultiplier = "ExplosionDamageMultiplier";
            /// <summary>
            /// *Stat_ExplosionRange*
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ExplosionRange = "ExplosionRange";
            /// <summary>
            /// 火防护
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string FireProtection = "FireProtection";
            /// <summary>
            /// 垂钓难度
            /// <para>Range: [1, 3.5]</para>
            /// </summary>
            public const string FishingDifficulty = "FishingDifficulty";
            /// <summary>
            /// 垂钓运气
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string FishingQualityFactor = "FishingQualityFactor";
            /// <summary>
            /// 垂钓能力
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string FishingTime = "FishingTime";
            /// <summary>
            /// 手电
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string FlashLight = "FlashLight";
            /// <summary>
            /// 食物效率
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string FoodGain = "FoodGain";
            /// <summary>
            /// 毒气防护
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string GasMask = "GasMask";
            /// <summary>
            /// 枪械爆头伤害
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string GunCritDamageGain = "GunCritDamageGain";
            /// <summary>
            /// 枪械暴击率
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string GunCritRateGain = "GunCritRateGain";
            /// <summary>
            /// 枪械伤害倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string GunDamageMultiplier = "GunDamageMultiplier";
            /// <summary>
            /// 枪械射程系数
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string GunDistanceMultiplier = "GunDistanceMultiplier";
            /// <summary>
            /// 枪械散布
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string GunScatterMultiplier = "GunScatterMultiplier";
            /// <summary>
            /// 枪械射速倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string GunShootSpeedMultiplier = "GunShootSpeedMultiplier";
            /// <summary>
            /// 入侵距离系数
            /// <para>Range: [2, 2]</para>
            /// </summary>
            public const string HackRangeFactor = "HackRangeFactor";
            /// <summary>
            /// 入侵装置等级
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string HackSkillLevel = "HackSkillLevel";
            /// <summary>
            /// 入侵时间系数
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string HackTimeFactor = "HackTimeFactor";
            /// <summary>
            /// 头部护甲
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string HeadArmor = "HeadArmor";
            /// <summary>
            /// 治疗效率
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string HealGain = "HealGain";
            /// <summary>
            /// 听力
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string HearingAbility = "HearingAbility";
            /// <summary>
            /// 炎热防护
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string HeatProtection = "HeatProtection";
            /// <summary>
            /// 最大能量
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string HungerDurability = "HungerDurability";
            /// <summary>
            /// 背包空间
            /// <para>Range: [10, 10]</para>
            /// </summary>
            public const string InventoryCapacity = "InventoryCapacity";
            /// <summary>
            /// 最大能量
            /// <para>Range: [100, 100]</para>
            /// </summary>
            public const string MaxEnergy = "MaxEnergy";
            /// <summary>
            /// 最大生命值
            /// <para>Range: [40, 40]</para>
            /// </summary>
            public const string MaxHealth = "MaxHealth";
            /// <summary>
            /// 腰射最大扩散
            /// <para>Range: [0, 2.295]</para>
            /// </summary>
            public const string MaxScatter = "MaxScatter";
            /// <summary>
            /// 瞄准最大扩散
            /// <para>Range: [0.792, 20]</para>
            /// </summary>
            public const string MaxScatterADS = "MaxScatterADS";
            /// <summary>
            /// 最大水分
            /// <para>Range: [100, 100]</para>
            /// </summary>
            public const string MaxWater = "MaxWater";
            /// <summary>
            /// 最大负重
            /// <para>Range: [45, 45]</para>
            /// </summary>
            public const string MaxWeight = "MaxWeight";
            /// <summary>
            /// 近战暴击伤害
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string MeleeCritDamageGain = "MeleeCritDamageGain";
            /// <summary>
            /// 近战暴击率
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string MeleeCritRateGain = "MeleeCritRateGain";
            /// <summary>
            /// 近战伤害倍率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string MeleeDamageMultiplier = "MeleeDamageMultiplier";
            /// <summary>
            /// 移动能力
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string Moveability = "Moveability";
            /// <summary>
            /// 移动速度系数
            /// <para>Range: [0, 1.3]</para>
            /// </summary>
            public const string MoveSpeedMultiplier = "MoveSpeedMultiplier";
            /// <summary>
            /// 夜视能力
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string NightVisionAbility = "NightVisionAbility";
            /// <summary>
            /// 夜视类型
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string NightVisionType = "NightVisionType";
            /// <summary>
            /// 开枪模式
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string OverrideTriggerMode = "OverrideTriggerMode";
            /// <summary>
            /// 单位穿透
            /// <para>Range: [0, 2]</para>
            /// </summary>
            public const string Penetrate = "Penetrate";
            /// <summary>
            /// *Stat_Performance*
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string Performance = "Performance";
            /// <summary>
            /// 宠物背包容量
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string PetCapcity = "PetCapcity";
            /// <summary>
            /// 后座力控制
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string RecoilControl = "RecoilControl";
            /// <summary>
            /// 最大水平后坐力
            /// <para>Range: [0.444, 0.8]</para>
            /// </summary>
            public const string RecoilHMax = "RecoilHMax";
            /// <summary>
            /// 最小水平后坐力
            /// <para>Range: [-0.556, -0.2]</para>
            /// </summary>
            public const string RecoilHMin = "RecoilHMin";
            /// <summary>
            /// 后坐力恢复
            /// <para>Range: [150, 550]</para>
            /// </summary>
            public const string RecoilRecover = "RecoilRecover";
            /// <summary>
            /// 后坐力恢复起始时间
            /// <para>Range: [0.08, 0.2]</para>
            /// </summary>
            public const string RecoilRecoverTime = "RecoilRecoverTime";
            /// <summary>
            /// 水平后坐力
            /// <para>Range: [10, 190]</para>
            /// </summary>
            public const string RecoilScaleH = "RecoilScaleH";
            /// <summary>
            /// 垂直后坐力
            /// <para>Range: [12.5, 144.5]</para>
            /// </summary>
            public const string RecoilScaleV = "RecoilScaleV";
            /// <summary>
            /// 后坐力持续时间
            /// <para>Range: [0.05, 0.5]</para>
            /// </summary>
            public const string RecoilTime = "RecoilTime";
            /// <summary>
            /// 最大垂直后坐力
            /// <para>Range: [1.023, 1.286]</para>
            /// </summary>
            public const string RecoilVMax = "RecoilVMax";
            /// <summary>
            /// 最小垂直后坐力
            /// <para>Range: [0.714, 0.977]</para>
            /// </summary>
            public const string RecoilVMin = "RecoilVMin";
            /// <summary>
            /// 换弹速率
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ReloadSpeedGain = "ReloadSpeedGain";
            /// <summary>
            /// 换弹时间
            /// <para>Range: [0, 5.8]</para>
            /// </summary>
            public const string ReloadTime = "ReloadTime";
            /// <summary>
            /// 奔跑加速度
            /// <para>Range: [70, 70]</para>
            /// </summary>
            public const string RunAcc = "RunAcc";
            /// <summary>
            /// 奔跑声音距离
            /// <para>Range: [16, 16]</para>
            /// </summary>
            public const string RunSoundRange = "RunSoundRange";
            /// <summary>
            /// 奔跑速度
            /// <para>Range: [6, 6]</para>
            /// </summary>
            public const string RunSpeed = "RunSpeed";
            /// <summary>
            /// 腰射散布
            /// <para>Range: [0, 80]</para>
            /// </summary>
            public const string ScatterFactor = "ScatterFactor";
            /// <summary>
            /// 瞄准散布
            /// <para>Range: [2, 67]</para>
            /// </summary>
            public const string ScatterFactorADS = "ScatterFactorADS";
            /// <summary>
            /// 腰射扩散增长
            /// <para>Range: [0, 1.639]</para>
            /// </summary>
            public const string ScatterGrow = "ScatterGrow";
            /// <summary>
            /// 瞄准扩散增长
            /// <para>Range: [0.101, 24.5]</para>
            /// </summary>
            public const string ScatterGrowADS = "ScatterGrowADS";
            /// <summary>
            /// 腰射扩散恢复
            /// <para>Range: [0.2, 0.8]</para>
            /// </summary>
            public const string ScatterRecover = "ScatterRecover";
            /// <summary>
            /// 瞄准扩散恢复
            /// <para>Range: [0.3, 1]</para>
            /// </summary>
            public const string ScatterRecoverADS = "ScatterRecoverADS";
            /// <summary>
            /// 感知距离
            /// <para>Range: [1.5, 1.5]</para>
            /// </summary>
            public const string SenseRange = "SenseRange";
            /// <summary>
            /// 射速
            /// <para>Range: [0.5, 22]</para>
            /// </summary>
            public const string ShootSpeed = "ShootSpeed";
            /// <summary>
            /// *Stat_ShootSpeedGainByShootMax*
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ShootSpeedGainByShootMax = "ShootSpeedGainByShootMax";
            /// <summary>
            /// *Stat_ShootSpeedGainEachShoot*
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ShootSpeedGainEachShoot = "ShootSpeedGainEachShoot";
            /// <summary>
            /// 单发射击角度
            /// <para>Range: [0, 360]</para>
            /// </summary>
            public const string ShotAngle = "ShotAngle";
            /// <summary>
            /// 单发子弹数
            /// <para>Range: [1, 9]</para>
            /// </summary>
            public const string ShotCount = "ShotCount";
            /// <summary>
            /// 声音距离
            /// <para>Range: [2.2, 45.6]</para>
            /// </summary>
            public const string SoundRange = "SoundRange";
            /// <summary>
            /// 听声辨位
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string SoundVisable = "SoundVisable";
            /// <summary>
            /// 体力
            /// <para>Range: [100, 100]</para>
            /// </summary>
            public const string Stamina = "Stamina";
            /// <summary>
            /// 体力消耗
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string StaminaCost = "StaminaCost";
            /// <summary>
            /// 体力消耗
            /// <para>Range: [7, 7]</para>
            /// </summary>
            public const string StaminaDrainRate = "StaminaDrainRate";
            /// <summary>
            /// 体力恢复
            /// <para>Range: [20, 20]</para>
            /// </summary>
            public const string StaminaRecoverRate = "StaminaRecoverRate";
            /// <summary>
            /// 体力恢复时间
            /// <para>Range: [1.5, 1.5]</para>
            /// </summary>
            public const string StaminaRecoverTime = "StaminaRecoverTime";
            /// <summary>
            /// 风暴防护
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string StormProtection = "StormProtection";
            /// <summary>
            /// 子弹追踪
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string TraceAbility = "TraceAbility";
            /// <summary>
            /// 转向速度
            /// <para>Range: [200, 200]</para>
            /// </summary>
            public const string TurnSpeed = "TurnSpeed";
            /// <summary>
            /// 视野角度
            /// <para>Range: [100, 100]</para>
            /// </summary>
            public const string ViewAngle = "ViewAngle";
            /// <summary>
            /// 视野距离
            /// <para>Range: [37, 37]</para>
            /// </summary>
            public const string ViewDistance = "ViewDistance";
            /// <summary>
            /// 被发现距离系数
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string VisableDistanceFactor = "VisableDistanceFactor";
            /// <summary>
            /// 行走加速度
            /// <para>Range: [50, 50]</para>
            /// </summary>
            public const string WalkAcc = "WalkAcc";
            /// <summary>
            /// 行走声音距离
            /// <para>Range: [8, 8]</para>
            /// </summary>
            public const string WalkSoundRange = "WalkSoundRange";
            /// <summary>
            /// 行走速度
            /// <para>Range: [4, 4]</para>
            /// </summary>
            public const string WalkSpeed = "WalkSpeed";
            /// <summary>
            /// 水分消耗速率
            /// <para>Range: [9, 9]</para>
            /// </summary>
            public const string WaterCost = "WaterCost";
            /// <summary>
            /// 基地中水分和能量恢复速率
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string WaterEnergyRecoverMultiplier = "WaterEnergyRecoverMultiplier";
            /// <summary>
            /// 武器维修损耗
            /// <para>Range: [1, 1]</para>
            /// </summary>
            public const string WeaponRepairLossFactor = "WeaponRepairLossFactor";
        }

        /// <summary>
        /// 对应 Item.Constants 属性
        /// 物品的配置参数
        /// </summary>
        public static class ItemConstants
        {
            /// <summary>
            /// <para>Range: [0, 35]</para>
            /// </summary>
            public const string ArmorBreakGain = "ArmorBreakGain";
            /// <summary>
            /// <para>Range: [-3, 9]</para>
            /// </summary>
            public const string ArmorPiercingGain = "ArmorPiercingGain";
            /// <summary>
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string bleedChance = "bleedChance";
            /// <summary>
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string buffChanceMultiplier = "buffChanceMultiplier";
            /// <summary>
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string Caliber = "Caliber";
            /// <summary>
            /// <para>Range: [0, 1]</para>
            /// </summary>
            public const string CritDamageFactorGain = "CritDamageFactorGain";
            /// <summary>
            /// <para>Range: [-0.1, 0]</para>
            /// </summary>
            public const string CritRateGain = "CritRateGain";
            /// <summary>
            /// <para>Range: [0.15, 3]</para>
            /// </summary>
            public const string damageMultiplier = "damageMultiplier";
            /// <summary>
            /// <para>Range: [0.04, 15]</para>
            /// </summary>
            public const string DurabilityCost = "DurabilityCost";
            /// <summary>
            /// <para>Range: [0, 75]</para>
            /// </summary>
            public const string ExplosionDamage = "ExplosionDamage";
            /// <summary>
            /// <para>Range: [0, 3.5]</para>
            /// </summary>
            public const string ExplosionRange = "ExplosionRange";
            /// <summary>
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string GameID = "GameID";
            /// <summary>
            /// <para>Range: [0, 400]</para>
            /// </summary>
            public const string MaxDurability = "MaxDurability";
            /// <summary>
            /// <para>Range: [0, 0.2]</para>
            /// </summary>
            public const string RepairLossRatio = "RepairLossRatio";
            /// <summary>
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string SFX_Put = "SFX_Put";
            /// <summary>
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ShowHair = "ShowHair";
            /// <summary>
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string ShowMouth = "ShowMouth";
        }

        /// <summary>
        /// 对应 Item.Variables 属性
        /// 物品的运行时状态
        /// </summary>
        public static class Variables
        {
            /// <summary>
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string BulletCount = "BulletCount";
            /// <summary>
            /// <para>Range: [0, 0]</para>
            /// </summary>
            public const string Count = "Count";
            /// <summary>
            /// <para>Range: [50, 50]</para>
            /// </summary>
            public const string CurrentEnergy = "CurrentEnergy";
            /// <summary>
            /// <para>Range: [50, 50]</para>
            /// </summary>
            public const string CurrentWater = "CurrentWater";
            /// <summary>
            /// <para>Range: [0, 400]</para>
            /// </summary>
            public const string Durability = "Durability";
            /// <summary>
            /// <para>Range: [0, 0.641]</para>
            /// </summary>
            public const string DurabilityLoss = "DurabilityLoss";
            /// <summary>
            /// <para>Range: [0, 90]</para>
            /// </summary>
            public const string MaxDurability = "MaxDurability";
        }
    }
}