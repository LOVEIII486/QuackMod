using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using Duckov.Utilities;

namespace QuackCore.NPC
{
    /// <summary>
    /// NPC 生成工具类
    /// </summary>
    public static class QuackSpawner
    {
        #region 核心生成接口

        /// <summary>
        /// 生成 Quack 模组自定义 NPC。
        /// </summary>
        public static async UniTask<CharacterMainControl> SpawnNPC(string quackNPCId, Vector3 position, Teams? team = null)
        {
            var def = QuackNPCRegistry.GetDefinition(quackNPCId);
            if (def == null) return null;

            CharacterRandomPreset template = QuackNPCFactory.GetTemplate(def.ID);

            if (template == null)
            {
                ModLogger.LogError($"[QuackSpawner] 未找到 NPC 模板: {quackNPCId}");
                return null;
            }

            return await ExecuteSpawnAsync(template, position, team);
        }

        /// <summary>
        /// 生成游戏原生 NPC。
        /// </summary>
        public static async UniTask<CharacterMainControl> SpawnVanillaNPC(string assetName, Vector3 position, Teams? team = null)
        {
            var sourcePreset = GetNativePresetByName(assetName);
            if (sourcePreset == null)
            {
                ModLogger.LogError($"[QuackSpawner] 原生预设不存在: {assetName}");
                return null;
            }

            return await ExecuteSpawnAsync(sourcePreset, position, team);
        }

        #endregion

        #region 通用生成逻辑
        
        private static async UniTask<CharacterMainControl> ExecuteSpawnAsync(CharacterRandomPreset preset, Vector3 position, Teams? team)
        {
            if (preset == null) return null;

            Vector3 lookDir = Vector3.forward;
            if (CharacterMainControl.Main != null)
            {
                lookDir = CharacterMainControl.Main.CurrentAimDirection;
                lookDir.y = 0;
                lookDir.Normalize();
            }

            int sceneIndex = MultiSceneCore.MainScene.HasValue
                ? MultiSceneCore.MainScene.Value.buildIndex
                : UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            Teams originalTeam = preset.team;
            if (team.HasValue) preset.team = team.Value;

            try
            {
                CharacterMainControl character = await preset.CreateCharacterAsync(
                    position + Vector3.down * 0.25f, 
                    lookDir,
                    sceneIndex,
                    null,
                    false
                );

                if (character != null)
                {
                    character.SetPosition(position);

                    if (character.movementControl != null)
                        character.movementControl.ForceTurnTo(lookDir);

                    if (character.Team == Teams.player && character.aiCharacterController != null)
                    {
                        character.aiCharacterController.leader = CharacterMainControl.Main;
                        var pet = character.aiCharacterController.GetComponent<PetAI>();
                        if (pet != null) pet.SetMaster(CharacterMainControl.Main);
                    }

                    return character;
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"[QuackSpawner] 生成异常: {ex}");
            }
            finally
            {
                preset.team = originalTeam;
            }

            return null;
        }

        #endregion

        #region 工具函数


        public static CharacterRandomPreset GetNativePresetByName(string name)
        {
            if (GameplayDataSettings.CharacterRandomPresetData == null) return null;
            return GameplayDataSettings.CharacterRandomPresetData.presets
                .FirstOrDefault(p => p != null && p.name == name);
        }

        #endregion
    }
}