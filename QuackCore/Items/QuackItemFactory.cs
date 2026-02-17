using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Duckov.ItemBuilders;
using FastModdingLib;
using ItemStatsSystem;
using QuackCore.BuffSystem;

namespace QuackCore.Items
{
    public static class QuackItemFactory
    {
        #region 初始化
        
        private static Dictionary<string, Projectile> _elementBulletCache = new Dictionary<string, Projectile>();

        /// <summary>
        /// 初始化工厂
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

        #region 核心创建入口

        /// <summary>
        /// 统一的复杂物品创建函数
        /// </summary>
        public static Item CreateComplexItem(string modPath, QuackItemDefinition def)
        {
            Item item;

            if (def.BaseItemId > 0)
            {
                item = CreateFromOriginal(modPath, def);
            }
            else
            {
                item = CreateSlotItem(modPath, def);
                SetExtendedItemProperties(item, def);
            }

            return item;
        }

        #endregion

        #region 内部构建

        /// <summary>
        /// 基于原始物品克隆并创建
        /// </summary>
        private static Item CreateFromOriginal(string modPath, QuackItemDefinition def)
        {
            Item originalPrefab = ItemAssetsCollection.GetPrefab(def.BaseItemId);
            if (originalPrefab == null) return null;

            Item item = UnityEngine.Object.Instantiate(originalPrefab);
            UnityEngine.Object.DontDestroyOnLoad(item);

            SetPrivateField(item, "typeID", def.BaseData.itemId);

            ItemUtils.SetItemProperties(item, def.BaseData);

            if (!string.IsNullOrEmpty(def.BaseData.spritePath))
                item.Icon = ItemUtils.LoadEmbeddedSprite(modPath, def.BaseData.spritePath, def.BaseData.itemId);

            SetExtendedItemProperties(item, def);

            return item;
        }

        /// <summary>
        /// 使用 ItemBuilder 从零构建物品
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

        #region 扩展属性设置

        public static void SetExtendedItemProperties(Item item, QuackItemDefinition def)
        {
            if (item == null || def == null) return;

            OverrideItemSettingGun(item, def);
            OverrideItemSettingMeleeWeapon(item, def);

            if (def.PropertyOverrides != null)
            {
                foreach (var kvp in def.PropertyOverrides)
                {
                    SetItemProperty(item, kvp.Key, kvp.Value);
                }
            }
        }

        private static void OverrideItemSettingGun(Item item, QuackItemDefinition def)
        {
            var gun = item.GetComponent<ItemSetting_Gun>();
            if (gun == null || def.Gun == null) return;
            var gDef = def.Gun;

            if (!string.IsNullOrEmpty(gDef.TriggerMode)) gun.triggerMode = (ItemSetting_Gun.TriggerModes)ParseTriggerModes(gDef.TriggerMode);

            if (!string.IsNullOrEmpty(gDef.ReloadMode)) gun.reloadMode = (ItemSetting_Gun.ReloadModes)ParseReloadModes(gDef.ReloadMode);

            if (gDef.AutoReload.HasValue) gun.autoReload = gDef.AutoReload.Value;

            if (gDef.CanControlMind.HasValue) gun.CanControlMind = gDef.CanControlMind.Value;

            if (!string.IsNullOrEmpty(gDef.ShootKey)) gun.shootKey = gDef.ShootKey;

            if (!string.IsNullOrEmpty(gDef.ReloadKey)) gun.reloadKey = gDef.ReloadKey;
    
            if (gDef.BuffID.HasValue)
            {
                gun.buff = QuackBuffFactory.GetBuff(gDef.BuffID.Value);
            }
            
            if (!string.IsNullOrEmpty(gDef.Element))
            {
                string el = gDef.Element.ToLower();
                gun.element = (ElementTypes)ParseElementIndex(el);
                if (_elementBulletCache.TryGetValue(el, out var bullet))
                {
                    gun.bulletPfb = bullet;
                }
            }
        }

        private static void OverrideItemSettingMeleeWeapon(Item item, QuackItemDefinition def)
        {
            var melee = item.GetComponent<ItemSetting_MeleeWeapon>();
            if (melee == null || def.Melee == null) return;

            var mDef = def.Melee;

            if (mDef.DealExplosionDamage.HasValue) melee.dealExplosionDamage = mDef.DealExplosionDamage.Value;

            if (mDef.BuffChance.HasValue) melee.buffChance = mDef.BuffChance.Value;

            if (mDef.BuffID.HasValue)
            {
                melee.buff = QuackBuffFactory.GetBuff(mDef.BuffID.Value);
            }

            if (!string.IsNullOrEmpty(mDef.Element))
            {
                melee.element = (ElementTypes)ParseElementIndex(mDef.Element.ToLower());
            }
        }
        
        public static void SetItemProperty(Item item, string key, float value)
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

        #region 辅助

        private static int ParseElementIndex(string el)
        {
            return el switch { "fire" => 1, "poison" => 2, "electricity" => 3, "space" => 4, "ghost" => 5, "ice" => 6, _ => 0 };
        }

        private static int ParseTriggerModes(string triggerMode)
        {
            return triggerMode switch { "auto" => 0, "semi" => 1, "bolt" => 2, };
        }
        
        private static int ParseReloadModes(string triggerMode)
        {
            return triggerMode switch { "fullMag" => 0, "singleBullet" => 1};
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