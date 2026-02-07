using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuackCore.BuffSystem; // 确保引用了重构后的核心库
using Duckov.Buffs;

namespace QuackItem.Buffs
{
    // 改为实现 IQuackBuffLogic 接口，用于处理复杂的协程逻辑
    public class QuackItemTextEffect : IQuackBuffLogic
    {
        private readonly Dictionary<int, Coroutine> _activeCoroutines = new Dictionary<int, Coroutine>();

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            if (target == null) return;

            // 由于 Buff 是 MonoBehaviour，我们直接在 buff 实例上启动协程
            // 这样当 Buff 被销毁时，协程也会随之自动停止，更加安全
            Coroutine routine = buff.StartCoroutine(TextTickRoutine(target));
            _activeCoroutines[buff.GetInstanceID()] = routine;
            
            ModLogger.Log($"[QuackItem] 开启文字 Tick 特效: {target.name}");
        }

        // 接口必须实现的方法，此处不需要实时更新，留空即可
        public void OnUpdate(Buff buff, CharacterMainControl target) { }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
            int id = buff.GetInstanceID();
            if (_activeCoroutines.TryGetValue(id, out var routine))
            {
                if (buff != null) buff.StopCoroutine(routine);
                _activeCoroutines.Remove(id);
            }
            ModLogger.Log($"[QuackItem] 关闭文字 Tick 特效: {target.name}");
        }

        private IEnumerator TextTickRoutine(CharacterMainControl target)
        {
            while (target != null && !target.Health.IsDead)
            {
                // 这里的 PopText 是游戏内置弹窗文字方法
                target.PopText("<color=yellow>Quack Tick!</color>");
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}