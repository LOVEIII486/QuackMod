using QuackCore.NPC;

namespace QuackItem.NPC
{
    public static class NPCRegistry
    {
        public static void RegisterAll()
        {
            foreach (var npcDef in NPCDefinitions.AllNpcs)
            {
                QuackNPCRegistry.Register(npcDef);
            }
            
            ModLogger.Log($"[QuackItem] 成功注册了 {NPCDefinitions.AllNpcs.Count} 个自定义 NPC 配置。");
        }
    }
}