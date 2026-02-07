using Duckov.Buffs;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class RegenerationLogic : IQuackBuffLogic
    {
        private readonly float _healPercent;
        private readonly float _interval;
        private readonly bool _showUI;
        private readonly float _uiGroupInterval = 1.0f;

        private float _healTimer;
        private float _uiTimer;
        private float _accumulatedHeal;

        public RegenerationLogic(float healPercent, float interval, bool showUI = true)
        {
            _healPercent = healPercent;
            _interval = interval;
            _showUI = showUI;
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            _healTimer = 0f;
            _uiTimer = 0f;
            _accumulatedHeal = 0f;
        }

        public void OnUpdate(Buff buff, CharacterMainControl target)
        {
            if (target == null || target.Health == null || target.Health.IsDead) return;

            float deltaTime = Time.deltaTime;
            _healTimer += deltaTime;

            if (_healTimer >= _interval)
            {
                _healTimer -= _interval;
                
                float healAmount = target.Health.MaxHealth * _healPercent;
                target.AddHealth(healAmount);
                _accumulatedHeal += healAmount;
            }

            if (_showUI)
            {
                _uiTimer += deltaTime;
                if (_uiTimer >= _uiGroupInterval)
                {
                    _uiTimer -= _uiGroupInterval;
                    if (_accumulatedHeal > 0.1f)
                    {
                        target.PopText($"<color=#00FF00>+{_accumulatedHeal:F0}</color>");
                        _accumulatedHeal = 0f;
                    }
                }
            }
        }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
            _accumulatedHeal = 0f;
        }
    }
}