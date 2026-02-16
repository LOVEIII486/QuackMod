using QuackCore.NPC;
using QuackItem.Constants;

namespace QuackItem.NPC
{
    public static class NPCRegistry
    {
        public static void RegisterAll(string modPath)
        {
            foreach (var npcDef in NPCDefinitions.AllNpcs)
            {
                QuackNPCRegistry.Register(modPath, npcDef, ModConstant.ModId);
            }
            ModLogger.Log($"注册了 {NPCDefinitions.AllNpcs.Count} 个自定义 NPC 配置");
        }

        public static void UnregisterAll()
        {
            QuackNPCRegistry.UnregisterAll(ModConstant.ModId);
            ModLogger.Log($"注销了 {NPCDefinitions.AllNpcs.Count} 个自定义 NPC 配置");
        }
    }
}