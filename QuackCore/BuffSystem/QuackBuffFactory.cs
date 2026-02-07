using System;
using System.Collections.Generic;
using System.Reflection;
using Duckov.Buffs;
using SodaCraft.Localizations;
using UnityEngine;

namespace QuackCore.BuffSystem
{
    public static class QuackBuffFactory
    {
        private static readonly Dictionary<string, Buff> SharedTemplates = new Dictionary<string, Buff>();
        private static FieldInfo _idField, _displayNameField, _limitedLifeTimeField, _totalLifeTimeField, _descriptionField;
        private static bool _initialized = false;
        private static GameObject _templateRoot;
        
        public struct BuffConfig
        {
            public string ModID;
            public string BuffName;
            public int Id; 
            public float Duration;
            
            public string BuffNameKey => $"{ModID}_{BuffName}";
            public string DisplayName;
            public string BuffDescriptionKey => $"{BuffNameKey}_Desc";
            public string Description;

            // 构造函数 A：自动哈希 ID，支持可选本地化文本
            public BuffConfig(string modId, string name, float duration = 5f, string displayName = null, string description = null)
            {
                ModID = modId; BuffName = name; Duration = duration;
                DisplayName = displayName;
                Description = description;
                Id = 0; 
            }

            // 构造函数 B：手动指定 ID，支持可选本地化文本
            public BuffConfig(string modId, string name, int manualId, float duration = 5f, string displayName = null, string description = null)
            {
                ModID = modId; BuffName = name; Id = manualId; Duration = duration;
                DisplayName = displayName;
                Description = description;
            }
        }

        public static void Apply(CharacterMainControl target, string compositeName, CharacterMainControl attacker = null)
        {
            var def = QuackBuffRegistry.Instance.GetDefinition(compositeName);
            if (def == null) return;

            var template = GetOrCreateTemplate(def.Config);
            if (template != null)
            {
                target.AddBuff(template, attacker, 1);
            }
        }

        public static Buff GetOrCreateTemplate(BuffConfig config)
        {
            InitializeReflection();
            string buffNameKey = config.BuffNameKey;
            string buffdescKey = config.BuffDescriptionKey;
            
            if (SharedTemplates.TryGetValue(buffNameKey, out var b) && b != null) return b;

            if (_templateRoot == null)
            {
                _templateRoot = new GameObject("QuackBuffTemplates");
                _templateRoot.SetActive(false); 
                UnityEngine.Object.DontDestroyOnLoad(_templateRoot);
            }

            int finalId = config.Id != 0 ? config.Id : Math.Abs(buffNameKey.GetHashCode());

            GameObject go = new GameObject(buffNameKey);
            go.transform.SetParent(_templateRoot.transform);
            Buff newBuff = go.AddComponent<Buff>();
            
            if (!string.IsNullOrEmpty(config.DisplayName))
            {
                LocalizationManager.SetOverrideText(buffNameKey, config.DisplayName);
            }
            if (!string.IsNullOrEmpty(config.Description))
            {
                LocalizationManager.SetOverrideText(buffdescKey, config.Description);
            }
            
            try
            {
                _idField?.SetValue(newBuff, finalId);
                _displayNameField?.SetValue(newBuff, buffNameKey); 
                _descriptionField?.SetValue(newBuff, buffdescKey);
                _limitedLifeTimeField?.SetValue(newBuff, config.Duration > 0);
                _totalLifeTimeField?.SetValue(newBuff, config.Duration);

                SharedTemplates[buffNameKey] = newBuff;
                ModLogger.Log($"创建 Buff 模板: {buffNameKey} (ID: {finalId}, 永久: {config.Duration <= 0})");
                return newBuff;
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"填充 Buff 模板失败: {ex.Message}");
                return null;
            }
        }

        private static void InitializeReflection()
        {
            if (_initialized) return;
            var t = typeof(Buff);
            _idField = t.GetField("id", BindingFlags.Instance | BindingFlags.NonPublic);
            _displayNameField = t.GetField("displayName", BindingFlags.Instance | BindingFlags.NonPublic);
            _descriptionField = t.GetField("description", BindingFlags.Instance | BindingFlags.NonPublic);
            _limitedLifeTimeField = t.GetField("limitedLifeTime", BindingFlags.Instance | BindingFlags.NonPublic);
            _totalLifeTimeField = t.GetField("totalLifeTime", BindingFlags.Instance | BindingFlags.NonPublic);
            _initialized = true;
        }
    }
}