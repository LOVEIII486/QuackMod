using System.Collections.Generic;
using System.Linq;

namespace QuackCore.BuffSystem
{
    public class QuackBuffRegistry
    {
        private static QuackBuffRegistry _instance;
        public static QuackBuffRegistry Instance => _instance ??= new QuackBuffRegistry();

        private readonly Dictionary<int, QuackBuffDefinition> _definitions = new Dictionary<int, QuackBuffDefinition>();
        private readonly HashSet<string> _modPrefixes = new HashSet<string>();

        public void Register(QuackBuffDefinition definition)
        {
            int id = definition.Config.ID;
            
            if (_definitions.ContainsKey(id))
            {
                _definitions[id] = definition;
                ModLogger.LogDebug($"重新覆盖注册 Buff ID: {id} ({definition.Config.BuffNameKey})");
            }
            else
            {
                _definitions.Add(id, definition);
                ModLogger.LogDebug($"注册自定义 Buff ID: {id}");
            }

            _modPrefixes.Add(definition.Config.ModID);
            QuackBuffFactory.GetOrCreateTemplate(definition.Config);
        }

        public void UnregisterAll(string modId)
        {
            var idsToRemove = _definitions
                .Where(kvp => kvp.Value.Config.ModID == modId)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var id in idsToRemove)
            {
                _definitions.Remove(id);
            }

            if (!_definitions.Values.Any(d => d.Config.ModID == modId))
            {
                _modPrefixes.Remove(modId);
            }
        }

        public QuackBuffDefinition GetDefinition(int id)
        {
            _definitions.TryGetValue(id, out var def);
            return def;
        }

        public bool IsQuackModBuff(int id)
        {
            return _definitions.ContainsKey(id);
        }
    }
}