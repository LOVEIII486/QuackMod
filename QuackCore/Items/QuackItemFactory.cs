using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Duckov.ItemBuilders;
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

        private static Dictionary<string, Projectile> _elementBulletCache = new Dictionary<string, Projectile>();

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

            var bulletSourceMap = new Dictionary<int, string>
            {
                { 915, ItemElementConstants.Space },
                { 862, ItemElementConstants.Fire },
                { 1302, ItemElementConstants.Ice },
                { 733, ItemElementConstants.Electricity },
                { 1238, ItemElementConstants.Poison }
            };

            foreach (var kvp in bulletSourceMap)
            {
                int itemId = kvp.Key;
                string elementKey = kvp.Value;
                var original = ItemAssetsCollection.GetPrefab(itemId);
                if (original == null) continue;
                var gun = original.GetComponent<ItemSetting_Gun>();
                if (gun != null && gun.bulletPfb != null)
                {
                    _elementBulletCache[elementKey] = gun.bulletPfb;
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

            if (!string.IsNullOrEmpty(gDef.TriggerMode))
                gun.triggerMode = (ItemSetting_Gun.TriggerModes)ParseTriggerModes(gDef.TriggerMode);

            if (!string.IsNullOrEmpty(gDef.ReloadMode))
                gun.reloadMode = (ItemSetting_Gun.ReloadModes)ParseReloadModes(gDef.ReloadMode);

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
            var stat = item.GetStat(key.GetHashCode());
            if (stat != null)
            {
                stat.BaseValue = value;
                return;
            }

            ApplySingleModifier(item, key, value);

            if (item.Constants != null) item.Constants.SetFloat(key, value, true);
            if (item.Variables != null) item.Variables.SetFloat(key, value, true);
        }

        private static void ApplySingleModifier(Item item, string key, float value)
        {
            if (item.Modifiers == null) return;

            List<ModifierDescription> modifiersList = GetModifiersList(item);
            if (modifiersList == null) return;

            ModifierDescription targetMod = modifiersList.FirstOrDefault(m => m.Key == key);

            if (targetMod != null)
            {
                // 已存在则更新数值
                SetPrivateField(targetMod, "value", value);
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

        private static void ApplyAccessoryModifiers(Item item, QuackItemDefinition def)
        {
            if (item.Modifiers == null || def.PropertyOverrides == null) return;
            List<ModifierDescription> modifiersList = GetModifiersList(item);
            if (modifiersList == null) return;

            ModifierDescriptionCollection collection = item.Modifiers;

            foreach (var kvp in def.PropertyOverrides)
            {
                ModifierDescription targetMod = null;
                foreach (var mod in modifiersList)
                {
                    if (mod.Key == kvp.Key)
                    {
                        targetMod = mod;
                        break;
                    }
                }

                if (targetMod != null)
                {
                    SetPrivateField(targetMod, "value", kvp.Value);
                    SetPrivateField(targetMod, "display", true);
                }
                else
                {
                    targetMod = new ModifierDescription(
                        ModifierTarget.Parent,
                        kvp.Key,
                        ModifierType.Add,
                        kvp.Value,
                        false,
                        0
                    );
                    SetPrivateField(targetMod, "display", true);
                    modifiersList.Add(targetMod);
                }

                targetMod.ReapplyModifier(collection);
            }

            ModLogger.LogDebug($"[QuackFactory] 已成功应用 {def.PropertyOverrides.Count} 个配件修正器。");
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

        private static int ParseElementIndex(string el)
        {
            return el switch
            {
                "fire" => 1, "poison" => 2, "electricity" => 3, "space" => 4, "ghost" => 5, "ice" => 6, _ => 0
            };
        }

        private static int ParseTriggerModes(string triggerMode)
        {
            return triggerMode switch { "auto" => 0, "semi" => 1, "bolt" => 2, };
        }

        private static int ParseReloadModes(string triggerMode)
        {
            return triggerMode switch { "fullMag" => 0, "singleBullet" => 1 };
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