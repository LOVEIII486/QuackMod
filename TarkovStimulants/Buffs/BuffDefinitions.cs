using System.Collections.Generic;
using QuackCore.BuffSystem;
using QuackCore.BuffSystem.Logic;

namespace TarkovStimulants.Buffs
{
    public static class BuffDefinitions
    {
        public static readonly QuackBuffDefinition ETGc_Buff = 
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "eTGc_Buff", 999001, 15f))
                .SetCustomLogic(new RegenerationLogic(0.1f, 1.0f, true));

        public static readonly QuackBuffDefinition SJ12_Buff = 
            new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "SJ12_Buff", 999002, 120f))
                .SetCustomLogic(new EnegyWaterRestoreLogic(1.0f, 2.0f,3f));
        
        public static readonly List<QuackBuffDefinition> AllBuffs = new List<QuackBuffDefinition>
        {
            ETGc_Buff,
            SJ12_Buff
        };
    }
}