using QuackCore.BuffSystem;

namespace QuackItem.Buffs
{
    public static class BuffRegistry
    {
        public static void RegisterAll()
        {
            foreach (var buff in BuffDefinitions.AllBuffs)
            {
                QuackBuffRegistry.Instance.Register(buff);
            }
            ModLogger.Log($"成功注册了 {BuffDefinitions.AllBuffs.Count} 个 Buff");
        }
    }
}