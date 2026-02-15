using System.Collections.Generic;
using QuackCore.NPC;

namespace QuackItem.NPC
{
    public static class NPCDefinitions
    {
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

        public static readonly QuackNPCDefinition GunTurretBeacon = new QuackNPCDefinition(
            "GunTurretBeacon",
            new QuackNPCConfig
            {
                BasePresetName = QuackCore.Constants.NPCPresetNames.Special.GunTurret,
                CustomName = "自动炮台",
                Team = Teams.player,
                ShowName = true,
            }
        );

        public static readonly List<QuackNPCDefinition> AllNpcs = new List<QuackNPCDefinition>
        {
            MimicTearAshes,
            GunTurretBeacon
        };
    }
}