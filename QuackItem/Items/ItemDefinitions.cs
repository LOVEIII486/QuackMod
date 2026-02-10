using System.Collections.Generic;
using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Constants;
using QuackCore.Items;
using QuackCore.Items.UsageData;
using QuackItem.Items.Data;

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
        
        public static readonly QuackItemDefinition Food_LifeFruit = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777003,
                order = 100,
                localizationKey = "Food_LifeFruit",
                localizationDesc = "Food_LifeFruit_Desc",
                weight = 0.1f,
                value = 2000,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 5,
                tags = new List<string> { "Food" },
                spritePath = "items/LifeFruit.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_food",
                    useSound = string.Empty,
                    useTime = 1.0f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new QuackAddBuffData
                        {
                            buffName = "QuackItem_LifeFruit_Buff",
                            chance = 1.0f,
                        }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Fo,
                MaxStock = 2,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = true
            },
            Crafting = null,
            Decompose = null
        };
        
        public static readonly QuackItemDefinition Item_ReturnOrb = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777004,
                order = 100,
                localizationKey = "Item_ReturnOrb",
                localizationDesc = "Item_ReturnOrb_Desc",
                weight = 0.1f,
                value = 3000,
                quality = 5,
                displayQuality = DisplayQuality.Orange,
                maxStackCount = 3,
                //maxDurability = 3,
                tags = new List<string> { "Tool" },
                spritePath = "items/ReturnOrb.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_syringe",
                    useSound = "SFX/Item/use_syringe_success",
                    useTime = 1.0f,
                    useDurability = false,
                    durabilityUsage = 0,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new ReturnOrbData()
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
        
        public static readonly QuackItemDefinition Item_MimicTearAshes = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777005,
                order = 100,
                localizationKey = "Item_MimicTearAshes",
                localizationDesc = "Item_MimicTearAshes_Desc",
                weight = 0.1f,
                value = 6000,
                quality = 5,
                displayQuality = DisplayQuality.Orange,
                maxStackCount = 1,
                maxDurability = 100,
                tags = new List<string> { "Tool" },
                spritePath = "items/MimicTearAshes.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_syringe",
                    useSound = "SFX/Item/use_syringe_success",
                    useTime = 1.0f,
                    useDurability = false,
                    durabilityUsage = 0,
                    behaviors = new List<UsageBehaviorData>
                    {
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Fo,
                MaxStock = 1,
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
            Item_GoldenCarrot2,
            Food_LifeFruit,
            Item_ReturnOrb,
            Item_MimicTearAshes,
        };
    }
}