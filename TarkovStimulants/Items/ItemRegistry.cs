using QuackCore.Items;

namespace TarkovStimulants.Items
{
    public static class ItemRegistry
    {
        public static void RegisterAll(string modPath)
        {
            foreach (var quackItem in ItemDefinitions.AllQuackItems)
            {
                QuackItemRegistry.Register(modPath, quackItem, "TarkovStimulants");
            }
            
            ModLogger.Log($"成功注册了 {ItemDefinitions.AllQuackItems.Count} 个物品。");
        }
    }
}