using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Duckov.ItemBuilders;
using Duckov.Utilities;
using FastModdingLib;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using ItemStatsSystem.Stats;
using QuackCore.BuffSystem;
using QuackCore.Constants;

namespace QuackCore.Items
{
    public static class QuackItemFactory
    {
        #region 初始化

        private static Dictionary<ElementTypes, Projectile> _elementBulletCache = new Dictionary<ElementTypes, Projectile>();

        /// <summary>
        /// 初始化工厂
        /// </summary>
        public static void InitializeFactory()
        {
            InitializeBulletCache();
        }

        private static void InitializeBulletCache()
        {
            _elementBulletCache.Clear();

            var bulletSourceMap = new Dictionary<int, ElementTypes>
            {
                { 915, ElementTypes.space },
                { 862, ElementTypes.fire },
                { 1302, ElementTypes.ice },
                { 733, ElementTypes.electricity },
                { 1238, ElementTypes.poison }
            };

            foreach (var kvp in bulletSourceMap)
            {
                int itemId = kvp.Key;
                ElementTypes type = kvp.Value; 
        
                var original = ItemAssetsCollection.GetPrefab(itemId);
                if (original == null) continue;
        
                var gun = original.GetComponent<ItemSetting_Gun>();
                if (gun != null && gun.bulletPfb != null)
                {
                    _elementBulletCache[type] = gun.bulletPfb;
                }
            }

            ModLogger.LogDebug($"[QuackFactory] 子弹库加载完毕，共解析 {_elementBulletCache.Count} 种元素特效。");
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
                AddAdditionalSlots(item, def);
            }
            else
            {
                item = CreateItemFromItemBuilder(modPath, def);
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
            item.name = $"CustomItem_{def.BaseData.itemId}";

            if (def.ResetItemProperties)
            {
                ResetItemProperties(item);
            }

            ItemUtils.SetItemProperties(item, def.BaseData);

            if (!string.IsNullOrEmpty(def.BaseData.spritePath))
                item.Icon = ItemUtils.LoadEmbeddedSprite(modPath, def.BaseData.spritePath, def.BaseData.itemId);

            SetExtendedItemProperties(item, def);

            return item;
        }

        /// <summary>
        /// 使用 ItemBuilder 从零构建物品
        /// </summary>
        private static Item CreateItemFromItemBuilder(string modPath, QuackItemDefinition def)
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
            OverrideItemSettingAccessory(item, def);
            
            // 现应用手动配置的modifier列表
            ApplyCustomModifiers(item, def);
            
            // 如果开发者没有写modifier，而是直接写在PropertyOverrides中，由AutoSetItemProperty自动判断
            if (def.PropertyOverrides != null)
            {
                foreach (var kvp in def.PropertyOverrides)
                {
                    AutoSetItemProperty(item, kvp.Key, kvp.Value);
                }
            }
        }

        private static void OverrideItemSettingGun(Item item, QuackItemDefinition def)
        {
            var gun = item.GetComponent<ItemSetting_Gun>();
            if (gun == null || def.Gun == null) return;
            var gDef = def.Gun;

            if (gDef.TriggerMode.HasValue) gun.triggerMode = gDef.TriggerMode.Value;
            if (gDef.ReloadMode.HasValue) gun.reloadMode = gDef.ReloadMode.Value;
            if (gDef.AutoReload.HasValue) gun.autoReload = gDef.AutoReload.Value;
            if (gDef.CanControlMind.HasValue) gun.CanControlMind = gDef.CanControlMind.Value;
            
            if (!string.IsNullOrEmpty(gDef.ShootKey)) gun.shootKey = gDef.ShootKey;
            if (!string.IsNullOrEmpty(gDef.ReloadKey)) gun.reloadKey = gDef.ReloadKey;

            if (gDef.BuffID.HasValue)
            {
                gun.buff = QuackBuffFactory.GetBuff(gDef.BuffID.Value);
            }

            if (gDef.Element.HasValue)
            {
                gun.element = gDef.Element.Value;
                if (_elementBulletCache.TryGetValue(gDef.Element.Value, out var bullet))
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

            if (mDef.Element.HasValue)
            {
                melee.element = mDef.Element.Value;
            }
        }

        private static void OverrideItemSettingAccessory(Item item, QuackItemDefinition def)
        {
            var acc = item.GetComponent<ItemSetting_Accessory>();
            if (acc == null || def.Accessory == null) return;

            if (def.Accessory.AutoSetMarker)
            {
                acc.SetMarkerParam(item);
            }
        }

        public static void AutoSetItemProperty(Item item, string key, float value)
        {
            // 1. 原生 Stat
            var stat = item.GetStat(key.GetHashCode());
            if (stat != null)
            {
                stat.BaseValue = value;
                return;
            }

            // 2. 是否已存在 Modifier
            var modifiersList = GetModifiersList(item);
            if (modifiersList != null && modifiersList.Any(m => m.Key == key))
            {
                // 此时内部会自动覆盖已有 Modifier 的数值，不会影响修改类型
                ApplyDefaultAddModifier(item, key, value);
                return;
            }

            // 3. 是否为已有的 Constant
            if (item.Constants != null && item.Constants.GetEntry(key) != null)
            {
                item.Constants.SetFloat(key, value);
                return;
            }

            // 4. 是否为已有的 Variable
            if (item.Variables != null && item.Variables.GetEntry(key) != null)
            {
                item.Variables.SetFloat(key, value);
                return;
            }

            // 5. 视为新的 Modifier
            ApplyDefaultAddModifier(item, key, value);
        }

        private static void ApplyDefaultAddModifier(Item item, string key, float value)
        {
            if (item.Modifiers == null) return;

            List<ModifierDescription> modifiersList = GetModifiersList(item);
            if (modifiersList == null) return;

            ModifierDescription targetMod = modifiersList.FirstOrDefault(m => m.Key == key);

            if (targetMod != null)
            {
                // 已存在则更新数值
                SetPrivateField(targetMod, "value", value);
                SetPrivateField(targetMod, "display", true);
            }
            else
            {
                // 不存在则新建
                ModifierTarget target = item.GetComponent<ItemSetting_Accessory>() != null
                    ? ModifierTarget.Parent
                    : ModifierTarget.Character;

                targetMod = new ModifierDescription(target, key, ModifierType.Add, value, false, 0);
                SetPrivateField(targetMod, "display", true);
                modifiersList.Add(targetMod);
            }

            targetMod.ReapplyModifier(item.Modifiers);
        }

        private static void ApplyCustomModifiers(Item item, QuackItemDefinition def)
        {
            if (def.Modifiers == null || def.Modifiers.Count == 0) return;

            List<ModifierDescription> modifiersList = GetModifiersList(item);
            if (modifiersList == null) return;

            foreach (var config in def.Modifiers)
            {
                if (string.IsNullOrEmpty(config.Key)) continue;

                ModifierTarget target;
                if (config.Target.HasValue)
                {
                    target = config.Target.Value;
                }
                else
                {
                    target = item.GetComponent<ItemSetting_Accessory>() != null
                        ? ModifierTarget.Parent
                        : ModifierTarget.Character;
                }

                var modDesc = new ModifierDescription(
                    target, 
                    config.Key, 
                    config.Type,
                    config.Value, 
                    config.OverrideOrder, 
                    config.Order
                );

                SetPrivateField(modDesc, "enableInInventory", config.EnableInInventory);
                SetPrivateField(modDesc, "display", config.Display);
                modifiersList.Add(modDesc);
                modDesc.ReapplyModifier(item.Modifiers);
            }
            
            ModLogger.LogDebug($"[QuackFactory] 已应用 {def.Modifiers.Count} 个自定义详细修改器。");
        }
        
        private static void AddAdditionalSlots(Item item, QuackItemDefinition def)
        {
            if (def.Slots == null || def.Slots.Count == 0) return;

            try
            {
                if (item.gameObject.GetComponent<SlotCollection>() == null)
                {
                    item.CreateSlotsComponent();
                }

                if (def.ReplaceExistingSlots)
                {
                    item.Slots.Clear();
                }

                for (int i = 0; i < def.Slots.Count; i++)
                {
                    var sDef = def.Slots[i];
                    
                    string slotKey = string.Format("{0}_slot_{1}_{2}", item.TypeID, i, sDef.Key);
                    
                    Slot newSlot = new Slot(slotKey);
                    newSlot.Initialize(item.Slots);

                    if (sDef.RequireTags != null)
                    {
                        foreach (var tagStr in sDef.RequireTags)
                        {
                            var tag = ItemUtils.GetTargetTag(tagStr);
                            if (tag != null) newSlot.requireTags.Add(tag);
                        }
                    }

                    if (sDef.ExcludeTags != null)
                    {
                        foreach (var tagStr in sDef.ExcludeTags)
                        {
                            var tag = ItemUtils.GetTargetTag(tagStr);
                            if (tag != null) newSlot.excludeTags.Add(tag);
                        }
                    }

                    item.Slots.Add(newSlot);
                }
                
                ModLogger.LogDebug($"[QuackFactory] 物品 {item.name} 槽位配置完成。模式: {(def.ReplaceExistingSlots ? "替换" : "追加")}");
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"[QuackFactory] 槽位配置失败: {item.TypeID}, {ex.Message}");
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

        public static void ResetItemProperties(Item item)
        {
            List<Stat> statsList = GetStatsList(item);
            if (statsList != null)
            {
                statsList.Clear();
                SetPrivateField(item.Stats, "list", statsList);
            }

            List<ModifierDescription> modifiersList = GetModifiersList(item);
            if (modifiersList != null)
            {
                modifiersList.Clear();
                SetPrivateField(item.Modifiers, "list", modifiersList);
            }
        }

        #endregion

        #region 辅助

        private static List<Stat> GetStatsList(Item item)
        {
            if (item.Stats == null) return null;
            FieldInfo field = typeof(StatCollection).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
            return field?.GetValue(item.Stats) as List<Stat>;
        }

        private static List<ModifierDescription> GetModifiersList(Item item)
        {
            if (item.Modifiers == null) return null;
            FieldInfo field =
                typeof(ModifierDescriptionCollection).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
            return field?.GetValue(item.Modifiers) as List<ModifierDescription>;
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