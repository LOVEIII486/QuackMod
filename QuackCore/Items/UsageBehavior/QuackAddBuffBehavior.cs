using ItemStatsSystem;
using QuackCore.BuffSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace QuackCore.Items.UsageBehavior
{
    public class QuackAddBuffBehavior : ItemStatsSystem.UsageBehavior
    {
        public int buffId; 
        public float chance = 1f;

        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                var settings = new DisplaySettingsData { display = true, description = "" };
                
                var def = QuackBuffRegistry.Instance.GetDefinition(buffId);
                if (def == null)
                {
                    settings.description = $"ID {buffId} 【Buff 未注册！】";
                    return settings;
                }

                string displayName = def.Config.BuffNameKey.ToPlainText();
                settings.description = displayName;

                if (def.Config.Duration > 0)
                {
                    settings.description += $" : {def.Config.Duration:F1}s";
                }

                if (chance < 1.0f)
                {
                    string chanceLabel = "UI_AddBuffChance".ToPlainText();
                    settings.description += $" ({chanceLabel} : {Mathf.RoundToInt(chance * 100f)}%)";
                }

                return settings;
            }
        }

        public override bool CanBeUsed(Item item, object user) => true;

        protected override void OnUse(Item item, object user)
        {
            if (user is CharacterMainControl character)
            {
                if (Random.Range(0f, 1f) <= chance)
                {
                    QuackBuffFactory.Apply(character, buffId);
                    ModLogger.LogDebug($"[QuackAddBuffBehavior] 物品 {item.name} 触发成功：已施加 ID {buffId}");
                }
                else
                {
                    ModLogger.LogDebug($"[QuackAddBuffBehavior] 物品 {item.name} 触发失败：未通过几率判定 ({chance * 100}%)");
                }
            }
        }
    }
}