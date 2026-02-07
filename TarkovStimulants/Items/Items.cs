using System.Collections.Generic;
using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageData;

namespace TarkovStimulants.Items
{
    public static class TarkovStimulantsItems
    {
        public static ItemData Stim_eTGc = new ItemData
        {
            itemId = 999001,
            order = 100,
            localizationKey = "Stim_eTGc",
            localizationDesc = "Stim_eTGc_Desc",
            weight = 0.1f,
            value = 500,
            quality = 3,
            displayQuality = DisplayQuality.Blue,
            maxStackCount = 3,
            tags = new List<string> { "Medic", "Healing" },
            spritePath = "items/eTG-c.png",
            usages = new UsageData
            {
                actionSound = string.Empty,
                useSound = "SFX/Item/use_food",
                useTime = 0.5f,
                useDurability = false,
                behaviors = new List<UsageBehaviorData>
                {
                    new FoodData
                    {
                        energyValue = -30f,
                        waterValue = -40f
                    },
                    new QuackAddBuffData
                    {
                        BuffName = "TarkovStimulants_eTG-c_Buff",
                        Chance = 1.0f
                    }
                }
            }
        };
    }
}