using System;
using System.Collections.Generic;
using FastModdingLib;
using ItemStatsSystem;

namespace TarkovStimulants.Items
{
    public static class TarkovStimulantsItems
    {
        public static ItemData Cookie = new ItemData
        {
            itemId = 999001,
            order = 100,
            localizationKey = "Cookie",
            localizationDesc = "Cookie_Desc",
            weight = 0.1f,
            value = 500,
            quality = 4,
            displayQuality = DisplayQuality.Blue,
            maxStackCount = 1,
            tags = new List<string> { "Food" },
            spritePath = "items/Cookie.png",
            usages = new UsageData
            {
                actionSound = "SFX/Item/use_food",
                useSound = string.Empty,
                useTime = 2.0f,
                useDurability = false,
                behaviors = new List<UsageBehaviorData>
                {
                    new FoodData
                    {
                        energyValue = 30f,
                        waterValue = -5f
                    },
                    new AddBuffData()
                    {
                        buff = 999001,
                        chance = 1.0f,
                    }
                    // new QuackCookieData
                    // {
                    //     buffId = 999001,
                    //     popMessage = "嘎！生命力涌现！"
                    // },
                }
            }
        };
    }
}