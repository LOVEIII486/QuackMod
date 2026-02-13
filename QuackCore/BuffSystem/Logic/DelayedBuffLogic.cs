using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov.Buffs;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class DelayedBuffLogic : IQuackBuffLogic
    {
        private class DelayedTask
        {
            public string BuffName;
            public float Delay;
            public float Duration;
            public bool IsTriggered;

            public DelayedTask(string name, float delay, float duration)
            {
                BuffName = name;
                Delay = delay;
                Duration = duration;
                IsTriggered = false;
            }
        }

        private class LogicState
        {
            public float ElapsedTime;
            public List<DelayedTask> Tasks;
        }

        private static readonly ConditionalWeakTable<Buff, LogicState> _instanceStates = new();
        private readonly List<DelayedTask> _taskTemplates = new();

        public DelayedBuffLogic(string targetBuffName, float delay, float targetDuration = -1f)
        {
            _taskTemplates.Add(new DelayedTask(targetBuffName, delay, targetDuration));
        }

   
        public DelayedBuffLogic(params (string name, float delay, float duration)[] tasks)
        {
            foreach (var task in tasks)
            {
                _taskTemplates.Add(new DelayedTask(task.name, task.delay, task.duration));
            }
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            var state = new LogicState
            {
                ElapsedTime = 0f,
                Tasks = new List<DelayedTask>()
            };

            foreach (var t in _taskTemplates)
            {
                state.Tasks.Add(new DelayedTask(t.BuffName, t.Delay, t.Duration));
            }

            _instanceStates.Add(buff, state);
        }

        public void OnUpdate(Buff buff, CharacterMainControl target)
        {
            if (!_instanceStates.TryGetValue(buff, out var state)) return;

            state.ElapsedTime += Time.deltaTime;

            bool allFinished = true;
            foreach (var task in state.Tasks)
            {
                if (task.IsTriggered) continue;

                allFinished = false;

                if (state.ElapsedTime >= task.Delay)
                {
                    task.IsTriggered = true;
                    QuackBuffFactory.Apply(target, task.BuffName, task.Duration, buff.fromWho);
                    ModLogger.LogDebug($"[DelayedBuffLogic] 延迟触发: {task.BuffName} (延时: {task.Delay}s)");
                }
            }
        }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
            _instanceStates.Remove(buff);
        }
    }
}