using Duckov.Buffs;
using QuackCore.AttributeModifier;
using QuackCore.BuffSystem;

namespace TarkovStimulants.Buffs.Effects
{
    public class MaxHpLogic : IQuackBuffLogic
    {
        private readonly float _bonusAmount;

        public MaxHpLogic(float amount)
        {
            _bonusAmount = amount;
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            if (target == null) return;

            CharacterModifier.Modify(
                target, 
                StatModifier.Attributes.MaxHealth, 
                _bonusAmount, 
                false, 
                buff
            );
            
            if (target.Health != null)
            {
                target.Health.SetHealth(target.Health.MaxHealth);
            }
        }

        public void OnUpdate(Buff buff, CharacterMainControl target) { }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
            if (target == null) return;

            CharacterModifier.ClearAll(target, buff);
        }
    }
}