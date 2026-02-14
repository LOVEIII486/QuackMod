using System;
using System.Linq;
using FastModdingLib;
using ItemStatsSystem;

namespace QuackCore.Items
{
    public static class QuackItemRegistry
    {
        /// <summary>
        /// 智能注册入口，负责注册物品本体、商店、合成配方和分解配方。
        /// 自动检测 Slots 配置，若存在则使用复杂构建模式。
        /// </summary>
        public static void Register(string dllPath, QuackItemDefinition def, string modId)
        {
            if (def.BaseData == null) return;

            if (def.Slots != null && def.Slots.Count > 0)
            {
                Item complexItem = QuackItemFactory.CreateComplexItem(dllPath, def);
                ItemUtils.RegisterItem(complexItem, modId);
            }
            else
            {
                ItemUtils.CreateCustomItem(dllPath, def.BaseData, modId);
            }

            RegisterExtraData(def, modId);
        }
        
        /// <summary>
        /// 独立的复杂物品注册函数。
        /// 绕过 FML 的简易构建流程，支持自定义槽位。
        /// </summary>
        public static void RegisterComplex(string dllPath, QuackItemDefinition def, string modId)
        {
            if (def.BaseData == null) return;

            Item complexItem = QuackItemFactory.CreateComplexItem(dllPath, def);
            // 仅使用 FML 进行最终的系统挂载注册
            ItemUtils.RegisterItem(complexItem, modId);
            RegisterExtraData(def, modId);
        }
        
        /// <summary>
        /// 卸载指定 ModId 关联的所有物品、配方和分解表
        /// </summary>
        public static void UnregisterAll(string modId)
        {
            try
            {
                CraftingUtils.RemoveAllAddedFormulas(modId);
                CraftingUtils.RemoveAllAddedDecomposeFormulas(modId);
                ItemUtils.UnregisterAllItem(modId);

                ModLogger.Log($"模组 {modId} 的相关物品与配方已成功注销。");
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"注销模组 {modId} 内容时发生异常: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 内部工具函数：注册物品关联的商店、合成及分解数据。
        /// </summary>
        private static void RegisterExtraData(QuackItemDefinition def, string modId)
        {
            // 1. 注册商店
            if (def.Shop != null)
            {
                ShopUtils.AddGoods(new ShopGoodsData
                {
                    merchantProfileID = def.Shop.MerchantID,
                    typeID = def.BaseData.itemId,
                    maxStock = def.Shop.MaxStock,
                    priceFactor = def.Shop.PriceFactor,
                    possibility = def.Shop.Probability,
                    forceUnlock = def.Shop.ForceUnlock
                });
            }

            // 2. 注册合成配方
            if (def.Crafting != null)
            {
                var costItems = def.Crafting.Materials
                    .Select(m => new ValueTuple<int, long>(m.itemId, m.count))
                    .ToArray();

                CraftingUtils.AddCraftingFormula(
                    def.Crafting.FormulaID ?? $"formula_{def.BaseData.itemId}_craft",
                    def.Crafting.MoneyCost,
                    costItems,
                    def.BaseData.itemId,
                    def.Crafting.ResultCount,
                    def.Crafting.Workbenches,
                    def.Crafting.RequirePerk,
                    def.Crafting.UnlockByDefault,
                    def.Crafting.HideInIndex,
                    def.Crafting.LockInDemo,
                    modId
                );
            }

            // 3. 注册分解配方
            if (def.Decompose != null)
            {
                var results = def.Decompose.Results
                    .Select(r => new ValueTuple<int, long>(r.itemId, r.count))
                    .ToArray();

                try
                {
                    CraftingUtils.AddDecomposeFormula(
                        def.BaseData.itemId,
                        def.Decompose.MoneyGain,
                        results,
                        modId
                    );
                }
                catch (Exception e)
                {
                    ModLogger.LogError($"注册分解失败: {def.BaseData.itemId}, {e.Message}");
                }
            }
        }
    }
}