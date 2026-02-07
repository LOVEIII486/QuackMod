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

        public static readonly QuackBuffDefinition Cookie_Buff =
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("QuackItem", "Cookie_Buff", 777001, 15f, GetIconPath("Cookie")))
                .AddCustomLogic(new RegenerationLogic(0.05f, 0.5f, -1, true));

        public static readonly List<QuackBuffDefinition> AllBuffs = new List<QuackBuffDefinition>
        {
            Cookie_Buff
        };
    }
}