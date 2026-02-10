using FastModdingLib;
using ItemStatsSystem;
using QuackItem.Items.Behavior;

namespace QuackItem.Items.Data
{
    public class ReturnOrbData : UsageBehaviorData
    {
        public override ItemStatsSystem.UsageBehavior GetBehavior(Item item)
        {
            var behavior = item.gameObject.AddComponent<ReturnOrbBehavior>();
            return behavior;
        }
    }
}

