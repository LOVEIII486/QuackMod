using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FastModdingLib;
using Duckov.Economy; // 必须引用游戏经济系统命名空间
using UnityEngine;

namespace QuackCore.Items
{
    public static class QuackItemRegistry
    {
        /// <summary>
        /// 智能注册入口：整合了对 FML 分解功能的 Bug 修复
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
                    def.Crafting.FormulaID ?? $"formula_{def.BaseData.itemId}", // 若未定义则自动生成
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

            // 4. 注册分解配方 (应用 FML Bug 修复补丁)
            if (def.Decompose != null)
            {
                RegisterDecomposeWithFix(def, modId);
            }
        }

        /// <summary>
        /// 针对 FML 源码中 AddDecomposeFormula 忘记赋值回 entries 数组的补丁修复
        /// </summary>
        private static void RegisterDecomposeWithFix(QuackItemDefinition def, string modId)
        {
            try
            {
                int itemId = def.BaseData.itemId;
                long moneyGain = def.Decompose.MoneyGain;
                var results = def.Decompose.Results.Select(r => (r.itemId, r.count)).ToArray();

                // 1. 先调用 FML 原生方法（为了触发其内部的 addedDecomposeItemIds 记录和日志）
                CraftingUtils.AddDecomposeFormula(itemId, moneyGain, results, modId);

                // 2. 反射获取分解数据库实例
                DecomposeDatabase db = DecomposeDatabase.Instance;
                if (db == null) return;

                // 3. 获取私有的 entries 数组字段
                FieldInfo entriesField = typeof(DecomposeDatabase).GetField("entries", 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                
                if (entriesField == null) return;

                // 4. 获取当前数组并检查是否已包含（防止重复添加）
                DecomposeFormula[] currentEntries = (DecomposeFormula[])entriesField.GetValue(db);
                if (currentEntries.Any(f => f.item == itemId)) return;

                // 5. 手动构建新的 DecomposeFormula 对象
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

                // 6. 【核心修复】创建新数组并重新赋值给数据库（FML 漏掉的步骤）
                DecomposeFormula[] newEntries = new DecomposeFormula[currentEntries.Length + 1];
                Array.Copy(currentEntries, newEntries, currentEntries.Length);
                newEntries[currentEntries.Length] = newFormula;

                entriesField.SetValue(db, newEntries);

                // 7. 调用数据库的索引重建方法
                typeof(DecomposeDatabase).GetMethod("RebuildDictionary", 
                    BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(db, null);

                ModLogger.Log($"[QuackCore] 已通过补丁修复并激活分解配方: {itemId}");
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"分解配方补丁应用失败: {ex.Message}");
            }
        }
    }
}