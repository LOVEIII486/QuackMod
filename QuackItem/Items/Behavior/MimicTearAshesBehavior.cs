using ItemStatsSystem;
using QuackCore.NPC;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace QuackItem.Items.Behavior
{
    public class MimicTearAshesBehavior : ItemStatsSystem.UsageBehavior
    {
        public string basePresetName;
        public string npcConfigId;
        private readonly ModelReplacer _modelReplacer = new ModelReplacer();

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
                ExecuteSpawn(character, item).Forget();
            }
        }

        private async UniTaskVoid ExecuteSpawn(CharacterMainControl user, Item usedItem)
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

            // spawned 可能为 null（生成失败），也可能未完成初始化某些字段，但 SpawnNPC 返回时通常已可操作
            if (spawned != null)
            {
                // 等待 spawned 的 characterModel 或相关组件初始化（最多等待 1 秒）
                int waitFrames = 0;
                while (spawned.characterModel == null && waitFrames < 60)
                {
                    await UniTask.Yield();
                    waitFrames++;
                }

                // Apply model replacement using the player as source
                var player = CharacterMainControl.Main;
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
                }
            }
        }
    }
}
