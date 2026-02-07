using System.Collections.Generic;
using FastModdingLib;
using QuackCore.Constants;

namespace QuackCore.Items
{
    public class QuackItemDefinition
    {
        public ItemData BaseData { get; set; }

        public ShopConfig Shop { get; set; }

        public CraftingConfig Crafting { get; set; }

        public DecomposeConfig Decompose { get; set; }

        #region 子配置类定义

        public class ShopConfig
        {
            public string MerchantID { get; set; } = MerchantIDs.Normal;
            public int MaxStock { get; set; } = 5;
            public float PriceFactor { get; set; } = 1.0f;
            public float Probability { get; set; } = 1.0f;
            public bool ForceUnlock { get; set; } = true;
        }

        public class CraftingConfig
        {
            public string FormulaID { get; set; }
            public long MoneyCost { get; set; } = 0L;
            public List<(int itemId, long count)> Materials { get; set; } = new List<(int, long)>();
            public int ResultCount { get; set; } = 1;
            public string[] Workbenches { get; set; } = [ WorkbenchIDs.WorkBenchAdvanced ];
            public string RequirePerk { get; set; } = "";
            public bool UnlockByDefault { get; set; } = true;
            public bool HideInIndex { get; set; } = false;
            public bool LockInDemo { get; set; } = false;
        }

        public class DecomposeConfig
        {
            public long MoneyGain { get; set; } = 0L;
            public List<(int itemId, long count)> Results { get; set; } = new List<(int, long)>();
        }

        #endregion
    }
}