using Duckov.Buffs;
using System.Collections.Generic;

namespace QuackCore.BuffSystem.Logic
{
    public class DelayedBuffLogic : IQuackBuffLogic
    {
        private readonly List<(string name, float delay, float duration)> _tasks = new();

        public DelayedBuffLogic(string targetBuffName, float delay, float targetDuration = -1f)
        {
            _tasks.Add((targetBuffName, delay, targetDuration));
        }

        public DelayedBuffLogic(params (string name, float delay, float duration)[] tasks)
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
                DelayedBuffManager.Instance.AddTask(target, task.name, task.delay, task.duration, buff.fromWho);
            }
        }

        public void OnUpdate(Buff buff, CharacterMainControl target) { }
        public void OnDestroy(Buff buff, CharacterMainControl target) { }
    }
}