using System.Collections.Generic;
using System.Linq;

namespace QuackCore.NPC
{
    public static class QuackNPCRegistry
    {
        private static readonly Dictionary<string, QuackNPCDefinition> _definitions = new Dictionary<string, QuackNPCDefinition>();

        public static void Register(string dllPath, QuackNPCDefinition definition, string modId)
        {
            if (definition == null || string.IsNullOrEmpty(definition.Id)) return;

            if (QuackSpawner.Instance != null)
            {
                var source = QuackSpawner.Instance.GetNativePreset(definition.Config.BasePresetName);
                if (source == null)
                {
                    ModLogger.LogError($"NPC {definition.Id} 注册失败：找不到基底预设 {definition.Config.BasePresetName}");
                    return;
                }
                var preset = QuackSpawner.Instance.CreateCustomPreset(source, definition.Config);
                if (preset == null)
                {
                    ModLogger.LogError($"NPC {definition.Id} 注册失败：Spawner 无法生成预制体副本。");
                    return;
                }
            }
            else
            {
                ModLogger.LogError($"NPC {definition.Id} 注册失败：QuackSpawner 实例尚未准备好。");
                return;
            }

            string id = definition.Id;
            if (_definitions.ContainsKey(id))
            {
                _definitions[id] = definition;
                ModLogger.LogWarning($"覆盖注册 NPC ID: {id} (来自 Mod: {modId})");
            }
            else
            {
                _definitions.Add(id, definition);
                ModLogger.Log($"成功注册 NPC ID: {id}");
            }
        }
        
        public static void UnregisterAll(string modId)
        {
            var idsToRemove = _definitions
                .Where(kvp => kvp.Value.ModId == modId)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var id in idsToRemove)
            {
                var def = _definitions[id];
                if (QuackSpawner.Instance != null)
                {
                    QuackSpawner.Instance.RemoveCustomPreset(def.Config);
                }

                _definitions.Remove(id);
            }

            ModLogger.Log($"已清理来自 Mod {modId} 的所有 NPC 定义。");
        }

        public static QuackNPCDefinition GetDefinition(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _definitions.TryGetValue(id, out var def);
            return def;
        }

        public static QuackNPCConfig GetConfig(string id)
        {
            return GetDefinition(id)?.Config;
        }
    }
}