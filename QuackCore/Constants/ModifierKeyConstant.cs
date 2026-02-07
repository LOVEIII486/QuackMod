namespace QuackCore.Constants
{
    public static class ModifierKeyConstant
    {
        public static class Stat
        {
            public const string MaxHealth = "MaxHealth"; 
            public const string Stamina = "Stamina";
            public const string StaminaDrainRate = "StaminaDrainRate";
            public const string StaminaRecoverRate = "StaminaRecoverRate";
            public const string StaminaRecoverTime = "StaminaRecoverTime";
            public const string MaxEnergy = "MaxEnergy";
            public const string EnergyCost = "EnergyCost";
            public const string MaxWater = "MaxWater";
            public const string WaterCost = "WaterCost";
            public const string MaxWeight = "MaxWeight";
            public const string FoodGain = "FoodGain";
            public const string HealGain = "HealGain";

            public const string WalkSpeed = "WalkSpeed";
            public const string WalkAcc = "WalkAcc";
            public const string RunSpeed = "RunSpeed";
            public const string RunAcc = "RunAcc";
            public const string TurnSpeed = "TurnSpeed";
            public const string AimTurnSpeed = "AimTurnSpeed";
            public const string DashSpeed = "DashSpeed";
            public const string Moveability = "Moveability";

            public const string GunDamageMultiplier = "GunDamageMultiplier";
            public const string GunShootSpeedMultiplier = "GunShootSpeedMultiplier";
            public const string ReloadSpeedGain = "ReloadSpeedGain";
            public const string GunCritRateGain = "GunCritRateGain";
            public const string GunCritDamageGain = "GunCritDamageGain";
            public const string BulletSpeedMultiplier = "BulletSpeedMultiplier";
            public const string RecoilControl = "RecoilControl";
            public const string GunScatterMultiplier = "GunScatterMultiplier";
            public const string GunDistanceMultiplier = "GunDistanceMultiplier";
            
            public const string MeleeDamageMultiplier = "MeleeDamageMultiplier";
            public const string MeleeCritRateGain = "MeleeCritRateGain";
            public const string MeleeCritDamageGain = "MeleeCritDamageGain";

            public const string HeadArmor = "HeadArmor";
            public const string BodyArmor = "BodyArmor";
            public const string ElementFactor_Physics = "ElementFactor_Physics";
            public const string ElementFactor_Fire = "ElementFactor_Fire";
            public const string ElementFactor_Poison = "ElementFactor_Poison";
            public const string ElementFactor_Electricity = "ElementFactor_Electricity";
            public const string ElementFactor_Space = "ElementFactor_Space";
            public const string ElementFactor_Ghost = "ElementFactor_Ghost";
            public const string ElementFactor_Ice = "ElementFactor_Ice";

            public const string ViewDistance = "ViewDistance";
            public const string ViewAngle = "ViewAngle";
            public const string HearingAbility = "HearingAbility";
            public const string SenseRange = "SenseRange";
            public const string WalkSoundRange = "WalkSoundRange";
            public const string RunSoundRange = "RunSoundRange";
            public const string VisableDistanceFactor = "VisableDistanceFactor";

            public const string GasMask = "GasMask";
            public const string InventoryCapacity = "InventoryCapacity";
            public const string PetCapcity = "PetCapcity";
            public const string FlashLight = "FlashLight";
            
            public const string StormProtection = "StormProtection";
            public const string ColdProtection = "ColdProtection";
            public const string HeatProtection = "HeatProtection";
    
            public const string WaterEnergyRecoverMultiplier = "WaterEnergyRecoverMultiplier";
            public const string NightVisionAbility = "NightVisionAbility";
            public const string DashCanControl = "DashCanControl";
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