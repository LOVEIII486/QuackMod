using System.Collections.Generic;

namespace QuackCore.BuffSystem
{
    public class QuackBuffRegistry
    {
        private static QuackBuffRegistry _instance;
        public static QuackBuffRegistry Instance => _instance ??= new QuackBuffRegistry();

        private readonly Dictionary<string, QuackBuffDefinition> _definitions = new Dictionary<string, QuackBuffDefinition>();
        private readonly HashSet<string> _modPrefixes = new HashSet<string>();

        public void Register(QuackBuffDefinition definition)
        {
            string buffNameKey = definition.Config.BuffNameKey;
            if (!_definitions.ContainsKey(buffNameKey))
            {
                _definitions.Add(buffNameKey, definition);
                _modPrefixes.Add(definition.Config.ModID);
                ModLogger.Log($"注册自定义buff: {buffNameKey}");
            }
        }

        public QuackBuffDefinition GetDefinition(string compositeName)
        {
            _definitions.TryGetValue(compositeName, out var def);
            return def;
        }

        public bool IsQuackModBuff(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            foreach (var prefix in _modPrefixes)
                if (name.StartsWith(prefix)) return true;
            return false;
        }
    }
}