using System.Collections.Generic;
using QuackCore.NPC;

namespace QuackItem.NPC
{
    public static class NPCDefinitions
    {
        // 示例：金萝卜护卫配置
        public static readonly QuackNPCDefinition EliteGuard = new QuackNPCDefinition(
            "Elite_Guard_Carrot", 
            new QuackNPCConfig 
            {
                BasePresetName = "EnemyPreset_Scav",
                CustomName = "金萝卜守护者",
                Health = 600f,
                Team = Teams.player, // 设为玩家阵营
                ShowName = true,
                MoveSpeedFactor = 1.25f
            }
        );

        // 示例：疯狂鸭子配置
        public static readonly QuackNPCDefinition MadQuack = new QuackNPCDefinition(
            "Mad_Quack_Boss",
            new QuackNPCConfig
            {
                BasePresetName = "EnemyPreset_Scav",
                CustomName = "狂暴的小鸭子",
                Health = 1500f,
                MoveSpeedFactor = 1.5f,
                ResistFire = 0.8f // 高火抗
            }
        );

        // 汇总列表，用于批量注册
        public static readonly List<QuackNPCDefinition> AllNpcs = new List<QuackNPCDefinition>
        {
            EliteGuard,
            MadQuack
        };
    }
}