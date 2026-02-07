using FastModdingLib;
using ItemStatsSystem;
using QuackCore.Items.UsageBehavior;

namespace QuackCore.Items.UsageData
{
    public class QuackAddBuffData : UsageBehaviorData
    {
        public string BuffName;
        public float Chance = 1f;

        public override ItemStatsSystem.UsageBehavior GetBehavior(Item item)
        {
            var behavior = item.gameObject.AddComponent<QuackAddBuffBehavior>();
            
            behavior.buffName = this.BuffName;
            behavior.chance = this.Chance;
            
            return behavior;
        }
    }
}