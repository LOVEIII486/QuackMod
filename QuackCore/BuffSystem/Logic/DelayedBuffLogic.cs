using System.Runtime.CompilerServices;
using Duckov.Buffs;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class DelayedBuffLogic : IQuackBuffLogic
    {
        private class LogicState { 
            public float Timer; 
            public bool IsTriggered; 
            public float TickTimer;
        }

        private static readonly ConditionalWeakTable<Buff, LogicState> _instanceStates = new();
        
        private const float TICK_INTERVAL = 0.5f;

        private readonly string _targetBuffName;
        private readonly float _delay;
        private readonly float _targetDuration;

        /// <param name="targetBuffName">要施加的 Buff 注册名</param>
        /// <param name="delay">延迟秒数</param>
        /// <param name="targetDuration">施加的 Buff 持续时间（-1 表示使用 Buff 默认配置）</param>
        public DelayedBuffLogic(string targetBuffName, float delay, float targetDuration = -1f)
        {
            _targetBuffName = targetBuffName;
            _delay = delay;
            _targetDuration = targetDuration;
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            _instanceStates.Add(buff, new LogicState { Timer = _delay, IsTriggered = false, TickTimer = 0f });
        }

        public void OnUpdate(Buff buff, CharacterMainControl target)
        {
            if (!_instanceStates.TryGetValue(buff, out var state) || state.IsTriggered) 
                return;

            state.TickTimer += Time.deltaTime;
            if (state.TickTimer < TICK_INTERVAL) return;

            state.Timer -= state.TickTimer;
            state.TickTimer = 0f;

            if (state.Timer <= 0f)
            {
                state.IsTriggered = true;
                QuackBuffFactory.Apply(target, _targetBuffName, _targetDuration, buff.fromWho);
            }
        }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
            _instanceStates.Remove(buff);
        }
    }
}