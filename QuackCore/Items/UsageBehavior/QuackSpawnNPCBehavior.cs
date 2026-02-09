using ItemStatsSystem;
using QuackCore.NPC;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace QuackCore.Items.UsageBehavior
{
    /// <summary>
    /// NPC 生成物品的实际逻辑执行器
    /// </summary>
    public class QuackSpawnNPCBehavior : ItemStatsSystem.UsageBehavior
    {
        public string basePresetName;

        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                return new DisplaySettingsData 
                { 
                    display = true, 
                    description = $"召唤: {basePresetName}" 
                };
            }
        }

        public override bool CanBeUsed(Item item, object user) => true;

        protected override void OnUse(Item item, object user)
        {
            if (user is CharacterMainControl character)
            {
                // 执行异步生成任务
                ExecuteSpawn(character).Forget();
            }
        }

        private async UniTaskVoid ExecuteSpawn(CharacterMainControl user)
        {
            if (QuackSpawner.Instance == null)
            {
                ModLogger.LogError("QuackSpawner 尚未初始化。");
                return;
            }

            Vector3 spawnPos = user.transform.position + user.transform.forward * 2f;

            var testConfig = new QuackNPCConfig 
            { 
                BasePresetName = this.basePresetName,
            };

            await QuackSpawner.Instance.SpawnNPC(testConfig, spawnPos);

            QuackSpawner.Instance.ExportNativePresets();
            QuackSpawner.Instance.SimpleCheck();
            
            ModLogger.Log($"已在位置 {spawnPos} 生成原版 NPC: {basePresetName}");
        }
    }
}