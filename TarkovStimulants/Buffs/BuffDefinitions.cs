using System.Collections.Generic;
using System.Reflection;
using System.IO;
using QuackCore.AttributeModifier;
using QuackCore.BuffSystem;
using QuackCore.BuffSystem.Effects;
using QuackCore.BuffSystem.Logic;
using QuackCore.Constants;

namespace TarkovStimulants.Buffs
{
    public static class BuffDefinitions
    {
        private static readonly string DllDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static string GetIconPath(string fileName) 
            => Path.Combine(DllDir, $"assets/textures/buffs/{fileName}.png");

        public static readonly QuackBuffDefinition ETGc_Buff = 
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "eTGc_Buff", 999001, 15f, GetIconPath("eTG-c")))
                .SetCustomLogic(new RegenerationLogic(0.05f, 0.5f, -1,true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.EnergyCost, 2f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.WaterCost, 2f, true));

        public static readonly QuackBuffDefinition SJ12_Buff =
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "SJ12_Buff", 999002, 120f,
                    GetIconPath("SJ12")))
                .SetCustomLogic(new EnegyWaterRestoreLogic(0.1f, 0.3f, 2f))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.SenseRange, 1.2f, true));

        public static readonly QuackBuffDefinition Propital_Buff =
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "Propital_Buff", 999003, 180f,
                    GetIconPath("Propital")))
                .SetCustomLogic(new RegenerationLogic(0.01f, 1f, -1f, false))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.MaxHealth, 1.2f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.EnergyCost, 0.9f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.WaterCost, 0.9f, true));

        public static readonly QuackBuffDefinition SJ6_Buff =
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "SJ6_Buff", 999004, 90f, GetIconPath("SJ6")))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.WalkSpeed, 1.2f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.RunSpeed, 1.2f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.Stamina, 1.5f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.StaminaRecoverRate, 1.5f, true));

        public static readonly QuackBuffDefinition MULE_Buff = 
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "MULE_Buff", 999005, 240f, GetIconPath("MULE")))
                .SetCustomLogic(new RegenerationLogic(-0.002f, 1.0f, -1f, false))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.MaxWeight, 2f, true));

        public static readonly QuackBuffDefinition Adrenaline_Buff = 
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "Adrenaline_Buff", 999006, 60f, GetIconPath("Adrenaline")))
                .SetCustomLogic(new RegenerationLogic(0.05f, 1f, 15f, false))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.Stamina, 1.1f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.GunDamageMultiplier, 1.1f, true));

        public static readonly QuackBuffDefinition Meldonin_Buff = 
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "Meldonin_Buff", 999007, 180f, GetIconPath("Meldonin")))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.WalkSpeed, 1.3f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.RunSpeed, 1.3f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.Stamina, 1.3f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.StaminaRecoverRate, 1.3f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.EnergyCost, 1.2f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.WaterCost, 1.2f, true));
        
        public static readonly List<QuackBuffDefinition> AllBuffs = new List<QuackBuffDefinition>
        {
            ETGc_Buff,
            SJ12_Buff,
            Propital_Buff,
            SJ6_Buff,
            MULE_Buff,
            Adrenaline_Buff,
            Meldonin_Buff
        };
    }
}