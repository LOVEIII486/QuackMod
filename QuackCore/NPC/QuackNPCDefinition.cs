using System;

namespace QuackCore.NPC
{
    /// <summary>
    /// 自定义 NPC 定义，包含 ID 和对应的配置
    /// </summary>
    [Serializable]
    public class QuackNPCDefinition
    {
        /// <summary>
        /// 唯一标识符，用于在注册表中查找
        /// </summary>
        public string Id;

        /// <summary>
        /// NPC 的具体属性配置
        /// </summary>
        public QuackNPCConfig Config;

        public QuackNPCDefinition(string id, QuackNPCConfig config)
        {
            this.Id = id;
            this.Config = config;
        }
    }
}