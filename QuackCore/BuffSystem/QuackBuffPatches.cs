using System;
using System.Collections.Generic;
using Duckov.Buffs;
using HarmonyLib;
using ItemStatsSystem;
using QuackCore.AttributeModifier;

namespace QuackCore.BuffSystem
{
    [HarmonyPatch(typeof(Buff), "Setup")]
    public static class QuackBuffSetupPatch
    {
        [HarmonyPrefix]
        public static void Prefix(Buff __instance, ref List<Effect> ___effects)
        {
            if (QuackBuffRegistry.Instance.IsQuackModBuff(__instance.name))
                ___effects.Clear();
        }

        [HarmonyPostfix]
        public static void Postfix(Buff __instance, CharacterBuffManager manager)
        {
            var def = QuackBuffRegistry.Instance.GetDefinition(CleanName(__instance.name));
            if (def != null && manager?.Master != null)
                def.ExecuteSetup(__instance, manager.Master);
        }
        
        private static string CleanName(string n) => n.Replace("(Clone)", "").Replace("_Permanent", "").Trim();
    }

    [HarmonyPatch(typeof(Buff), "OnDestroy")]
    public static class QuackBuffDestroyPatch
    {
        [HarmonyPrefix]
        public static void Prefix(Buff __instance)
        {
            var target = __instance.Character;
            var def = QuackBuffRegistry.Instance.GetDefinition(CleanName(__instance.name));
            
            if (def != null && target != null)
                def.ExecuteDestroy(__instance, target);

            // 用 AttributeModifier 统一清理
            // 只要是在 AttributeModifierAction 中以 __instance (buff对象) 为 source 添加的修改
            // 都会在这里被一次性清理掉，避免遗漏
            if (target != null)
            {
                CharacterModifier.ClearAll(target, __instance);
            }
        }
        
        private static string CleanName(string n) => n.Replace("(Clone)", "").Replace("_Permanent", "").Trim();
    }
}