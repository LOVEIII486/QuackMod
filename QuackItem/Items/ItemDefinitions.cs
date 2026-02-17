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
                ForceUnlock = false
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
                quality = 3,
                displayQuality = DisplayQuality.Blue,
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
                        new QuackAddBuffData { buffId = 777003, chance = 1.0f }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Fo,
                MaxStock = 2,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = false
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
                value = 20000,
                quality = 5,
                displayQuality = DisplayQuality.Orange,
                maxStackCount = 1,
                maxDurability = 10,
                tags = new List<string> { },
                spritePath = "items/MimicTearAshes.png",
                usages = new UsageData
                {
                    actionSound = string.Empty,
                    useSound = string.Empty,
                    useTime = 1.0f,
                    useDurability = true,
                    durabilityUsage = 1,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new MimicTearAshesData
                        {
                            basePresetName = NPCPresetNames.Enemies.Raider,
                            npcConfigId = "MimicTearAshes"
                        }
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
        
        public static readonly QuackItemDefinition Item_SnowPMCAshes = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777006,
                order = 100,
                localizationKey = "Item_SnowPMCAshes",
                localizationDesc = "Item_SnowPMCAshes_Desc",
                weight = 0.2f,
                value = 20000,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 1,
                maxDurability = 3,
                tags = new List<string> { },
                spritePath = "items/SnowPMCAshes.png",
                usages = new UsageData
                {
                    actionSound = string.Empty,
                    useSound = string.Empty,
                    useTime = 1.0f,
                    useDurability = true,
                    durabilityUsage = 1,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new QuackSpawnNPCData 
                        { 
                            basePresetName = NPCPresetNames.Special.SnowPMC,
                        },
                        new ReturnItemData
                        {
                            display = true,
                            itemTypeID = 388
                        }
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
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_SnowPMCAshes_craft",
                MoneyCost = 0L,
                Materials = new List<(int itemId, long count)>
                {
                    (1368, 1L), //煤球头套
                    (388, 3L), //实体比特币
                },
                ResultCount = 1,
                Workbenches = new string[] { WorkbenchIDs.WorkBenchAdvanced },
                UnlockByDefault = true,
                RequirePerk = "",
                HideInIndex = false,
                LockInDemo = false
            },
            Decompose = null
        };
        
        public static readonly QuackItemDefinition Item_AmmoCase = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777007,
                order = 100,
                localizationKey = "Item_AmmoCase",
                localizationDesc = "Item_AmmoCase_Desc",
                weight = 1.2f,
                value = 10000,
                quality = 5,
                displayQuality = DisplayQuality.Orange,
                maxStackCount = 1,
                tags = new List<string> { "Continer" },
                spritePath = "items/AmmoCase.png",
                usages = null
            },
            Slots = new List<QuackItemDefinition.SlotConfig>
            {
                new QuackItemDefinition.SlotConfig { Key = "0", RequireTags = new List<string> { "Bullet" } },
                new QuackItemDefinition.SlotConfig { Key = "1", RequireTags = new List<string> { "Bullet" } },
                new QuackItemDefinition.SlotConfig { Key = "2", RequireTags = new List<string> { "Bullet" } },
                new QuackItemDefinition.SlotConfig { Key = "3", RequireTags = new List<string> { "Bullet" } },
                new QuackItemDefinition.SlotConfig { Key = "4", RequireTags = new List<string> { "Bullet" } },
                new QuackItemDefinition.SlotConfig { Key = "5", RequireTags = new List<string> { "Bullet" } },
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Weapon,
                MaxStock = 1,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = false
            }
        };
        
        public static readonly QuackItemDefinition Item_GunTurretBeacon = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777008,
                order = 100,
                localizationKey = "Item_GunTurretBeacon",
                localizationDesc = "Item_GunTurretBeacon_Desc",
                weight = 0.2f,
                value = 13000,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 1,
                maxDurability = 3,
                tags = new List<string> { "Tool", "Electric" },
                spritePath = "items/GunTurretBeacon.png",
                usages = new UsageData
                {
                    actionSound = string.Empty,
                    useSound = string.Empty,
                    useTime = 1.0f,
                    useDurability = true,
                    durabilityUsage = 1,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new QuackSpawnNPCData 
                        { 
                            basePresetName = NPCPresetNames.Special.GunTurret,
                            npcConfigId = "GunTurretBeacon"
                        },
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
        
        
        public static readonly QuackItemDefinition Item_AGDumbbell = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777009,
                order = 100,
                localizationKey = "Item_AGDumbbell",
                localizationDesc = "Item_AGDumbbell_Desc",
                weight = -46.0f,
                value = 31321,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 1,
                tags = new List<string> { "Daily", "Luxury", "MeleeWeapon" },
                spritePath = "items/AGDumbbell.png",
                usages = null
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Mud,
                MaxStock = 1,
                PriceFactor = 1.0f,
                Probability = 0.5f,
                ForceUnlock = false
            },
            Crafting = null,
            Decompose = null,
            BaseItemId = 1175,
            Melee = new QuackItemDefinition.MeleeConfig
            {
                Element = "poison"
            },
        };

        public static readonly List<QuackItemDefinition> AllQuackItems = new List<QuackItemDefinition>
        {
            Item_GoldenCarrot,
            Item_GoldenCarrot2,
            Food_LifeFruit,
            Item_ReturnOrb,
            Item_MimicTearAshes,
            Item_SnowPMCAshes,
            Item_AmmoCase,
            Item_GunTurretBeacon,
            Item_AGDumbbell
        };
    }
}