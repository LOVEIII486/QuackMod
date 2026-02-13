using ItemStatsSystem;
using ItemStatsSystem.Stats;

namespace QuackCore.AttributeModifier
{
    /// <summary>
    /// Stat 修改器
    /// </summary>
    public static class StatModifier
    {
        /// <summary>
        /// 为角色添加属性修改器并返回实例
        /// </summary>
        /// <param name="target">目标角色</param>
        /// <param name="statKey">属性 Key</param>
        /// <param name="value">修改数值 (例如 0.5f 代表 +50%)</param>
        /// <param name="type">修改类型 (默认百分比叠加)</param>
        /// <param name="source">来源标识 (用于后续批量移除)</param>
        /// <returns>创建的 Modifier 实例，用于精准移除</returns>
        public static Modifier AddModifier(CharacterMainControl target, string statKey, float value, ModifierType type = ModifierType.PercentageMultiply, object source = null)
        {
            if (target == null || target.CharacterItem == null) return null;

            Stat targetStat = target.CharacterItem.Stats.GetStat(statKey);
            
            if (targetStat == null)
            {
                targetStat = target.GetComponent<StatCollection>()?.GetStat(statKey);
            }

            if (targetStat != null)
            {
                Modifier modifier = new Modifier(type, value, source ?? ("QuackModifier_" + statKey));
                targetStat.AddModifier(modifier);
                
                //暂时不自动回满血
                // if (statKey == Attributes.MaxHealth && target.Health != null)
                // {
                //     target.Health.AddHealth(target.Health.MaxHealth);
                // }

                return modifier;
            }

            ModLogger.LogWarning($"未能在角色 {target.name} 上找到属性 Key: {statKey}");
            return null;
        }
        
        public static void RemoveModifier(CharacterMainControl target, string statKey, Modifier modifier)
        {
            if (target == null || modifier == null) return;

            Stat targetStat = target.CharacterItem?.Stats.GetStat(statKey);
            if (targetStat == null)
            {
                targetStat = target.GetComponent<StatCollection>()?.GetStat(statKey);
            }

            targetStat?.RemoveModifier(modifier);
        }
        
        public static void RemoveAllModifiersFromSource(CharacterMainControl target, object source)
        {
            if (target == null || target.CharacterItem == null || source == null) return;
            target.CharacterItem.RemoveAllModifiersFrom(source);
        }
    }
}