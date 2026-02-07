using ItemStatsSystem;
using QuackCore.BuffSystem;

namespace TarkovStimulants.Behaviors
{
    public class TarkovStimulantsCookieBehavior : UsageBehavior
    {
        public int BuffID;
        public string Message;

        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                var settings = new DisplaySettingsData();
                settings.display = true;
                settings.description = $"食用后获得特殊增益";
                return settings;
            }
        }

        public override bool CanBeUsed(Item item, object user)
        {
            return user is CharacterMainControl character && !character.Health.IsDead;
        }

        protected override void OnUse(Item item, object user)
        {
            if (user is CharacterMainControl character)
            {
                QuackBuffFactory.Apply(character, $"QuackItem_QuackMaxHpBuff");

                character.PopText($"<color=yellow>{Message}</color>");

                ModLogger.Log($"[QuackItem] 玩家吃掉了曲奇，触发 Buff ID: {BuffID}");
            }
        }
    }
}