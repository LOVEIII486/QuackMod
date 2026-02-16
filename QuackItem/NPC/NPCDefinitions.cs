using System.Collections.Generic;
using QuackCore.NPC;
using QuackItem.Constants;

namespace QuackItem.NPC
{
    public static class NPCDefinitions
    {
        public static readonly QuackNPCDefinition MimicTearAshes = new QuackNPCDefinition(
            "MimicTearAshes",
            ModConstant.ModId,
            new QuackNPCConfig
            {
                BasePresetName = QuackCore.Constants.NPCPresetNames.Enemies.Raider,
                CustomPresetName = "MimicTearAshes",
                DisplayNameKey = "NPC_MimicTearAshes",
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
            ModConstant.ModId,
            new QuackNPCConfig
            {
                BasePresetName = QuackCore.Constants.NPCPresetNames.Special.GunTurret,
                CustomPresetName = "GunTurretBeacon",
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