using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov.Buffs;
using HarmonyLib;
using ItemStatsSystem;
using QuackCore.AttributeModifier;

namespace QuackCore.BuffSystem
{
    public static class QuackBuffPatches
    {
        private static readonly ConditionalWeakTable<Buff, QuackBuffDefinition> _defCache = new();

        private static string CleanName(string n) => n.Replace("(Clone)", "").Replace("_Permanent", "").Trim();

        [HarmonyPatch(typeof(Buff), "Setup")]
        public static class SetupPatch
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
                if (def != null)
                {
                    // 存入缓存，供后续 Update 使用
                    _defCache.AddOrUpdate(__instance, def);
                    if (manager?.Master != null)
                        def.ExecuteSetup(__instance, manager.Master);
                }
            }
        }

        [HarmonyPatch(typeof(Buff), "NotifyUpdate")]
        public static class UpdatePatch
        {
            [HarmonyPostfix]
            public static void Postfix(Buff __instance)
            {
                if (_defCache.TryGetValue(__instance, out var def))
                {
                    var target = __instance.Character;
                    if (target != null)
                        def.ExecuteUpdate(__instance, target);
                }
            }
        }

        [HarmonyPatch(typeof(Buff), "OnDestroy")]
        public static class DestroyPatch
        {
            [HarmonyPrefix]
            public static void Prefix(Buff __instance)
            {
                var target = __instance.Character;

                if (_defCache.TryGetValue(__instance, out var def))
                {
                    if (target != null)
                        def.ExecuteDestroy(__instance, target);
                    _defCache.Remove(__instance);
                }

                if (target != null)
                {
                    CharacterModifier.ClearAll(target, __instance);
                }
            }
        }
    }
}