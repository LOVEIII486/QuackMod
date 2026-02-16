using System;

namespace QuackCore.NPC
{
    /// <summary>
    /// 自定义 NPC 定义
    /// </summary>
    [Serializable]
    public class QuackNPCDefinition
    {
        public string Id;
        public string ModId;

        public QuackNPCConfig Config;

        public QuackNPCDefinition(string id, string modId, QuackNPCConfig config )
        {
            this.Id = id;
            this.ModId = modId;
            this.Config = config;
        }
    }
}