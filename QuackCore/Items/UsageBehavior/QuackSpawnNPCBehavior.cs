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
                var preset = QuackSpawner.GetNativePresetByName(basePresetName);
                var displayName = preset.DisplayName;
                
                return new DisplaySettingsData 
                { 
                    display = true, 
                    description = $"召唤: {(string.IsNullOrEmpty(displayName) ? "未知角色" : displayName)}" 
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
            Vector3 spawnPos = user.transform.position + user.transform.forward * 2f;
            
            var registeredConfig = QuackNPCRegistry.GetConfig(npcConfigId);

            if (registeredConfig != null)
            {
                await QuackSpawner.SpawnNPC(npcConfigId, spawnPos);
                ModLogger.LogDebug($"[Behavior] 使用自定义 ID: {npcConfigId} 生成 NPC。");
            }
            else
            {
                string presetToSpawn = string.IsNullOrEmpty(basePresetName) ? "EnemyPreset_Scav" : basePresetName;
                await QuackSpawner.SpawnVanillaNPC(presetToSpawn, spawnPos);
                ModLogger.LogDebug($"[Behavior] 未找到自定义 Config，生成原版 NPC: {presetToSpawn}");
            }
        }
    }
}