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
        }

        private static readonly ConditionalWeakTable<Buff, LogicState> _instanceStates = new();

        private readonly string _targetBuffName;
        private readonly float _delay;

        public DelayedBuffLogic(string targetBuffName, float delay)
        {
            _targetBuffName = targetBuffName;
            _delay = delay;
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            _instanceStates.Add(buff, new LogicState { Timer = _delay, IsTriggered = false });
        }

        public void OnUpdate(Buff buff, CharacterMainControl target)
        {
            if (!_instanceStates.TryGetValue(buff, out var state) || state.IsTriggered) 
                return;

            state.Timer -= Time.deltaTime;
            if (state.Timer <= 0f)
            {
                state.IsTriggered = true;
                QuackBuffFactory.Apply(target, _targetBuffName, buff.fromWho);
            }
        }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
            _instanceStates.Remove(buff);
        }
    }
}