using FastModdingLib;
using ItemStatsSystem;
using QuackItem.Items.Behavior;

namespace QuackItem.Items.Data
{
    public class MimicTearAshesData : UsageBehaviorData
    {
        public string basePresetName = "EnemyPreset_Scav";
        public string npcConfigId;

        public override ItemStatsSystem.UsageBehavior GetBehavior(Item item)
        {
            var behavior = item.gameObject.AddComponent<MimicTearAshesBehavior>();

            behavior.basePresetName = this.basePresetName;
            behavior.npcConfigId = this.npcConfigId;

            return behavior;
        }
    }
}