using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageBehavior;

namespace QuackCore.Items.UsageData
{
    public class QuackSpawnNPCData : UsageBehaviorData
    {
        public string basePresetName = "EnemyPreset_Scav";

        public override ItemStatsSystem.UsageBehavior GetBehavior(Item item)
        {
            var behavior = item.gameObject.AddComponent<QuackSpawnNPCBehavior>();
            
            behavior.basePresetName = this.basePresetName;
            
            return behavior;
        }
    }
}