using Duckov.Buffs;

namespace QuackCore.BuffSystem.Effects
{
    public class ApplyBuffEffect : IQuackBuffEffect
    {
        private int _targetBuffId;
        private float _duration;

        /// <summary>
        /// 施加buff效果
        /// </summary>
        /// <param name="targetBuffId">目标 Buff 的数字 ID</param>
        /// <param name="duration">覆盖时长。默认 float.NaN 表示不覆盖</param>
        public ApplyBuffEffect(int targetBuffId, float duration = float.NaN) 
        {
            _targetBuffId = targetBuffId;
            _duration = duration;
        }

        public void OnApplied(Buff buff, CharacterMainControl target) 
        {
            QuackBuffFactory.Apply(target, _targetBuffId, _duration, buff.fromWho);
        }

        public void OnRemoved(Buff buff, CharacterMainControl target) { }
    }
}