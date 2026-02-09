using Duckov.Buffs;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class RegenerationLogic : IQuackBuffLogic
    {
        private readonly float _healPercent;
        private readonly float _interval;
        private readonly float _healDuration;
        private readonly bool _showUI;
        private readonly float _uiGroupInterval = 1.0f;

        private float _healTimer;
        private float _uiTimer;
        private float _accumulatedHeal;
        private float _durationTimer;

        /// <summary>
        /// 再生
        /// </summary>
        /// <param name="healPercent">每次恢复的最大生命百分比</param>
        /// <param name="interval">恢复间隔</param>
        /// <param name="healDuration">总恢复时长（设为 -1 或 0 则随 Buff 全程持续）</param>
        /// <param name="showUI">是否显示弹出文本</param>
        public RegenerationLogic(float healPercent, float interval, float healDuration = -1f, bool showUI = true)
        {
            _healPercent = healPercent;
            _interval = interval;
            _healDuration = healDuration;
            _showUI = showUI;
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            _healTimer = 0f;
            _uiTimer = 0f;
            _accumulatedHeal = 0f;
            _durationTimer = 0f;
        }

        public void OnUpdate(Buff buff, CharacterMainControl target)
        {
            if (target == null || target.Health == null || target.Health.IsDead) return;

            float deltaTime = Time.deltaTime;
            _durationTimer += deltaTime;

            bool isWithinDuration = _healDuration <= 0 || _durationTimer <= _healDuration;

            if (isWithinDuration)
            {
                _healTimer += deltaTime;
                if (_healTimer >= _interval)
                {
                    _healTimer -= _interval;
                    float healAmount = target.Health.MaxHealth * _healPercent;
                    target.AddHealth(healAmount);
                    _accumulatedHeal += healAmount;
                }
            }

            if (_showUI)
            {
                _uiTimer += deltaTime;
                if (_uiTimer >= _uiGroupInterval)
                {
                    _uiTimer -= _uiGroupInterval;
            
                    if (Mathf.Abs(_accumulatedHeal) > 0.1f)
                    {
                        string color = _accumulatedHeal > 0 ? "#00FF00" : "#FF0000";
                        string sign = _accumulatedHeal > 0 ? "+" : "";
                
                        target.PopText($"<color={color}>{sign}{_accumulatedHeal:F0}</color>");
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