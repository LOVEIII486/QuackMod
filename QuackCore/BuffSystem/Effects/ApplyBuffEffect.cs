using Duckov.Buffs;

namespace QuackCore.BuffSystem.Effects
{
    public class ApplyBuffEffect : IQuackBuffEffect
    {
        private string _targetBuff;
        private float _duration;

        public ApplyBuffEffect(string targetBuff, float duration) {
            _targetBuff = targetBuff;
            _duration = duration;
        }

        public void OnApplied(Buff buff, CharacterMainControl target) {
            QuackBuffFactory.Apply(target, _targetBuff, _duration, buff.fromWho);
        }

        public void OnRemoved(Buff buff, CharacterMainControl target) { }
    }
}