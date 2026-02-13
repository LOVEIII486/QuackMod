using System;
using ItemStatsSystem.Stats;
using System.Collections.Generic;
using System.Reflection;
using QuackCore.Constants;
using QuackCore.Utils;

namespace QuackCore.AttributeModifier
{
    /// <summary>
    /// 统一属性修改器
    /// </summary>
    public static class CharacterModifier
    {
        private static readonly HashSet<string> AI_FIELDS;
        
        static CharacterModifier()
        {
            AI_FIELDS = new HashSet<string>();
            FieldInfo[] fields = typeof(ModifierKeyConstant.AIField).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    AI_FIELDS.Add(field.GetValue(null).ToString());
                }
            }
        }
        
        public static class Quick
        {
            /// <summary>
            /// 修改血量上限并补满
            /// </summary>
            public static void ModifyHealth(CharacterMainControl target, float multiplier, object source, bool healToFull = true)
            {
                float val = multiplier - 1f;
                StatModifier.AddModifier(target, ModifierKeyConstant.Stat.MaxHealth, val, ModifierType.PercentageMultiply, source);
                if (healToFull && target?.Health != null)
                {
                    target.Health.SetHealth(target.Health.MaxHealth);
                }
            }

            /// <summary>
            /// 修改远程与近战伤害
            /// </summary>
            public static void ModifyDamage(CharacterMainControl target, float multiplier, object source)
            {
                float val = multiplier - 1f;
                StatModifier.AddModifier(target, ModifierKeyConstant.Stat.GunDamageMultiplier, val, ModifierType.PercentageMultiply, source);
                StatModifier.AddModifier(target, ModifierKeyConstant.Stat.MeleeDamageMultiplier, val, ModifierType.PercentageMultiply, source);
            }

            /// <summary>
            /// 修改移动速度
            /// </summary>
            public static void ModifySpeed(CharacterMainControl target, float multiplier, object source)
            {
                if (target == null) return;
                float val = multiplier - 1f;
                StatModifier.AddModifier(target, ModifierKeyConstant.Stat.WalkSpeed, val, ModifierType.PercentageMultiply, source);
                StatModifier.AddModifier(target, ModifierKeyConstant.Stat.RunSpeed, val, ModifierType.PercentageMultiply, source);
            }
        }
        
        public static void SelfCheck()
        {
            try
            {
                HashSet<string> availableKeys = StatValidator.GetCachedKeys();
                
                HashSet<string> constantKeys = new HashSet<string>();
                FieldInfo[] fields = typeof(ModifierKeyConstant.Stat).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (FieldInfo field in fields)
                {
                    if (field.IsLiteral && !field.IsInitOnly) 
                    {
                        constantKeys.Add(field.GetValue(null).ToString());
                    }
                }

                bool hasMismatch = false;
                foreach (string cKey in constantKeys)
                {
                    if (!availableKeys.Contains(cKey))
                    {
                        ModLogger.LogWarning($"[CharacterModifier] 发现无效常量: ModifierKeyConstant 中定义了 '{cKey}'，但当前游戏版本库中不存在该属性。");
                        hasMismatch = true;
                    }
                }
                foreach (string aKey in availableKeys)
                {
                    if (!constantKeys.Contains(aKey))
                    {
                        ModLogger.LogWarning($"[CharacterModifier] 发现缺失定义: 游戏原生支持属性 '{aKey}'，但模组常量类中尚未配置。");
                        hasMismatch = true;
                    }
                }

                if (!hasMismatch)
                {
                    ModLogger.Log("[CharacterModifier] 系统自检完成：所有属性常量校验通过。");
                }
                else
                {
                    StatValidator.LogAllKeys();
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"[CharacterModifier] 自检过程中发生异常: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 通用属性修改入口：自动判断是走 Stat 系统还是反射系统
        /// 优先级：Stat 属性 > AI 字段 > 放弃修改
        /// </summary>
        /// <param name="target">目标角色</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="value">目标数值</param>
        /// <param name="isMultiplier">是否为倍率模式</param>
        /// <param name="source">来源标识</param>
        /// <returns>如果是 Stat 修改，返回 Modifier 对象；如果是 AI 逻辑修改或无效属性，返回 null</returns>
        public static object Modify(CharacterMainControl target, string attributeName, float value, bool isMultiplier, object source = null)
        {
            if (target == null) return null;

            if (StatValidator.IsValid(attributeName))
            {
                ModifierType type = isMultiplier ? ModifierType.PercentageMultiply : ModifierType.Add;
                float finalValue = isMultiplier ? (value - 1f) : value;
                return StatModifier.AddModifier(target, attributeName, finalValue, type, source);
            }

            if (AI_FIELDS.Contains(attributeName) || attributeName.Contains("."))
            {
                AIFieldModifier.ModifyDelayed(target, attributeName, value, isMultiplier);
                return null;
            }

            ModLogger.LogWarning($"[CharacterModifier] 尝试修改无效属性 '{attributeName}'。该属性不在 Stat 列表或 AI 字段定义中，操作已跳过。");
            return null;
        }

        /// <summary>
        /// 统一清理方法
        /// </summary>
        public static void ClearAll(CharacterMainControl target, object source)
        {
            if (target == null || source == null) return;
            StatModifier.RemoveAllModifiersFromSource(target, source);
        }
    }
}