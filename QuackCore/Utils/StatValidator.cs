using System.Collections.Generic;

namespace QuackCore.Utils
{
    public static class StatValidator
    {
        private static readonly HashSet<string> _cachedKeys = new HashSet<string>();

        public static HashSet<string> GetCachedKeys()
        {
            if (_cachedKeys.Count == 0) RefreshCache();
            return _cachedKeys;
        }

        public static bool IsValid(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            if (_cachedKeys.Count == 0) RefreshCache();
            return _cachedKeys.Contains(key);
        }

        public static void RefreshCache()
        {
            var main = CharacterMainControl.Main;
            if (main?.CharacterItem?.Stats != null)
            {
                _cachedKeys.Clear();
                foreach (var s in main.CharacterItem.Stats)
                {
                    if (!string.IsNullOrEmpty(s.Key)) _cachedKeys.Add(s.Key);
                }
            }
        }

        public static void LogAllKeys()
        {
            RefreshCache();
            var main = CharacterMainControl.Main;
            if (main?.CharacterItem?.Stats != null)
            {
                ModLogger.Log($"[StatValidator] 当前可用属性总数: {main.CharacterItem.Stats.Count}");
                foreach (var s in main.CharacterItem.Stats)
                {
                    ModLogger.Log($" -> ID: {s.Key} | 名称: {s.DisplayName}");
                }
            }
        }
    }
}