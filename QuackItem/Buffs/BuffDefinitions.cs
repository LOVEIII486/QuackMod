using System.Collections.Generic;
using System.Reflection;
using System.IO;
using QuackCore.BuffSystem;
using QuackCore.BuffSystem.Effects;
using QuackCore.BuffSystem.Logic;
using QuackCore.Constants;

namespace QuackItem.Buffs
{
    public static class BuffDefinitions
    {
        private static readonly string DllDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static string GetIconPath(string fileName)
            => Path.Combine(DllDir, $"assets/textures/buffs/{fileName}.png");

        public static readonly QuackBuffDefinition LifeFruit_Buff =
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("QuackItem", "LifeFruit_Buff", 777003, 60f,
                    GetIconPath("LifeFruit")))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.MaxHealth, 50f, false));

        public static readonly List<QuackBuffDefinition> AllBuffs = new List<QuackBuffDefinition>
        {
            LifeFruit_Buff
        };
    }
}