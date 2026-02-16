using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuackCore.BuffSystem
{
    public static class QuackBuffRegistry
    {
        private static readonly Dictionary<int, QuackBuffDefinition> _definitions = new Dictionary<int, QuackBuffDefinition>();
        
        public static void Register(string dllPath, QuackBuffDefinition definition, string modId)
        {
            string modPath = Path.GetDirectoryName(dllPath);
            var template = QuackBuffFactory.CreateTemplate(definition.Config, modPath, modId);

            if (template == null)
            {
                ModLogger.LogError($"Buff ID {definition.Config.ID} 注册失败！");
                return;
            }

            int id = definition.Config.ID;
            if (_definitions.ContainsKey(id))
            {
                _definitions[id] = definition;
                ModLogger.LogWarning($"覆盖注册 Buff ID: {id} ({definition.Config.BuffNameKey})");
            }
            else
            {
                _definitions.Add(id, definition);
                ModLogger.Log($"成功注册 Buff ID: {id}");
            }
        }

        public static void UnregisterAll(string modId)
        {
            var idsToRemove = _definitions
                .Where(kvp => kvp.Value.Config.ModID == modId)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var id in idsToRemove)
            {
                QuackBuffFactory.DestroyTemplate(id);
                _definitions.Remove(id);
            }
            ModLogger.Log($"已完全清理 Mod {modId} 的所有 Buff。");
        }

        public static QuackBuffDefinition GetDefinition(int id)
        {
            _definitions.TryGetValue(id, out var def);
            return def;
        }

        public static bool IsQuackModBuff(int id)
        {
            return _definitions.ContainsKey(id);
        }
    }
}