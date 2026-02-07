using Duckov.Buffs;

namespace QuackCore.BuffSystem
{
    public interface IQuackBuffEffect
    {
        void OnApplied(Buff buff, CharacterMainControl target);
        void OnRemoved(Buff buff, CharacterMainControl target);
    }

    public interface IQuackBuffLogic
    {
        void OnSetup(Buff buff, CharacterMainControl target);
        void OnUpdate(Buff buff, CharacterMainControl target);
        void OnDestroy(Buff buff, CharacterMainControl target);
    }
}