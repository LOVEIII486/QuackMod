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
        public static readonly QuackItemDefinition Food_Cookie = new QuackItemDefinition
        {
            BaseData = new ItemData
            {
                itemId = 777001,
                order = 100,
                localizationKey = "Food_Cookie",
                localizationDesc = "Food_Cookie_Desc",
                weight = 0.1f,
                value = 3000,
                quality = 5,
                displayQuality = DisplayQuality.Orange,
                maxStackCount = 1,
                tags = new List<string> { "Food" },
                spritePath = "items/Cookie.png",
                usages = new UsageData
                {
                    actionSound = "SFX/Item/use_food",
                    useSound = "SFX/Item/use_food_success",
                    useTime = 0.5f,
                    behaviors = new List<UsageBehaviorData>
                    {
                        new FoodData { energyValue = 10f, waterValue = 20f },
                        new QuackAddBuffData { buffName = "QuackItem_Cookie_Buff", chance = 1.0f }
                    }
                }
            },
            Shop = new QuackItemDefinition.ShopConfig
            {
                MerchantID = MerchantIDs.Mud,
                MaxStock = 1,
                PriceFactor = 1.0f,
                Probability = 1.0f,
                ForceUnlock = true
            },
            Crafting = new QuackItemDefinition.CraftingConfig
            {
                FormulaID = "formula_Cookie_craft",
                MoneyCost = 0L,
                Materials = new List<(int itemId, long count)>
                {
                    (14, 2L) //可乐
                },
                ResultCount = 3,
                Workbenches = new string[] { WorkbenchIDs.MedicStation },
                UnlockByDefault = true,
                RequirePerk = "",
                HideInIndex = false,
                LockInDemo = false
            },
            Decompose = new QuackItemDefinition.DecomposeConfig
            {
                MoneyGain = 0L,
                Results = new List<(int itemId, long count)>
                {
                    (88, 1L)
                }
            }
        };

        public static readonly List<QuackItemDefinition> AllQuackItems = new List<QuackItemDefinition>
        {
            Food_Cookie,
        };
    }
}