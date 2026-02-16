using System;
using System.Collections.Generic;
using ItemStatsSystem;
using QuackCore.NPC;
using UnityEngine;
using Cysharp.Threading.Tasks;
using QuackItem.Utils;

namespace QuackItem.Items.Behavior
{
    public class MimicTearAshesBehavior : ItemStatsSystem.UsageBehavior
    {
        public string basePresetName;
        public string npcConfigId;
        private readonly ModelReplacer _modelReplacer = new ModelReplacer();

        // 口径到弹药 ID 的保底映射
        private static readonly Dictionary<string, int> CaliberToBulletId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "SMG", 598 },
            { "AR", 607 },
            { "PWS", 1162 },
            { "PWL", 918 },
            { "MAG", 709 },
            { "Candy", 1262 },
            { "Pop", 944 },
            { "SHT", 634 },
            { "BR", 616 },
            { "SNP", 701 },
            { "Rocket", 326 },
            { "GL", 95815 }
        };

        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                string desc = "召唤仿身泪滴的骨灰";
                return new DisplaySettingsData
                {
                    display = true,
                    description = $"{desc}"
                };
            }
        }

        public override bool CanBeUsed(Item item, object user) => true;

        protected override void OnUse(Item item, object user)
        {
            if (user is CharacterMainControl character)
            {
                ExecuteSpawn(character).Forget();
            }
        }

        private async UniTaskVoid ExecuteSpawn(CharacterMainControl user)
        {
            Vector3 spawnPos = user.transform.position + user.transform.forward * 1.5f;
            
            CharacterMainControl mimic = await QuackSpawner.SpawnNPC(npcConfigId, spawnPos);

            if (mimic != null)
            {
                SetupMimicTear(user, mimic);
            }
        }
        
        private void SetupMimicTear(CharacterMainControl player, CharacterMainControl mimic)
        {
            try { _modelReplacer.ApplyModel(mimic, player); }
            catch (Exception ex) { ModLogger.LogWarning($"模型替换失败: {ex.Message}"); }

            try { EquipmentUtils.ReplaceAllEquipment(mimic, player).Forget(); }
            catch (Exception ex) { ModLogger.LogWarning($"装备同步失败: {ex.Message}"); }
        }
    }
}
