using System;

namespace QuackCore.NPC
{
    /// <summary>
    /// 自定义 NPC 定义
    /// </summary>
    [Serializable]
    public class QuackNPCDefinition
    {
        public string ID;
        public string ModID;

        public QuackNPCConfig Config;

        public QuackNPCDefinition(string id, string modId, QuackNPCConfig config )
        {
            this.ID = id;
            this.ModID = modId;
            this.Config = config;
        }
    }
}