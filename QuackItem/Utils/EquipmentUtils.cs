using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Duckov.Utilities;
using ItemStatsSystem;

namespace QuackItem.Utils
{
    public static class EquipmentUtils
    {
        public static async UniTask ReplaceAllEquipment(CharacterMainControl target, CharacterMainControl source)
        {
            if (target == null || source == null) return;

            // 1. 修复 AI 自身名称，防止外部逻辑解析 Character(Clone) 时崩溃
            target.name = target.characterPreset != null ? target.characterPreset.name : "MimicNPC";

            // 2. 深度等待：确保 AI 的物品系统和骨架完全就绪
            int waitFrames = 0;
            while (target.CharacterItem == null && waitFrames < 60)
            {
                await UniTask.Yield();
                waitFrames++;
            }
            // 额外延迟，确保 CharacterEquipmentController.Awake 完成 Socket 绑定
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

            ModLogger.Log($"[ReplaceAllEquipment] >>> 开始官方同步: {source.name} -> {target.name}");
            const string BackpackSlotKey = "Backpack";

            // 3. 彻底清理阶段：必须先 Detach 再 Destroy
            if (target.CharacterItem?.Slots != null)
            {
                foreach (var slot in target.CharacterItem.Slots)
                {
                    if (slot == null || slot.Key == BackpackSlotKey) continue;
                    if (slot.Content != null)
                    {
                        var oldItem = slot.Content;
                        if (oldItem.AgentUtilities != null) oldItem.AgentUtilities.ReleaseActiveAgent();
                        
                        // 核心修复：必须调用 Detach 才能清空 Slot 内部的引用
                        oldItem.Detach(); 
                        oldItem.DestroyTree();
                    }
                }
            }

            // 4. 同步循环
            int successCount = 0;
            foreach (var sourceSlot in source.CharacterItem.Slots)
            {
                if (sourceSlot?.Content == null || sourceSlot.Key == BackpackSlotKey) continue;

                string slotKey = sourceSlot.Key;
                Item originalItem = sourceSlot.Content;

                GameObject go = null;
                try
                {
                    // A. 实例化
                    go = UnityEngine.Object.Instantiate(originalItem.gameObject, target.transform.position, Quaternion.identity);
                    
                    // B. 核心修复：名称补丁
                    // 强制使用 ID_Name 格式，这是解决 startIndex 报错的终极手段
                    go.name = $"{originalItem.TypeID}_{originalItem.name.Replace("(Clone)", "")}";

                    Item clonedItem = go.GetComponent<Item>();
                    if (clonedItem == null) 
                    {
                        UnityEngine.Object.Destroy(go);
                        continue;
                    }

                    // C. 状态初始化
                    clonedItem.Detach();
                    if (clonedItem.AgentUtilities != null) clonedItem.AgentUtilities.ReleaseActiveAgent();
                    
                    // 显式初始化，捕获可能存在的 startIndex 报错，防止中断后续槽位
                    try { clonedItem.Initialize(); } 
                    catch (Exception) { /* 忽略初始化过程中的字符串解析错误 */ }

                    // D. 执行拾取：现在 Slot 是干净的，拾取必然成功
                    if (target.PickupItem(clonedItem))
                    {
                        successCount++;
                        
                        // E. 递归同步配件 (解决枪模型显示不全)
                        SyncNestedItems(clonedItem, originalItem, target);

                        // F. 同步手持状态
                        if (IsWeaponSlot(slotKey) && source.CurrentHoldItemAgent != null)
                        {
                            if (source.CurrentHoldItemAgent.transform.IsChildOf(originalItem.transform))
                            {
                                target.ChangeHoldItem(clonedItem);
                            }
                        }
                        ModLogger.Log($"[ReplaceAllEquipment] 槽位 [{slotKey}] 同步成功");
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(go);
                    }
                }
                catch (Exception ex)
                {
                    ModLogger.LogError($"[ReplaceAllEquipment] 同步 [{slotKey}] 失败: {ex.Message}");
                    if (go != null) UnityEngine.Object.Destroy(go);
                }
            }
            ModLogger.Log($"[ReplaceAllEquipment] <<< 同步结束。总计: {successCount} 件。");
        }

        private static void SyncNestedItems(Item targetItem, Item sourceItem, CharacterMainControl targetChar)
        {
            if (targetItem.Slots == null || sourceItem.Slots == null) return;
            foreach (var sSlot in sourceItem.Slots)
            {
                if (sSlot?.Content == null) continue;

                GameObject pGo = UnityEngine.Object.Instantiate(sSlot.Content.gameObject);
                pGo.name = $"{sSlot.Content.TypeID}_{sSlot.Content.name.Replace("(Clone)", "")}";
                Item pItem = pGo.GetComponent<Item>();

                if (pItem != null)
                {
                    pItem.Detach();
                    if (pItem.AgentUtilities != null) pItem.AgentUtilities.ReleaseActiveAgent();
                    pItem.Initialize();

                    if (!targetChar.PickupItem(pItem)) UnityEngine.Object.Destroy(pGo);
                    else SyncNestedItems(pItem, sSlot.Content, targetChar);
                }
            }
        }

        private static bool IsWeaponSlot(string key) => key == "PrimaryWeapon" || key == "SecondaryWeapon" || key == "MeleeWeapon";
    }
}