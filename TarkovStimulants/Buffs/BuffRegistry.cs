using QuackCore.BuffSystem;
using QuackCore.BuffSystem.Logic;

namespace TarkovStimulants.Buffs
{
    public static class BuffRegistry
    {
        public static void RegisterAll()
        {
            var etgcBuff = new QuackBuffDefinition(new QuackBuffFactory.BuffConfig("TarkovStimulants", "eTG-c_Buff",999001, 15f))
                .SetCustomLogic(new RegenerationLogic(0.2f, 1.0f, true));
            
            QuackBuffRegistry.Instance.Register(etgcBuff);
            
            ModLogger.Log("自定义 Buff 注册成功。");
        }
    }
}