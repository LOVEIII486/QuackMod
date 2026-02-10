using ItemStatsSystem;

namespace QuackItem.Items.UsageBehavior
{
    public class ReturnOrbBehavior : ItemStatsSystem.UsageBehavior
    {
        
        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                var settings = new DisplaySettingsData { display = true, description = "" };
                

                return settings;
            }
        }

        public override bool CanBeUsed(Item item, object user) => true;

        protected override void OnUse(Item item, object user)
        {
            
        }
    }
}