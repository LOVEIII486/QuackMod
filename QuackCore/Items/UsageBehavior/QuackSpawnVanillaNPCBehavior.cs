using ItemStatsSystem;
using QuackCore.NPC;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace QuackCore.Items.UsageBehavior
{
    /// <summary>
    /// 纯原版 NPC 生成
    /// </summary>
    public class QuackSpawnVanillaNPCBehavior : ItemStatsSystem.UsageBehavior
    {
        public string basePresetName;

        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                return new DisplaySettingsData 
                { 
                    display = true, 
                    description = $"召唤(原版): {basePresetName}" 
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
            if (QuackSpawner.Instance == null)
            {
                ModLogger.LogError("QuackSpawner 尚未初始化。");
                return;
            }

            Vector3 spawnPos = user.transform.position + user.transform.forward * 2f;
            
            await QuackSpawner.Instance.SpawnVanillaNPC(this.basePresetName, spawnPos, Teams.player);
        }
    }
}