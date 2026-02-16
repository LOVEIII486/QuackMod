using System.Collections.Generic;
using UnityEngine;

namespace QuackCore.BuffSystem
{
    public class DelayedBuffManager : MonoBehaviour
    {
        public static DelayedBuffManager Instance { get; private set; }

        private class DelayedTask
        {
            public CharacterMainControl Target;
            public int BuffId;
            public float RemainingTime;
            public float Duration;
            public CharacterMainControl Attacker;
        }

        private readonly List<DelayedTask> _activeTasks = new List<DelayedTask>();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 注册一个延迟施加 Buff 的任务。
        /// </summary>
        /// <param name="target">目标角色</param>
        /// <param name="buffId">Buff 的数字 ID</param>
        /// <param name="delay">延迟多长时间施加 (秒)</param>
        /// <param name="duration">施加后的持续时间 (秒)</param>
        /// <param name="fromWho">来源角色</param>
        public void AddTask(CharacterMainControl target, int buffId, float delay, float duration, CharacterMainControl fromWho)
        {
            if (target == null) return;

            _activeTasks.Add(new DelayedTask
            {
                Target = target,
                BuffId = buffId,
                RemainingTime = delay,
                Duration = duration,
                Attacker = fromWho
            });
            
            ModLogger.LogDebug($"[DelayedBuffManager] 注册任务：ID {buffId}, 将在 {delay}s 后施加给 {target.name}");
        }

        private void Update()
        {
            if (_activeTasks.Count == 0) return;

            float dt = Time.deltaTime;
            for (int i = _activeTasks.Count - 1; i >= 0; i--)
            {
                var task = _activeTasks[i];

                if (task.Target == null || task.Target.Health == null || task.Target.Health.IsDead)
                {
                    _activeTasks.RemoveAt(i);
                    continue;
                }

                task.RemainingTime -= dt;
                if (task.RemainingTime <= 0f)
                {
                    QuackBuffFactory.Apply(task.Target, task.BuffId, task.Duration, task.Attacker);
                    _activeTasks.RemoveAt(i);
                    ModLogger.LogDebug($"[DelayedBuffManager] 任务执行完成：ID {task.BuffId} 已施加");
                }
            }
        }

        /// <summary>
        /// 清理指定角色的所有延迟任务
        /// </summary>
        public void ClearTasksForTarget(CharacterMainControl target)
        {
            if (target == null) return;
            _activeTasks.RemoveAll(t => t.Target == target);
        }
    }
}