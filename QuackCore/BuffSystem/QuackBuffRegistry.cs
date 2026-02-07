using System.Collections.Generic;
using System.Linq;

namespace QuackCore.BuffSystem
{
    public class QuackBuffRegistry
    {
        private static QuackBuffRegistry _instance;
        public static QuackBuffRegistry Instance => _instance ??= new QuackBuffRegistry();

        private readonly Dictionary<string, QuackBuffDefinition> _definitions = new Dictionary<string, QuackBuffDefinition>();
        
        private readonly HashSet<string> _modPrefixes = new HashSet<string>();

        /// <summary>
        /// 注册自定义 Buff
        /// </summary>
        public void Register(QuackBuffDefinition definition)
        {
            string buffNameKey = definition.Config.BuffNameKey;
            
            // 允许覆盖注册
            if (_definitions.ContainsKey(buffNameKey))
            {
                _definitions[buffNameKey] = definition;
                ModLogger.LogDebug($"重新覆盖注册 Buff: {buffNameKey}");
            }
            else
            {
                _definitions.Add(buffNameKey, definition);
                ModLogger.LogDebug($"注册自定义 Buff: {buffNameKey}");
            }

            _modPrefixes.Add(definition.Config.ModID);
            QuackBuffFactory.GetOrCreateTemplate(definition.Config);
        }

        /// <summary>
        /// 安全注销指定模组的所有 Buff
        /// </summary>
        public void UnregisterAll(string modId)
        {
            var keysToRemove = _definitions
                .Where(kvp => kvp.Value.Config.ModID == modId)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _definitions.Remove(key);
                ModLogger.LogDebug($"注销自定义 Buff: {key}");
            }

            if (!_definitions.Values.Any(d => d.Config.ModID == modId))
            {
                _modPrefixes.Remove(modId);
            }
        }

        public QuackBuffDefinition GetDefinition(string compositeName)
        {
            if (string.IsNullOrEmpty(compositeName)) return null;
            _definitions.TryGetValue(compositeName, out var def);
            return def;
        }

        public bool IsQuackModBuff(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            if (_definitions.ContainsKey(name)) return true;
            
            foreach (var prefix in _modPrefixes)
                if (name.StartsWith(prefix)) return true;
            return false;
        }
    }
}