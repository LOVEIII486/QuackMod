using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Duckov.Buffs;
using Duckov.Utilities;
using UnityEngine;
using QuackCore.Utils;

namespace QuackCore.BuffSystem
{
    public static class QuackBuffFactory
    {
        private static readonly Dictionary<int, Buff> SharedTemplates = new Dictionary<int, Buff>();
        private static readonly Dictionary<int, Buff> VanillaCache = new Dictionary<int, Buff>();
        
        private static FieldInfo _idField, _displayNameField, _limitedLifeTimeField, _totalLifeTimeField, _descriptionField, _iconField;
        private static bool _initialized = false;
        private static GameObject _templateRoot;
        
        public struct BuffConfig
        {
            public string ModID;
            public int ID;
            public string BuffName;
            public float Duration;
            public string IconPath;
            public string BuffNameKey => $"{ModID}_{BuffName}";
            public string DisplayName;
            public string BuffDescriptionKey => $"{BuffNameKey}_Desc";
            public string Description;

            public BuffConfig(string modId, string name, int manualId, float duration, string iconPath = null, string displayName = null, string description = null)
            {
                ModID = modId; BuffName = name; ID = manualId; Duration = duration; IconPath = iconPath;
                DisplayName = displayName;
                Description = description;
            }
        }
        
        public static void InitializeFactory()
        {
            if (_initialized) return;

            try
            {
                InitializeReflection();
                var buffsField = typeof(GameplayDataSettings.BuffsData).GetField("allBuffs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var buffsList = buffsField?.GetValue(GameplayDataSettings.Buffs) as List<Buff>;

                if (buffsList != null)
                {
                    VanillaCache.Clear();
                    foreach (var buff in buffsList)
                    {
                        if (buff != null && !VanillaCache.ContainsKey(buff.ID))
                        {
                            VanillaCache.Add(buff.ID, buff);
                        }
                    }
                    ModLogger.LogDebug($"[QuackBuffFactory] 成功缓存 {VanillaCache.Count} 个原生 Buff 信息。");
                }
                else
                {
                    ModLogger.LogWarning("[QuackBuffFactory] 无法从 GameplayDataSettings 获取 Buff 列表。");
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"[QuackBuffFactory] 初始化异常: {ex.Message}");
            }
        }
        
        public static Buff GetBuff(int id)
        {
            if (!_initialized) InitializeFactory();

            if (VanillaCache.TryGetValue(id, out var buff))
            {
                return buff;
            }

            ModLogger.LogWarning($"[QuackBuffFactory] 未能找到 ID 为 {id} 的 Buff。");
            return null;
        }

        public static List<Buff> GetAllBuffs()
        {
            if (!_initialized) InitializeFactory();
            return VanillaCache.Values.ToList();
        }
        
        /// <summary>
        /// 统一的 Buff 施加接口。
        /// </summary>
        /// <param name="target">施加目标</param>
        /// <param name="buffId">Buff 数字 ID</param>
        /// <param name="durationOverride">覆盖时长。省略或传入 NaN 则使用 Buff 默认时长；传入 -1f 表示永久。</param>
        /// <param name="attacker">来源角色</param>
        public static void Apply(CharacterMainControl target, int buffId, float durationOverride = float.NaN, CharacterMainControl attacker = null)
        {
            InitializeReflection();
            if (target == null) return;

            Buff template = GetTemplate(buffId) ?? GetBuff(buffId);

            if (template == null)
            {
                ModLogger.LogWarning($"[BuffFactory] 无法施加：找不到 ID 为 {buffId} 的定义或原版资源");
                return;
            }

            float baseDuration = (float)_totalLifeTimeField.GetValue(template);
            bool isOverrideProvided = !float.IsNaN(durationOverride);
            bool needsOverride = isOverrideProvided && Math.Abs(durationOverride - baseDuration) > 0.001f;

            if (needsOverride)
            {
                Buff tempClone = UnityEngine.Object.Instantiate(template);
                tempClone.gameObject.SetActive(false);
                try
                {
                    SetBuffDuration(tempClone, durationOverride);
                    target.AddBuff(tempClone, attacker, 1);
                    ModLogger.LogDebug($"[BuffFactory] 覆盖施加：ID {buffId}, 覆盖时长 {durationOverride}s (原时长 {baseDuration}s)");
                }
                finally
                {
                    UnityEngine.Object.Destroy(tempClone.gameObject);
                }
            }
            else
            {
                target.AddBuff(template, attacker, 1);
                ModLogger.LogDebug($"[BuffFactory] 直接施加：ID {buffId}, 使用默认时长 {baseDuration}s");
            }
        }

        public static Buff GetTemplate(int buffId)
        {
            SharedTemplates.TryGetValue(buffId, out var b);
            return b;
        }
        
        public static Buff CreateTemplate(BuffConfig config, string modPath, string modId)
        {
            InitializeReflection();
            
            if (SharedTemplates.TryGetValue(config.ID, out var b) && b != null) return b;

            if (_templateRoot == null)
            {
                _templateRoot = new GameObject("QuackBuffTemplates");
                _templateRoot.SetActive(false); 
                UnityEngine.Object.DontDestroyOnLoad(_templateRoot);
            }

            GameObject go = new GameObject(config.BuffNameKey);
            go.transform.SetParent(_templateRoot.transform);
            Buff newBuff = go.AddComponent<Buff>();
            
            try
            {
                _idField?.SetValue(newBuff, config.ID);
                _displayNameField?.SetValue(newBuff, config.BuffNameKey); 
                _descriptionField?.SetValue(newBuff, config.BuffDescriptionKey);
                _limitedLifeTimeField?.SetValue(newBuff, config.Duration > 0);
                _totalLifeTimeField?.SetValue(newBuff, config.Duration);
                
                if (!string.IsNullOrEmpty(config.IconPath) && _iconField != null)
                {
                    Sprite customIcon = SpriteLoader.LoadSprite(Path.Combine(modPath,config.IconPath));
                    if (customIcon != null)
                    {
                        _iconField.SetValue(newBuff, customIcon);
                    }
                }

                SharedTemplates[config.ID] = newBuff;
                ModLogger.LogDebug($"[BuffFactory] 成功创建模板 ID: {config.ID} 来自 Mod: {modId}");
                return newBuff;
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"填充模板失败 ID {config.ID}: {ex.Message}");
                return null;
            }
        }
        
        public static void DestroyTemplate(int buffId)
        {
            if (SharedTemplates.TryGetValue(buffId, out var buff) && buff != null)
            {
                UnityEngine.Object.Destroy(buff.gameObject);
                SharedTemplates.Remove(buffId);
                ModLogger.LogDebug($"已销毁 Buff 模板 ID: {buffId}");
            }
        }

        private static void SetBuffDuration(Buff buff, float duration)
        {
            if (_limitedLifeTimeField == null || _totalLifeTimeField == null) return;
            _limitedLifeTimeField.SetValue(buff, duration > 0);
            _totalLifeTimeField.SetValue(buff, duration);
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
            _iconField = t.GetField("icon", BindingFlags.Instance | BindingFlags.NonPublic);
            _initialized = true;
        }
    }
}