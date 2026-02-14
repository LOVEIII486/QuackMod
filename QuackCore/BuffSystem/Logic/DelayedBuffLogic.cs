using Duckov.Buffs;
using System.Collections.Generic;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class DelayedBuffLogic : IQuackBuffLogic
    {
        private readonly List<(string name, float delay, float duration, float chance)> _tasks = new();

        /// <summary>
        /// 单任务构造函数（兼容旧代码，概率默认为 1）
        /// </summary>
        public DelayedBuffLogic(string targetBuffName, float delay, float targetDuration = -1f, float chance = 1.0f)
        {
            _tasks.Add((targetBuffName, delay, targetDuration, chance));
        }

        /// <summary>
        /// 多任务构造函数
        /// 使用示例：new DelayedBuffLogic(("BuffA", 1s, 600s, 0.25f), ("BuffB", 1s, 600s, 0.25f))
        /// </summary>
        public DelayedBuffLogic(params (string name, float delay, float duration, float chance)[] tasks)
        {
            foreach (var t in tasks) _tasks.Add(t);
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            if (DelayedBuffManager.Instance == null)
            {
                ModLogger.LogError("DelayedBuffManager 实例不存在，无法注册延迟效果！");
                return;
            }

            foreach (var task in _tasks)
            {
                if (Random.value <= task.chance)
                {
                    DelayedBuffManager.Instance.AddTask(target, task.name, task.delay, task.duration, buff.fromWho);
                    
                    if (task.chance < 1.0f)
                    {
                        ModLogger.LogDebug($"[DelayedBuffLogic] 概率判定通过: {task.name} (几率: {task.chance * 100}%)");
                    }
                }
            }
        }

        public void OnUpdate(Buff buff, CharacterMainControl target) { }
        public void OnDestroy(Buff buff, CharacterMainControl target) { }
    }
}