using FastModdingLib;

namespace TarkovStimulants.Items
{
    public static class ItemRegistry
    {
        public static void RegisterAll(string modPath)
        {
            foreach (var item in ItemDefinitions.AllItems)
            {
                ItemUtils.CreateCustomItem(modPath, item, "TarkovStimulants");
            }
            ModLogger.Log($"[Item] 成功注册了 {ItemDefinitions.AllItems.Count} 个物品");
        }
    }
}