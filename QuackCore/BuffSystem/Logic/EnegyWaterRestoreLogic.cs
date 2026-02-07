using Duckov.Buffs;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class EnegyWaterRestoreLogic : IQuackBuffLogic
    {
        private readonly float _energyPerTick; // 每秒能量增量
        private readonly float _waterPerTick;  // 每秒水分增量
        private readonly float _interval;      // 触发间隔（秒）
        
        private float _timer;
        
        public EnegyWaterRestoreLogic(float energyPerTick, float waterPerTick, float interval = 1.0f)
        {
            _energyPerTick = energyPerTick;
            _waterPerTick = waterPerTick;
            _interval = interval;
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            _timer = 0f;
        }

        public void OnUpdate(Buff buff, CharacterMainControl target)
        {
            if (target == null || target.Health == null || target.Health.IsDead) return;

            _timer += Time.deltaTime;

            if (_timer >= _interval)
            {
                _timer -= _interval;

                if (_energyPerTick != 0)
                {
                    target.AddEnergy(_energyPerTick * _interval);
                }

                if (_waterPerTick != 0)
                {
                    target.AddWater(_waterPerTick * _interval);
                }
            }
        }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
            
        }
    }
}