using System;
using System.Linq;
using System.Reflection;
using FastModdingLib;
using Duckov.Economy;

namespace QuackCore.Items
{
    public static class QuackItemRegistry
    {
        /// <summary>
        /// 智能注册入口，负责注册物品本体、商店、合成配方和分解配方
        /// </summary>
        public static void Register(string dllPath, QuackItemDefinition def, string modId)
        {
            if (def.BaseData == null) return;

            // 1. 注册物品本体
            ItemUtils.CreateCustomItem(dllPath, def.BaseData, modId);

            // 2. 注册商店
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

            // 3. 注册合成配方
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

            // 4. 注册分解配方
            if (def.Decompose != null)
            {
                RegisterDecomposeWithFix(def, modId);
            }
        }

        /// <summary>
        /// 卸载指定 ModId 关联的所有物品、配方和分解表
        /// </summary>
        public static void UnregisterAll(string modId)
        {
            try
            {
                // 1. 卸载合成配方
                CraftingUtils.RemoveAllAddedFormulas(modId);

                // 2. 卸载物品
                ItemUtils.UnregisterAllItem(modId);

                // 3. 卸载分解配方
                UnregisterDecomposeWithFix(modId);

                // 4. 商店物品清理
                // 似乎无需手动清理

                ModLogger.Log($"模组 {modId} 的相关物品与配方已成功注销。");
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"注销模组 {modId} 内容时发生异常: {ex.Message}");
            }
        }

        #region 临时修复FML

        private static void RegisterDecomposeWithFix(QuackItemDefinition def, string modId)
        {
            try
            {
                int itemId = def.BaseData.itemId;
                long moneyGain = def.Decompose.MoneyGain;
                var results = def.Decompose.Results.Select(r => (r.itemId, r.count)).ToArray();

                // 调用 FML 记录 ID
                CraftingUtils.AddDecomposeFormula(itemId, moneyGain, results, modId);

                DecomposeDatabase db = DecomposeDatabase.Instance;
                FieldInfo entriesField = typeof(DecomposeDatabase).GetField("entries", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (entriesField == null) return;

                DecomposeFormula[] currentEntries = (DecomposeFormula[])entriesField.GetValue(db);
                if (currentEntries.Any(f => f.item == itemId)) return;

                DecomposeFormula newFormula = new DecomposeFormula
                {
                    item = itemId,
                    valid = true,
                    result = new Cost
                    {
                        money = moneyGain,
                        items = results.Select(r => new Cost.ItemEntry { id = r.itemId, amount = r.count }).ToArray()
                    }
                };

                DecomposeFormula[] newEntries = new DecomposeFormula[currentEntries.Length + 1];
                Array.Copy(currentEntries, newEntries, currentEntries.Length);
                newEntries[currentEntries.Length] = newFormula;

                entriesField.SetValue(db, newEntries);
                RebuildDecomposeDatabase(db);
            }
            catch (Exception ex) { ModLogger.LogError($"分解注册修复失败: {ex.Message}"); }
        }

        private static void UnregisterDecomposeWithFix(string modId)
        {
            try
            {
                DecomposeDatabase db = DecomposeDatabase.Instance;
                FieldInfo entriesField = typeof(DecomposeDatabase).GetField("entries", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (entriesField == null) return;

                DecomposeFormula[] currentEntries = (DecomposeFormula[])entriesField.GetValue(db);
                
                // 找出属于该 Mod 的 ItemId (FML 已经在字典里记住了)
                var toRemoveIds = CraftingUtils.addedDecomposeItemIds
                    .Where(kvp => kvp.Value == modId)
                    .Select(kvp => kvp.Key)
                    .ToList();

                if (toRemoveIds.Count == 0) return;

                // 过滤掉这些 ID 对应的配方
                var newEntriesList = currentEntries.Where(f => !toRemoveIds.Contains(f.item)).ToArray();

                // 写回数据库
                entriesField.SetValue(db, newEntriesList);

                // 调用 FML 原生清理方法清理其内部字典
                CraftingUtils.RemoveAllAddedDecomposeFormulas(modId);

                RebuildDecomposeDatabase(db);
            }
            catch (Exception ex) { ModLogger.LogError($"分解注销修复失败: {ex.Message}"); }
        }

        private static void RebuildDecomposeDatabase(DecomposeDatabase db)
        {
            typeof(DecomposeDatabase).GetMethod("RebuildDictionary", 
                BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(db, null);
        }

        #endregion
    }
}