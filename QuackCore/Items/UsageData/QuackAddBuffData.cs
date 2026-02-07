using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageBehavior;

namespace QuackCore.Items.UsageData
{
    public class QuackAddBuffData : UsageBehaviorData
    {
        public string buffName;
        public float chance = 1f;

        public override ItemStatsSystem.UsageBehavior GetBehavior(Item item)
        {
            var behavior = item.gameObject.AddComponent<QuackAddBuffBehavior>();
            
            behavior.buffName = this.buffName;
            behavior.chance = this.chance;
            
            return behavior;
        }
    }
}