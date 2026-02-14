using System.Collections.Generic;
using UnityEngine;

namespace QuackCore.BuffSystem
{
    public class DelayedBuffManager : MonoBehaviour
    {
        public static DelayedBuffManager Instance { get; private set; }

        private class DelayedTask
        {
            public CharacterMainControl Target;    // 目标
            public string BuffName;               // 要施加的Buff
            public float RemainingTime;           // 剩余延迟时间
            public float Duration;                // 效果持续时间
            public CharacterMainControl Attacker; // 来源
        }

        private readonly List<DelayedTask> _activeTasks = new List<DelayedTask>();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 注册一个延迟施加 Buff 的任务
        /// </summary>
        public void AddTask(CharacterMainControl target, string buffName, float delay, float duration, CharacterMainControl fromWho)
        {
            if (target == null || string.IsNullOrEmpty(buffName)) return;

            _activeTasks.Add(new DelayedTask
            {
                Target = target,
                BuffName = buffName,
                RemainingTime = delay,
                Duration = duration,
                Attacker = fromWho
            });
            
            ModLogger.Log($"[DelayedBuffManager] 注册任务: {buffName}, 将在 {delay}s 后施加给 {target.name}");
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
                    QuackBuffFactory.Apply(task.Target, task.BuffName, task.Duration, task.Attacker);
                    _activeTasks.RemoveAt(i);
                    ModLogger.LogDebug($"[DelayedBuffManager] 任务执行: {task.BuffName} 已施加");
                }
            }
        }

        // 场景清理或死亡清理（可选）
        public void ClearTasksFor(CharacterMainControl target)
        {
            _activeTasks.RemoveAll(t => t.Target == target);
        }
    }
}