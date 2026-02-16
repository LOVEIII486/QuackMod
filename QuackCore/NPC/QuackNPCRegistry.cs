using System.Collections.Generic;
using System.Linq;
using FastModdingLib;

namespace QuackCore.NPC
{
    public static class QuackNPCRegistry
    {
        private static readonly Dictionary<string, QuackNPCDefinition> _definitions = new Dictionary<string, QuackNPCDefinition>();

        public static void Register(string dllPath, QuackNPCDefinition definition, string modId)
        {
            if (definition == null || string.IsNullOrEmpty(definition.ID)) return;

            var source = QuackSpawner.GetNativePresetByName(definition.Config.BasePresetName);
            if (source == null)
            {
                ModLogger.LogError($"NPC {definition.ID} 注册失败：找不到基底预设 {definition.Config.BasePresetName}");
                return;
            }

            var template = QuackNPCFactory.CreateTemplate(source, definition);
            if (template == null) return;

            definition.ModID = modId;
            if (_definitions.ContainsKey(definition.ID))
            {
                _definitions[definition.ID] = definition;
                ModLogger.LogWarning($"覆盖注册 NPC ID: {definition.ID} (Mod: {modId})");
            }
            else
            {
                _definitions.Add(definition.ID, definition);
                ModLogger.Log($"成功注册 NPC ID: {definition.ID} (Mod: {modId})");
            }
        }

        public static void UnregisterAll(string modId)
        {
            var idsToRemove = _definitions
                .Where(kvp => kvp.Value.ModID == modId)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var id in idsToRemove)
            {
                QuackNPCFactory.DestroyTemplate(id);
                _definitions.Remove(id);
            }
            ModLogger.Log($"已清理 Mod {modId} 的所有 NPC 定义。");
        }

        public static QuackNPCDefinition GetDefinition(string id)
        {
            _definitions.TryGetValue(id, out var def);
            return def;
        }
        
        public static QuackNPCConfig GetConfig(string id)
        {
            _definitions.TryGetValue(id, out var def);
            return def?.Config;
        }
    }
}