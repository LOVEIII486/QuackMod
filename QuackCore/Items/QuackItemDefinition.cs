using System.Collections.Generic;
using FastModdingLib;
using QuackCore.Constants;

namespace QuackCore.Items
{
    public class QuackItemDefinition
    {
        public int BaseItemId { get; set; }
        public bool ResetItemProperties { get; set; } = false;
            
        public ItemData BaseData { get; set; }

        public ShopConfig Shop { get; set; }

        public CraftingConfig Crafting { get; set; }

        public DecomposeConfig Decompose { get; set; }
        
        public Dictionary<string, float> PropertyOverrides { get; set; } = new Dictionary<string, float>();
        
        public List<SlotConfig> Slots { get; set; }
        
        public GunConfig Gun { get; set; }

        public MeleeConfig Melee { get; set; }
        
        public AccessoryConfig Accessory { get; set; }
        
        public class ShopConfig
        {
            public string MerchantID { get; set; } = MerchantIDs.Normal;
            public int MaxStock { get; set; } = 5;
            public float PriceFactor { get; set; } = 1.0f;
            public float Probability { get; set; } = 1.0f;
            public bool ForceUnlock { get; set; } = true;
        }

        public class CraftingConfig
        {
            public string FormulaID { get; set; }
            public long MoneyCost { get; set; } = 0L;
            public List<(int itemId, long count)> Materials { get; set; } = new List<(int, long)>();
            public int ResultCount { get; set; } = 1;
            public string[] Workbenches { get; set; } = [WorkbenchIDs.WorkBenchAdvanced];
            public string RequirePerk { get; set; } = "";
            public bool UnlockByDefault { get; set; } = true;
            public bool HideInIndex { get; set; } = false;
            public bool LockInDemo { get; set; } = false;
        }

        public class DecomposeConfig
        {
            public long MoneyGain { get; set; } = 0L;
            public List<(int itemId, long count)> Results { get; set; } = new List<(int, long)>();
        }

        public class SlotConfig
        {
            public string Key { get; set; }
            public List<string> RequireTags { get; set; } = new List<string>();
            public List<string> ExcludeTags { get; set; } = new List<string>();
        }

        public class GunConfig
        {
            public string? TriggerMode { get; set; } // 0:auto, 1:semi, 2:bolt
            public string? ReloadMode { get; set; }  // 0:fullMag, 1:singleBullet
            public bool? AutoReload { get; set; }
            public bool? CanControlMind { get; set; }
            public string ShootKey { get; set; }
            public string ReloadKey { get; set; }
            public string Element { get; set; }
            public int? BuffID { get; set; }
        }

        public class MeleeConfig
        {
            public bool? DealExplosionDamage { get; set; }
            public string Element { get; set; }
            public float? BuffChance { get; set; }
            public int? BuffID { get; set; }
        }
        
        public class AccessoryConfig
        {
            public string AdsAimMarkerKey { get; set; }
            public bool AutoSetMarker { get; set; } = true;
        }
    }
}