using System.Collections.Generic;
namespace QuackCore.BuffSystem
{
    public static class QuackImmunityHandler
    {
        private static readonly Dictionary<CharacterMainControl, Dictionary<int, int>> CharacterImmunities = new();

        public static void RegisterImmunity(CharacterMainControl target, int[] ids)
        {
            if (target == null || ids == null) return;
            if (!CharacterImmunities.TryGetValue(target, out var immuneMap))
            {
                immuneMap = new Dictionary<int, int>();
                CharacterImmunities[target] = immuneMap;
            }

            foreach (var id in ids)
            {
                if (immuneMap.ContainsKey(id)) immuneMap[id]++;
                else immuneMap[id] = 1;
            }
        }

        public static void UnregisterImmunity(CharacterMainControl target, int[] ids)
        {
            if (target == null || ids == null) return;
            if (CharacterImmunities.TryGetValue(target, out var immuneMap))
            {
                foreach (var id in ids)
                {
                    if (immuneMap.ContainsKey(id))
                    {
                        immuneMap[id]--;
                        if (immuneMap[id] <= 0) immuneMap.Remove(id);
                    }
                }
            }
        }

        public static bool IsImmune(CharacterMainControl target, int buffID)
        {
            if (target == null) return false;
            return CharacterImmunities.TryGetValue(target, out var immuneMap) && immuneMap.ContainsKey(buffID);
        }
    }
}