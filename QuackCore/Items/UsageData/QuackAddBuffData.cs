using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageBehavior;

namespace QuackCore.Items.UsageData
{
    public class QuackAddBuffData : UsageBehaviorData
    {
        public int buffId;
        public float chance = 1f;

        public override ItemStatsSystem.UsageBehavior GetBehavior(Item item)
        {
            var behavior = item.gameObject.AddComponent<QuackAddBuffBehavior>();
            
            behavior.buffId = this.buffId;
            behavior.chance = this.chance;
            
            return behavior;
        }
    }
}