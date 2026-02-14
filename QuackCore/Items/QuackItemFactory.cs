using System.Linq;
using Duckov.ItemBuilders;
using FastModdingLib;
using ItemStatsSystem;

namespace QuackCore.Items;

public static class QuackItemFactory 
{
    public static Item CreateComplexItem(string modPath, QuackItemDefinition def)
    {
        var config = def.BaseData;
        
        ItemBuilder builder = ItemBuilder.New()
            .TypeID(config.itemId)
            .EnableStacking(config.maxStackCount, 1)
            .Icon(ItemUtils.LoadEmbeddedSprite(modPath, config.spritePath, config.itemId));

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
}