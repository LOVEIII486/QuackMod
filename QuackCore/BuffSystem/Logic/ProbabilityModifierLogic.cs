using System.Collections.Generic;
using Duckov.Buffs;
using QuackCore.AttributeModifier;
using UnityEngine;

namespace QuackCore.BuffSystem.Logic
{
    public class ProbabilityModifierLogic : IQuackBuffLogic
    {
        public struct ModifierEntry
        {
            public string StatName;
            public float Value;
            public bool IsMultiplier;
            public float Chance;

            public ModifierEntry(string statName, float value, bool isMultiplier, float chance)
            {
                StatName = statName;
                Value = value;
                IsMultiplier = isMultiplier;
                Chance = chance;
            }
        }

        private readonly List<ModifierEntry> _entries = new();

        public ProbabilityModifierLogic(params ModifierEntry[] entries)
        {
            foreach (var entry in entries) _entries.Add(entry);
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            foreach (var entry in _entries)
            {
                if (Random.value <= entry.Chance)
                {
                    CharacterModifier.Modify(target, entry.StatName, entry.Value, entry.IsMultiplier, buff);
                    ModLogger.LogDebug($"触发概率属性: {entry.StatName} += {entry.Value} (几率: {entry.Chance * 100}%)");
                }
            }
        }

        public void OnUpdate(Buff buff, CharacterMainControl target) { }
        public void OnDestroy(Buff buff, CharacterMainControl target) { }
    }
}