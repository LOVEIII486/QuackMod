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

        [HarmonyPatch(typeof(Buff), "Setup")]
        public static class SetupPatch
        {
            [HarmonyPrefix]
            public static void Prefix(Buff __instance, ref List<Effect> ___effects)
            {
                if (QuackBuffRegistry.Instance.IsQuackModBuff(__instance.ID))
                {
                    ___effects.Clear();
                }
            }

            [HarmonyPostfix]
            public static void Postfix(Buff __instance, CharacterBuffManager manager)
            {
                var def = QuackBuffRegistry.Instance.GetDefinition(__instance.ID);
                if (def != null)
                {
                    _defCache.AddOrUpdate(__instance, def);
                    if (manager?.Master != null)
                    {
                        def.ExecuteSetup(__instance, manager.Master);
                    }
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
                    {
                        def.ExecuteUpdate(__instance, target);
                    }
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
                    {
                        def.ExecuteDestroy(__instance, target);
                    }
                    _defCache.Remove(__instance);
                }

                if (target != null)
                {
                    CharacterModifier.ClearAll(target, __instance);
                }
            }
        }
        
        [HarmonyPatch(typeof(CharacterBuffManager), "AddBuff")]
        public static class QuackImmunityAddBuffPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(CharacterBuffManager __instance, Buff buffPrefab)
            {
                if (buffPrefab == null) return true;

                var target = __instance.Master;
                int id = buffPrefab.ID;

                if (target != null && QuackImmunityHandler.IsImmune(target, id))
                {
                    // ModLogger.LogDebug($"[Immunity] 拦截 Buff ID: {id}");
                    return false; 
                }
                return true;
            }
        }
    }
}