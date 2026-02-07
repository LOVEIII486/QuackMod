using System.Collections.Generic;
using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Constants;
using QuackCore.Items;
using QuackCore.Items.UsageData;

namespace TarkovStimulants.Items
{
    public static class ItemDefinitions
    {
        public static readonly QuackItemDefinition Stim_eTGc = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 999001,
                order = 100,
                localizationKey = "Stim_eTGc",
                localizationDesc = "Stim_eTGc_Desc",
                weight = 0.1f,
                value = 3000,
                quality = 5,
                displayQuality = DisplayQuality.Orange,
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
                        new FoodData { energyValue = -10f, waterValue = -20f },
                        new QuackAddBuffData { buffName = "TarkovStimulants_eTGc_Buff", chance = 1.0f }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Mud,
                MaxStock = 3,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = true
            },
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_eTGc_craft",
                MoneyCost = 0L,
                Materials = new List<(int itemId, long count)>
                {
                    (136, 3L), //注射器
                    (875, 3L), //回复针
                    (14, 1L) //可乐
                },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true,
                RequirePerk = "",
                HideInIndex = false,
                LockInDemo = false
            },
            // Decompose = new QuackItemDefinition.DecomposeConfig
            // {
            //     MoneyGain = 0L,
            //     Results = new List<(int itemId, long count)>
            //     {
            //         (88, 1L)
            //     }
            // }
        };

        public static readonly QuackItemDefinition Stim_SJ12 = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 999002,
                order = 101,
                localizationKey = "Stim_SJ12",
                localizationDesc = "Stim_SJ12_Desc",
                weight = 0.1f,
                value = 1200,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
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
                        new QuackAddBuffData { buffName = "TarkovStimulants_SJ12_Buff", chance = 1.0f }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Mud,
                MaxStock = 3,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = true
            },
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_SJ12_craft",
                MoneyCost = 0L,
                Materials = new List<(int itemId, long count)>
                {
                    (136, 3L), // 注射器
                    (428, 1L), // 瓶装水
                    (132, 2L) //蛋糕
                },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true,
                RequirePerk = "",
                HideInIndex = false,
                LockInDemo = false
            },
        };

        public static readonly QuackItemDefinition Stim_Propital = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 999003,
                order = 102,
                localizationKey = "Stim_Propital",
                localizationDesc = "Stim_Propital_Desc",
                weight = 0.1f,
                value = 1500,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 3,
                tags = new List<string> { "Medic" },
                spritePath = "items/Propital.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_syringe",
                    useSound = "SFX/Item/use_syringe_success",
                    useTime = 0.5f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new FoodData { energyValue = -5f, waterValue = -10f },
                        new QuackAddBuffData { buffName = "TarkovStimulants_Propital_Buff", chance = 1.0f },
                        new RemoveBuffData { buffID = 1001 },
                        new AddBuffData { buff = 1019, chance = 1f },
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Mud,
                MaxStock = 3,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = true
            },
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_Propital_craft",
                MoneyCost = 0L,
                Materials = new List<(int itemId, long count)>
                {
                    (136, 3L), // 注射器
                    (875, 1L),
                    (1247, 1L)
                },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true
            }
        };

        public static readonly QuackItemDefinition Stim_SJ6 = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 999004,
                order = 103,
                localizationKey = "Stim_SJ6",
                localizationDesc = "Stim_SJ6_Desc",
                weight = 0.1f,
                value = 800,
                quality = 3,
                displayQuality = DisplayQuality.Blue,
                maxStackCount = 3,
                tags = new List<string> { "Medic" },
                spritePath = "items/SJ6.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_syringe",
                    useSound = "SFX/Item/use_syringe_success",
                    useTime = 0.5f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new FoodData { energyValue = -5f, waterValue = -10f },
                        new QuackAddBuffData { buffName = "TarkovStimulants_SJ6_Buff", chance = 1.0f }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
                { MerchantID = MerchantIDs.Mud, MaxStock = 2, ForceUnlock = true },
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_SJ6_craft",
                Materials = new List<(int itemId, long count)> { (136, 3L), (137, 2L), (88, 2L) },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true
            }
        };

        public static readonly QuackItemDefinition Stim_MULE = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 999005,
                order = 104,
                localizationKey = "Stim_MULE",
                localizationDesc = "Stim_MULE_Desc",
                weight = 0.1f,
                value = 1200,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 3,
                tags = new List<string> { "Medic" },
                spritePath = "items/MULE.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_syringe",
                    useSound = "SFX/Item/use_syringe_success",
                    useTime = 0.5f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new FoodData { energyValue = -5f, waterValue = -10f },
                        new QuackAddBuffData { buffName = "TarkovStimulants_MULE_Buff", chance = 1.0f }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
                { MerchantID = MerchantIDs.Mud, MaxStock = 1, ForceUnlock = true },
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_MULE_craft",
                Materials = new List<(int itemId, long count)> { (136, 3L), (132, 3L) },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true
            }
        };

        public static readonly QuackItemDefinition Stim_Adrenaline = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 999006,
                order = 105,
                localizationKey = "Stim_Adrenaline",
                localizationDesc = "Stim_Adrenaline_Desc",
                weight = 0.1f,
                value = 800,
                quality = 3,
                displayQuality = DisplayQuality.Blue,
                maxStackCount = 3,
                tags = new List<string> { "Medic" },
                spritePath = "items/Adrenaline.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_syringe",
                    useSound = "SFX/Item/use_syringe_success",
                    useTime = 0.5f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new FoodData { energyValue = -5f, waterValue = -10f },
                        new QuackAddBuffData { buffName = "TarkovStimulants_Adrenaline_Buff", chance = 1.0f },
                        new AddBuffData { buff = 1082 ,chance = 1.0f }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
                { MerchantID = MerchantIDs.Mud, MaxStock = 5, ForceUnlock = true },
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_Adrenaline_craft",
                Materials = new List<(int itemId, long count)> { (136, 3L), (409, 2L), (88, 1L) },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true
            }
        };

        public static readonly QuackItemDefinition Stim_Meldonin = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 999007,
                order = 106,
                localizationKey = "Stim_Meldonin",
                localizationDesc = "Stim_Meldonin_Desc",
                weight = 0.1f,
                value = 1800,
                quality = 4,
                displayQuality = DisplayQuality.Purple,
                maxStackCount = 3,
                tags = new List<string> { "Medic" },
                spritePath = "items/Meldonin.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_syringe",
                    useSound = "SFX/Item/use_syringe_success",
                    useTime = 0.5f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new FoodData { energyValue = -5f, waterValue = -10f },
                        new QuackAddBuffData { buffName = "TarkovStimulants_Meldonin_Buff", chance = 1.0f }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
                { MerchantID = MerchantIDs.Mud, MaxStock = 2, ForceUnlock = true },
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_Meldonin_craft",
                Materials = new List<(int itemId, long count)> { (136, 3L), (137, 4L), (1180, 1L) },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true
            }
        };

        public static readonly List<QuackItemDefinition> AllQuackItems = new List<QuackItemDefinition>
        {
            Stim_eTGc,
            Stim_SJ12,
            Stim_Propital,
            Stim_SJ6,
            Stim_MULE,
            Stim_Adrenaline,
            Stim_Meldonin
        };
    }
}