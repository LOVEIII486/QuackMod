using System.Collections.Generic;

namespace QuackCore.NPC
{
    /// <summary>
    /// 全局 NPC 注册表
    /// </summary>
    public static class QuackNPCRegistry
    {
        private static readonly Dictionary<string, QuackNPCDefinition> _definitions = new Dictionary<string, QuackNPCDefinition>();

        /// <summary>
        /// 注册一个新的 NPC 定义
        /// </summary>
        public static void Register(QuackNPCDefinition definition)
        {
            if (definition == null || string.IsNullOrEmpty(definition.Id)) return;
            _definitions[definition.Id] = definition;
        }

        /// <summary>
        /// 根据 ID 获取 NPC 配置
        /// </summary>
        public static QuackNPCConfig GetConfig(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _definitions.TryGetValue(id, out var def) ? def.Config : null;
        }
    }
}