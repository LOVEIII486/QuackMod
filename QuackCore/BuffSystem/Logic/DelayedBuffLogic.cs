using Duckov.Buffs;
using System.Collections.Generic;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class DelayedBuffLogic : IQuackBuffLogic
    {
        private readonly List<(int id, float delay, float duration, float chance)> _tasks = new();

        /// <summary>
        /// 延迟施加buff
        /// </summary>
        /// <param name="targetBuffId">目标 Buff 的数字 ID</param>
        /// <param name="delay">延迟时间</param>
        /// <param name="targetDuration">目标时长</param>
        /// <param name="chance">触发几率 (0-1.0)</param>
        public DelayedBuffLogic(int targetBuffId, float delay, float targetDuration = float.NaN, float chance = 1.0f)
        {
            _tasks.Add((targetBuffId, delay, targetDuration, chance));
        }

        /// <summary>
        /// 延迟施加多个buff
        /// new DelayedBuffLogic((999101, 10s, 60s, 1.0f), (999102, 20s, 30s, 0.5f))
        /// </summary>
        public DelayedBuffLogic(params (int id, float delay, float duration, float chance)[] tasks)
        {
            foreach (var t in tasks) _tasks.Add(t);
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            if (DelayedBuffManager.Instance == null)
            {
                ModLogger.LogError("[DelayedBuffLogic] DelayedBuffManager 实例不存在，无法注册延迟效果！");
                return;
            }

            foreach (var task in _tasks)
            {
                if (Random.value <= task.chance)
                {
                    DelayedBuffManager.Instance.AddTask(target, task.id, task.delay, task.duration, buff.fromWho);
                    if (task.chance < 1.0f)
                    {
                        ModLogger.LogDebug($"[DelayedBuffLogic] 概率判定通过: ID {task.id} (几率: {task.chance * 100}%)");
                    }
                }
            }
        }

        public void OnUpdate(Buff buff, CharacterMainControl target) { }

        public void OnDestroy(Buff buff, CharacterMainControl target) { }
    }
}