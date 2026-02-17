using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Duckov.Buffs;
using Duckov.ItemBuilders;
using Duckov.Utilities;
using FastModdingLib;
using ItemStatsSystem;
using QuackCore.BuffSystem;

namespace QuackCore.Items
{
    public static class QuackItemFactory
    {
        #region 1. 初始化与静态缓存 (Confirmed)
        
        private static Dictionary<string, Projectile> _elementBulletCache = new Dictionary<string, Projectile>();

        /// <summary>
        /// 初始化工厂：从游戏中抓取全局 Buff 列表和各元素对应的子弹预制体
        /// </summary>
        public static void InitializeFactory()
        {
                CacheElementBullet(915, "space");
                CacheElementBullet(862, "fire");
                CacheElementBullet(1302, "ice");
                CacheElementBullet(733, "electricity");
                CacheElementBullet(1238, "poison");
        }

        private static void CacheElementBullet(int itemId, string elName)
        {
            var gun = ItemAssetsCollection.GetPrefab(itemId)?.GetComponent<ItemSetting_Gun>();
            if (gun != null && gun.bulletPfb != null && !_elementBulletCache.ContainsKey(elName))
                _elementBulletCache.Add(elName, gun.bulletPfb);
        }

        #endregion

        #region 2. 核心创建入口 (Unified)

        /// <summary>
        /// 统一的复杂物品创建函数。
        /// 自动处理：克隆逻辑、槽位构建、近战/远程参数填充。
        /// </summary>
        public static Item CreateComplexItem(string modPath, QuackItemDefinition def)
        {
            Item item;

            // 逻辑分流：有模板 ID 则走克隆流程，否则走 Builder 构建流程
            if (def.BaseItemId > 0)
            {
                item = CreateFromOriginal(modPath, def);
            }
            else
            {
                item = CreateSlotItem(modPath, def);
                // 对于 Builder 构建的物品，补全扩展属性设置
                SetExtendedItemProperties(item, def);
            }

            return item;
        }

        #endregion

        #region 3. 内部构建实现 (Confirmed)

        /// <summary>
        /// 基于原始物品克隆并创建 (Confirmed)
        /// </summary>
        private static Item CreateFromOriginal(string modPath, QuackItemDefinition def)
        {
            Item originalPrefab = ItemAssetsCollection.GetPrefab(def.BaseItemId);
            if (originalPrefab == null) return null;

            Item item = UnityEngine.Object.Instantiate(originalPrefab);
            UnityEngine.Object.DontDestroyOnLoad(item);

            // 强制注入新 ID
            SetPrivateField(item, "typeID", def.BaseData.itemId);

            // 设置基础属性
            ItemUtils.SetItemProperties(item, def.BaseData);

            // 补全图标加载
            if (!string.IsNullOrEmpty(def.BaseData.spritePath))
                item.Icon = ItemUtils.LoadEmbeddedSprite(modPath, def.BaseData.spritePath, def.BaseData.itemId);

            // 调用扩展设置接口
            SetExtendedItemProperties(item, def);

            return item;
        }

        /// <summary>
        /// 使用 ItemBuilder 从零构建物品 (Confirmed)
        /// </summary>
        private static Item CreateSlotItem(string modPath, QuackItemDefinition def)
        {
            var config = def.BaseData;

            ItemBuilder builder = ItemBuilder.New()
                .TypeID(config.itemId)
                .EnableStacking(config.maxStackCount, 1)
                .Icon(ItemUtils.LoadEmbeddedSprite(modPath, config.spritePath, config.itemId));

            // 处理槽位信息
            if (def.Slots != null)
            {
                foreach (var s in def.Slots)
                {
                    var req = s.RequireTags?.Select(ItemUtils.GetTargetTag).ToList();
                    var exc = s.ExcludeTags?.Select(ItemUtils.GetTargetTag).ToList();
                    builder.Slot(s.Key, req, exc);
                }
            }

            config.modifiers.ForEach(m => builder.Modifier(m.getModifier()));

            Item item = builder.Instantiate();
            UnityEngine.Object.DontDestroyOnLoad(item);

            ItemUtils.SetItemProperties(item, config);

            return item;
        }

        #endregion

        #region 4. 扩展属性设置 (Confirmed)

        public static void SetExtendedItemProperties(Item item, QuackItemDefinition def)
        {
            if (item == null || def == null) return;

            ApplyGunProperties(item, def);
            ApplyMeleeProperties(item, def);

            if (def.PropertyOverrides != null)
            {
                foreach (var kvp in def.PropertyOverrides)
                {
                    SetItemNumericalProperty(item, kvp.Key, kvp.Value);
                }
            }
        }

        private static void ApplyGunProperties(Item item, QuackItemDefinition def)
        {
            var gun = item.GetComponent<ItemSetting_Gun>();
            if (gun == null || def.Gun == null) return;

            var gDef = def.Gun;

            if (gDef.BuffID.HasValue)
            {
                gun.buff = QuackBuffFactory.GetBuff(gDef.BuffID.Value);
            }

            if (!string.IsNullOrEmpty(gDef.Element))
            {
                string el = gDef.Element.ToLower();
                gun.element = (ElementTypes)ParseElementIndex(el); // 显式转换解决 CS0266
                
                if (_elementBulletCache.TryGetValue(el, out var bullet))
                    gun.bulletPfb = bullet;
            }

            if (gDef.Damage.HasValue) SetItemNumericalProperty(item, "Damage", gDef.Damage.Value);
            if (gDef.ShootSpeed.HasValue) SetItemNumericalProperty(item, "ShootSpeed", gDef.ShootSpeed.Value);
            if (gDef.ReloadTime.HasValue) SetItemNumericalProperty(item, "ReloadTime", gDef.ReloadTime.Value);
            if (gDef.Capacity.HasValue) SetItemNumericalProperty(item, "Capacity", (float)gDef.Capacity.Value);
        }

        private static void ApplyMeleeProperties(Item item, QuackItemDefinition def)
        {
            var melee = item.GetComponent<ItemSetting_MeleeWeapon>();
            if (melee == null || def.Melee == null) return;

            var mDef = def.Melee;

            if (!string.IsNullOrEmpty(mDef.Element))
                melee.element = (ElementTypes)ParseElementIndex(mDef.Element.ToLower());

            if (mDef.Damage.HasValue) SetItemNumericalProperty(item, "Damage", mDef.Damage.Value);
            if (mDef.AttackRange.HasValue) SetItemNumericalProperty(item, "AttackRange", mDef.AttackRange.Value);
        }

        /// <summary>
        /// 核心数值注入：模拟 ItemFarm 逻辑，修改物品的 Stats/Constants/Variables 系统
        /// </summary>
        public static void SetItemNumericalProperty(Item item, string key, float value)
        {
            var stat = item.GetStat(key.GetHashCode());
            if (stat != null)
            {
                stat.BaseValue = value;
                return;
            }

            if (item.Constants != null) item.Constants.SetFloat(key, value, true);
            if (item.Variables != null) item.Variables.SetFloat(key, value, true);
        }

        #endregion

        #region 5. 辅助工具 (Internal)

        private static int ParseElementIndex(string el)
        {
            return el switch { "fire" => 1, "poison" => 2, "electricity" => 3, "space" => 4, "ghost" => 5, "ice" => 6, _ => 0 };
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            Type type = target.GetType();
            FieldInfo field = null;
            while (field == null && type != null)
            {
                field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                type = type.BaseType;
            }
            field?.SetValue(target, value);
        }

        #endregion
    }
}