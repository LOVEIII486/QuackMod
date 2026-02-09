using ItemStatsSystem;
using QuackCore.NPC;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace QuackCore.Items.UsageBehavior
{
    public class QuackSpawnNPCBehavior : ItemStatsSystem.UsageBehavior
    {
        public string basePresetName;
        public string npcConfigId;

        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                var config = QuackNPCRegistry.GetConfig(npcConfigId);
                string displayName = config?.CustomName;

                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = SodaCraft.Localizations.LocalizationManager.GetPlainText(basePresetName);
                }

                return new DisplaySettingsData 
                { 
                    display = true, 
                    description = $"召唤: {displayName}" 
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

            if (registeredConfig != null)
            {
                // 生成自定义 NPC
                if (string.IsNullOrEmpty(registeredConfig.BasePresetName))
                {
                    registeredConfig.BasePresetName = this.basePresetName;
                }
                
                await QuackSpawner.Instance.SpawnNPC(registeredConfig,spawnPos);
                ModLogger.Log($"[QuackCore] 使用自定义配置 ID: {npcConfigId} 生成 NPC。");
            }
            else
            {
                // 生成原版 NPC
                string presetToSpawn = string.IsNullOrEmpty(basePresetName) ? "EnemyPreset_Scav" : basePresetName;
                
                await QuackSpawner.Instance.SpawnVanillaNPC(presetToSpawn, spawnPos);
                ModLogger.Log($"[QuackCore] 未提供有效 ConfigId，生成原版 NPC: {presetToSpawn}");
            }
        }
    }
}