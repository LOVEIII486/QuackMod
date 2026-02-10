using System;
using System.Collections.Generic;
using System.Linq;
using ItemStatsSystem;
using QuackCore.NPC;
using UnityEngine;
using Cysharp.Threading.Tasks;
using QuackItem.Utils;

namespace QuackItem.Items.Behavior
{
    public class MimicTearAshesBehavior : ItemStatsSystem.UsageBehavior
    {
        public string basePresetName;
        public string npcConfigId;
        private readonly ModelReplacer _modelReplacer = new ModelReplacer();

        // 口径到弹药 ID 的保底映射
        private static readonly Dictionary<string, int> CaliberToBulletId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "SMG", 598 },
            { "AR", 607 },
            { "PWS", 1162 },
            { "PWL", 918 },
            { "MAG", 709 },
            { "Candy", 1262 },
            { "Pop", 944 },
            { "SHT", 634 },
            { "BR", 616 },
            { "SNP", 701 },
            { "Rocket", 326 },
            { "GL", 95815 }
        };

        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                string desc = "召唤仿身泪滴的骨灰";
                return new DisplaySettingsData
                {
                    display = true,
                    description = $"{desc}"
                };
            }
        }

        public override bool CanBeUsed(Item item, object user) => true;

        protected override void OnUse(Item item, object user)
        {
            if (user is CharacterMainControl character)
            {
                ExecuteSpawn(character).Forget();
            }
        }

        private async UniTaskVoid ExecuteSpawn(CharacterMainControl user)
        {
            if (QuackSpawner.Instance == null) return;

            Vector3 spawnPos = user.transform.position + user.transform.forward * 2f;

            var registeredConfig = QuackNPCRegistry.GetConfig(npcConfigId);
            CharacterMainControl spawned = null;

            if (registeredConfig != null)
            {
                if (string.IsNullOrEmpty(registeredConfig.BasePresetName))
                {
                    registeredConfig.BasePresetName = this.basePresetName;
                }

                spawned = await QuackSpawner.Instance.SpawnNPC(registeredConfig, spawnPos);
                ModLogger.LogDebug($"使用自定义配置 ID: {npcConfigId} 生成 NPC（MimicTearAshes）。");
            }
            else
            {
                string presetToSpawn = string.IsNullOrEmpty(basePresetName) ? "EnemyPreset_Scav" : basePresetName;

                spawned = await QuackSpawner.Instance.SpawnVanillaNPC(presetToSpawn, spawnPos);
                ModLogger.LogDebug($"未提供有效 ConfigId（MimicTearAshes），生成原版 NPC: {presetToSpawn}");
            }

            if (spawned != null)
            {
                int waitFrames = 0;
                while (spawned.characterModel == null && waitFrames < 60)
                {
                    await UniTask.Yield();
                    waitFrames++;
                }

                var player = user;
                if (player != null)
                {
                    try
                    {
                        _modelReplacer.ApplyModel(spawned, player);
                    }
                    catch (System.Exception ex)
                    {
                        ModLogger.LogWarning($"MimicTearAshes 模型替换失败: {ex.Message}");
                    }

                    try
                    {
                        ClearWeaponSlots(spawned);

                        Item srcPrimary = GetSlotItem(player, "PrimaryWeapon");
                        Item srcHelmet = GetSlotItem(player, "Helmat");
                        Item srcArmor = GetSlotItem(player, "Armor");

                        if (srcPrimary != null) CloneAndSetupWeapon(srcPrimary, spawned);
                        if (srcHelmet != null) CloneToSlot(srcHelmet, spawned);
                        if (srcArmor != null) CloneToSlot(srcArmor, spawned);
                    }
                    catch (System.Exception ex)
                    {
                        ModLogger.LogWarning($"MimicTearAshes 复制装备失败: {ex.Message}");
                    }
                }
            }
        }

        private static Item GetSlotItem(CharacterMainControl c, string slotName)
        {
            if (c?.CharacterItem?.Slots == null) return null;
            var slot = c.CharacterItem.Slots.FirstOrDefault(s => s != null && s.Key == slotName);
            return slot?.Content;
        }

        private static void ClearWeaponSlots(CharacterMainControl c)
        {
            if (c?.CharacterItem == null) return;
            string[] slots = { "PrimaryWeapon", "SecondaryWeapon", "MeleeWeapon" };
            foreach (string s in slots)
            {
                var it = GetSlotItem(c, s);
                it?.DestroyTree();
            }
        }

        private static void CloneToSlot(Item src, CharacterMainControl enemy)
        {
            if (src == null || enemy == null) return;
            try
            {
                GameObject go = UnityEngine.Object.Instantiate(src.gameObject, enemy.transform.position, Quaternion.identity);
                Item clone = go.GetComponent<Item>();
                if (clone != null)
                {
                    clone.Detach();
                    clone.AgentUtilities.ReleaseActiveAgent();
                    enemy.PickupItem(clone);
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogWarning($"CloneToSlot 失败: {ex.Message}");
            }
        }

        private static void CloneAndSetupWeapon(Item srcWeapon, CharacterMainControl enemy)
        {
            if (srcWeapon == null || enemy == null) return;
            try
            {
                GameObject go = UnityEngine.Object.Instantiate(srcWeapon.gameObject, enemy.transform.position, Quaternion.identity);
                Item clone = go.GetComponent<Item>();
                if (clone == null) return;

                clone.Detach();
                clone.AgentUtilities.ReleaseActiveAgent();

                enemy.PickupItem(clone);
                enemy.ChangeHoldItem(clone);

                var gun = clone.GetComponent<ItemSetting_Gun>();
                if (gun != null)
                {
                    string caliber = srcWeapon.Constants.GetString("Caliber");

                    int bulletId = -1;

                    var srcGun = srcWeapon.GetComponent<ItemSetting_Gun>();
                    if (srcGun != null && srcGun.TargetBulletID > 0)
                    {
                        bulletId = srcGun.TargetBulletID;
                    }

                    if (bulletId <= 0 && !string.IsNullOrEmpty(caliber))
                    {
                        CaliberToBulletId.TryGetValue(caliber, out bulletId);
                    }

                    if (bulletId > 0)
                    {
                        var bulletSeed = ItemAssetsCollection.InstantiateSync(bulletId);
                        if (bulletSeed != null)
                        {
                            bulletSeed.Initialize();

                            if (bulletSeed.Stackable)
                            {
                                bulletSeed.StackCount = Mathf.Min(100, bulletSeed.MaxStackCount);
                            }
                            else
                            {
                                Debug.LogWarning($"[MimicTearAshes] 选取的物品 '{bulletSeed.DisplayName}' (ID:{bulletId}) 不可堆叠");
                                bulletSeed.StackCount = 1;
                            }

                            enemy.CharacterItem.Inventory.AddAndMerge(bulletSeed);

                            gun.SetTargetBulletType(bulletId);
                            clone.Variables.SetInt("BulletCount".GetHashCode(), gun.Capacity);
                        }
                        else
                        {
                            Debug.LogError($"[MimicTearAshes] 无法实例化物品ID: {bulletId}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[MimicTearAshes] 未能为武器: {srcWeapon.DisplayName} 找到口径 '{caliber}' 合法的弹药ID");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogWarning($"CloneAndSetupWeapon 失败: {ex.Message}");
            }
        }
    }
}
