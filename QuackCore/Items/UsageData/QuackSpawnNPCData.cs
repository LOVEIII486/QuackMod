using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageBehavior;

namespace QuackCore.Items.UsageData
{
    public class QuackSpawnNPCData : UsageBehaviorData
    {
        /// <summary>
        /// 原版 NPC 预设名称
        /// </summary>
        public string basePresetName = "EnemyPreset_Scav";

        /// <summary>
        /// 自定义 NPC 配置 ID
        /// </summary>
        public string npcConfigId;

        public override ItemStatsSystem.UsageBehavior GetBehavior(Item item)
        {
            var behavior = item.gameObject.AddComponent<QuackSpawnNPCBehavior>();
            
            behavior.basePresetName = this.basePresetName;
            behavior.npcConfigId = this.npcConfigId;
            
            return behavior;
        }
    }
}