using QuackCore.Items;
using QuackItem.Constants;

namespace QuackItem.Items
{
    public static class ItemRegistry
    {
        public static void RegisterAll(string modPath)
        {
            foreach (var quackItem in ItemDefinitions.AllQuackItems)
            {
                QuackItemRegistry.Register(modPath, quackItem, ModConstant.ModId);
            }
            ModLogger.Log($"注册了 {ItemDefinitions.AllQuackItems.Count} 个物品。");
        }
        
        public static void UnregisterAll()
        {
            QuackItemRegistry.UnregisterAll(ModConstant.ModId);
            ModLogger.Log($"注销了 {ItemDefinitions.AllQuackItems.Count} 个物品。");
        }
    }
}