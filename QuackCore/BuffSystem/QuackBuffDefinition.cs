using System.Collections.Generic;
using Duckov.Buffs;

namespace QuackCore.BuffSystem
{
    public class QuackBuffDefinition
    {
        public QuackBuffFactory.BuffConfig Config { get; }
        public List<IQuackBuffEffect> Effects { get; } = new List<IQuackBuffEffect>();
        public IQuackBuffLogic CustomLogic { get; private set; }

        public QuackBuffDefinition(QuackBuffFactory.BuffConfig config)
        {
            this.Config = config;
        }

        // 快捷效果
        public QuackBuffDefinition AddEffect(IQuackBuffEffect action)
        {
            Effects.Add(action);
            return this;
        }
        
        // 自定义逻辑
        public QuackBuffDefinition SetCustomLogic(IQuackBuffLogic logic)
        {
            CustomLogic = logic;
            return this;
        }

        internal void ExecuteSetup(Buff buff, CharacterMainControl target)
        {
            foreach (var effect in Effects) effect.OnApplied(buff, target);
            CustomLogic?.OnSetup(buff, target);
        }

        internal void ExecuteDestroy(Buff buff, CharacterMainControl target)
        {
            foreach (var effect in Effects) effect.OnRemoved(buff, target);
            CustomLogic?.OnDestroy(buff, target);
        }
    }
}