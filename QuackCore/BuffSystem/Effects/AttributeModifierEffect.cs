using Duckov.Buffs;
using QuackCore.AttributeModifier; 

namespace QuackCore.BuffSystem.Effects
{
    public class AttributeModifierEffect : IQuackBuffEffect
    {
        private readonly string _attributeName;
        private readonly float _value;
        private readonly bool _isMultiplier;

        public AttributeModifierEffect(string attributeName, float value, bool isMultiplier = false)
        {
            _attributeName = attributeName;
            _value = value;
            _isMultiplier = isMultiplier;
        }

        public void OnApplied(Buff buff, CharacterMainControl target)
        {
            CharacterModifier.Modify(target, _attributeName, _value, _isMultiplier, buff);
        }

        public void OnRemoved(Buff buff, CharacterMainControl target)
        {
            // 在 Patches 中实现了基于 Buff 实例的统一清理
        }
    }
}