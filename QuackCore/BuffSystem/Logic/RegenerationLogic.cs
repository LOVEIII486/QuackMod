using Duckov.Buffs;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class RegenerationLogic : IQuackBuffLogic
    {
        private readonly float _healPercent; // 每次恢復的最大生命值比例 (0.01 = 1%)
        private readonly float _interval;    // 恢復觸發間隔 (秒)
        private readonly bool _showUI;      // 是否顯示彈窗文字
        private readonly float _uiGroupInterval = 1.0f; // UI 匯總顯示間隔，防止彈窗過多

        private float _healTimer;
        private float _uiTimer;
        private float _accumulatedHeal;

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="healPercent">治療比例 (如 0.05 代表 5%)</param>
        /// <param name="interval">觸發間隔 (如 0.2 秒)</param>
        /// <param name="showUI">是否顯示綠色回血數字</param>
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

            // 1. 執行治療 Tick
            if (_healTimer >= _interval)
            {
                _healTimer -= _interval;
                
                float healAmount = target.Health.MaxHealth * _healPercent;
                target.Health.AddHealth(healAmount);
                _accumulatedHeal += healAmount;
            }

            // 2. 執行 UI 顯示 (如果開啟)
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
            // 清理邏輯
            _accumulatedHeal = 0f;
        }
    }
}