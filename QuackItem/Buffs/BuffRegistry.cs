using QuackCore.BuffSystem;
using QuackItem.Buffs.Effects;

namespace QuackItem.Buffs
{
    public static class BuffRegistry
    {
        public static void RegisterAll()
        {
            var config = new QuackBuffFactory.BuffConfig
            {
                ModID =  "QuackItem",
                BuffName =  "QuackMaxHpBuff",
                ID = 999001,
                Duration = 10f,
            };

            var healthBuff = new QuackBuffDefinition(config)
                .SetCustomLogic(new MaxHpLogic(50f));

            QuackBuffRegistry.Instance.Register(healthBuff);
            
            ModLogger.Log("自定义 Buff 注册成功。");
        }
    }
}