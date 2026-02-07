using System.Collections.Generic;
using Duckov.Buffs;

namespace QuackCore.BuffSystem
{
    public class QuackBuffDefinition
    {
        public QuackBuffFactory.BuffConfig Config { get; }
        public List<IQuackBuffEffect> Effects { get; } = new List<IQuackBuffEffect>();
        public List<IQuackBuffLogic> CustomLogics { get; } = new List<IQuackBuffLogic>();

        public QuackBuffDefinition(QuackBuffFactory.BuffConfig config)
        {
            this.Config = config;
        }

        public QuackBuffDefinition AddEffect(IQuackBuffEffect action)
        {
            Effects.Add(action);
            return this;
        }
        
        public QuackBuffDefinition AddCustomLogic(IQuackBuffLogic logic)
        {
            if (logic != null) CustomLogics.Add(logic);
            return this;
        }

        internal void ExecuteSetup(Buff buff, CharacterMainControl target)
        {
            foreach (var effect in Effects) effect.OnApplied(buff, target);
            foreach (var logic in CustomLogics) logic.OnSetup(buff, target);
        }
        
        internal void ExecuteUpdate(Buff buff, CharacterMainControl target)
        {
            foreach (var logic in CustomLogics) logic.OnUpdate(buff, target);
        }

        internal void ExecuteDestroy(Buff buff, CharacterMainControl target)
        {
            foreach (var effect in Effects) effect.OnRemoved(buff, target);
            foreach (var logic in CustomLogics) logic.OnDestroy(buff, target);
        }
    }
}