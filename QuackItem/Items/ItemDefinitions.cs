using System.Collections.Generic;
using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Constants;
using QuackCore.Items;
using QuackCore.Items.UsageData;

namespace QuackItem.Items
{
    public static class ItemDefinitions
    {
        public static readonly QuackItemDefinition Item_GoldenCarrot = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777001,
                order = 100,
                localizationKey = "Item_GoldenCarrot",
                localizationDesc = "Item_GoldenCarrot_Desc",
                weight = 0.2f,
                value = 648,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 1,
                tags = new List<string> { "Food" },
                spritePath = "items/GoldenCarrot.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_food",
                    useSound = string.Empty,
                    useTime = 1.0f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new QuackSpawnNPCData 
                        { 
                            basePresetName = NPCPresetNames.Vehicle.VehicleTest,
                        }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Fo,
                MaxStock = 3,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = true
            },
            Crafting = null,
            Decompose = null
        };
        
        public static readonly QuackItemDefinition Item_GoldenCarrot2 = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777002,
                order = 100,
                localizationKey = "Item_GoldenCarrot2",
                localizationDesc = "Item_GoldenCarrot2_Desc",
                weight = 0.2f,
                value = 648,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 1,
                tags = new List<string> { "Food" },
                spritePath = "items/GoldenCarrot2.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_food",
                    useSound = string.Empty,
                    useTime = 1.0f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new QuackSpawnNPCData 
                        { 
                            basePresetName = NPCPresetNames.Vehicle.VehicleTest2,
                        }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Fo,
                MaxStock = 3,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = true
            },
            Crafting = null,
            Decompose = null
        };

        public static readonly List<QuackItemDefinition> AllQuackItems = new List<QuackItemDefinition>
        {
            Item_GoldenCarrot,
            Item_GoldenCarrot2
        };
    }
}