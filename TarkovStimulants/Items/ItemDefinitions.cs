using System.Collections.Generic;
using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageData;

namespace TarkovStimulants.Items
{
    public static class ItemDefinitions
    {
        public static readonly ItemData Stim_eTGc = new ItemData
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
                actionSound = "SFX/Item/use_syringe",
                useSound = "SFX/Item/use_syringe_success",
                useTime = 0.5f,
                behaviors = new List<UsageBehaviorData>
                {
                    new FoodData { energyValue = -30f, waterValue = -40f },
                    new QuackAddBuffData { BuffName = "TarkovStimulants_eTGc_Buff", Chance = 1.0f }
                }
            }
        };
        
        public static readonly ItemData Stim_SJ12 = new ItemData
        {
            itemId = 999002,
            order = 101,
            localizationKey = "Stim_SJ12",
            localizationDesc = "Stim_SJ12_Desc",
            weight = 0.1f,
            value = 800,
            quality = 3,
            displayQuality = DisplayQuality.Blue,
            maxStackCount = 3,
            tags = new List<string> { "Medic" },
            spritePath = "items/SJ12.png",
            usages = new UsageData
            {
                actionSound = "SFX/Item/use_syringe",
                useSound = "SFX/Item/use_syringe_success",
                useTime = 0.5f,
                behaviors = new List<UsageBehaviorData>
                {
                    new FoodData { energyValue = 5f, waterValue = 10f },
                    new QuackAddBuffData { BuffName = "TarkovStimulants_SJ12_Buff", Chance = 1.0f }
                }
            }
        };
        
        
        public static readonly List<ItemData> AllItems = new List<ItemData>
        {
            Stim_eTGc,
            Stim_SJ12
        };
    }
}