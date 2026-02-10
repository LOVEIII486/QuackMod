using System.Collections.Generic;
using QuackCore.NPC;

namespace QuackItem.NPC
{
    public static class NPCDefinitions
    {
        public static readonly QuackNPCDefinition EliteGuard = new QuackNPCDefinition(
            "Elite_Guard_Carrot", 
            new QuackNPCConfig 
            {
                BasePresetName = "EnemyPreset_Scav",
                CustomName = "金萝卜守护者",
                Health = 600f,
                Team = Teams.player,
                ShowName = true,
                MoveSpeedFactor = 1.25f
            }
        );
        public static readonly QuackNPCDefinition MadQuack = new QuackNPCDefinition(
            "Mad_Quack_Boss",
            new QuackNPCConfig
            {
                BasePresetName = "EnemyPreset_Scav",
                CustomName = "狂暴的小鸭子",
                Health = 1500f,
                MoveSpeedFactor = 1.5f,
                ResistFire = 0.8f
            }
        );
        public static readonly QuackNPCDefinition MimicTearAshes = new QuackNPCDefinition(
            "MimicTearAshes",
            new QuackNPCConfig
            {
                BasePresetName = QuackCore.Constants.NPCPresetNames.Enemies.Raider,
                CustomName = "仿身泪滴",
                Health = 400f,
                Team = Teams.player,
                ShowName = true,
                MoveSpeedFactor = 1.1f,
                DamageMultiplier = 0.8f,
                BulletSpeedMultiplier = 1.2f,
                GunDistanceMultiplier = 1.2f,
                DropBoxOnDead = false,
                GunScatterMultiplier = 0.7f,
                ShootCanMove = true,
                CanDash = true,
                ResistPhysics = 0.8f,
            }
        );

        public static readonly List<QuackNPCDefinition> AllNpcs = new List<QuackNPCDefinition>
        {
            MimicTearAshes
        };
    }
}