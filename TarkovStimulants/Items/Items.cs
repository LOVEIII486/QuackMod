using System.Collections.Generic;
using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageData;

/*
SFX/Item/use_food（食物）

SFX/Item/use_meds（医疗用品）

SFX/Item/use_syringe（注射器）

SFX/Item/use_drink（饮料）

SFX/Item/use_pills（药片）
 */
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
                actionSound = "SFX/Item/use_syringe",
                useSound = "SFX/Item/use_syringe_success",
                useTime = 0.5f,
                useDurability = false,
                behaviors = new List<UsageBehaviorData>
                {
                    new FoodData
                    {
                        energyValue = -10f,
                        waterValue = -20f
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