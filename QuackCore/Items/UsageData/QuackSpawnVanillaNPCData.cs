using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageBehavior;

namespace QuackCore.Items.UsageData
{
    public class QuackSpawnVanillaNPCData : UsageBehaviorData
    {
        public string basePresetName = "EnemyPreset_Scav";

        public override ItemStatsSystem.UsageBehavior GetBehavior(Item item)
        {
            var behavior = item.gameObject.AddComponent<QuackSpawnVanillaNPCBehavior>();
            behavior.basePresetName = this.basePresetName;
            return behavior;
        }
    }
}