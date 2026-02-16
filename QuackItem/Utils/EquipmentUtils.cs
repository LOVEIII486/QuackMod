using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;

namespace QuackItem.Utils
{
    public static class EquipmentUtils
    {
        public static async UniTask ReplaceAllEquipment(CharacterMainControl target, CharacterMainControl source)
        {
            if (target == null || source == null) return;

            int waitFrames = 0;
            while (target.CharacterItem == null && waitFrames < 60)
            {
                await UniTask.Yield();
                waitFrames++;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            
            const string BackpackSlotKey = "Backpack";

            if (target.CharacterItem?.Slots != null)
            {
                foreach (var slot in target.CharacterItem.Slots)
                {
                    if (slot == null || slot.Key == BackpackSlotKey) continue;
                    if (slot.Content != null)
                    {
                        var oldItem = slot.Content;
                        if (oldItem.AgentUtilities != null) oldItem.AgentUtilities.ReleaseActiveAgent();
                        oldItem.Detach(); 
                        oldItem.DestroyTree();
                    }
                }
            }

            int successCount = 0;
            foreach (var sourceSlot in source.CharacterItem.Slots)
            {
                if (sourceSlot?.Content == null || sourceSlot.Key == BackpackSlotKey) continue;

                string slotKey = sourceSlot.Key;
                Item originalItem = sourceSlot.Content;

                GameObject go = null;
                try
                {
                    go = UnityEngine.Object.Instantiate(originalItem.gameObject, target.transform.position, Quaternion.identity);
         
                    Item clonedItem = go.GetComponent<Item>();
                    if (clonedItem == null) 
                    {
                        UnityEngine.Object.Destroy(go);
                        continue;
                    }

                    clonedItem.Detach();
                    if (clonedItem.AgentUtilities != null) clonedItem.AgentUtilities.ReleaseActiveAgent();
                    clonedItem.Initialize(); 

                    if (target.PickupItem(clonedItem))
                    {
                        successCount++;
                        SyncNestedItems(clonedItem, originalItem, target);
                        if (IsWeaponSlot(slotKey) && source.CurrentHoldItemAgent != null)
                        {
                            if (source.CurrentHoldItemAgent.transform.IsChildOf(originalItem.transform))
                            {
                                target.ChangeHoldItem(clonedItem);
                            }
                        }
                        ModLogger.LogDebug($"[ReplaceAllEquipment] 槽位 [{slotKey}] 同步成功");
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
            ModLogger.LogDebug($"[ReplaceAllEquipment] <<< 同步结束。总计: {successCount} 件。");
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