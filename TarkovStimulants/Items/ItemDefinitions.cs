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
                        new FoodData
                        {
                            energyValue = -15f,
                            waterValue = -20f
                        },
                        new QuackAddBuffData
                        {
                            buffName = "TarkovStimulants_eTGc_Buff",
                            chance = 1.0f
                        }
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
                    (136, 3L),
                    (875, 1L),
                    (14, 1L)
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
                value = 1500,
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
                    (136, 3L),
                    (428, 1L),
                    (132, 1L)
                },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true,
                RequirePerk = "",
                HideInIndex = false,
                LockInDemo = false
            },
        };

        public static readonly List<QuackItemDefinition> AllQuackItems = new List<QuackItemDefinition>
        {
            Stim_eTGc,
            Stim_SJ12
        };
    }
}