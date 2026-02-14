using System;
using System.Collections;
using Duckov.Scenes;
using HarmonyLib;
using QuackCore.AttributeModifier;
using QuackCore.BuffSystem;
using QuackCore.Constants;
using QuackCore.NPC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuackCore
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static ModBehaviour Instance { get; private set; }
        
        private Harmony _harmony;
        private bool _isPatched = false;
        private bool _hasSelfChecked = false;

        #region Unity Lifecycle

        private void Awake()
        {
            if (Instance != null) { Destroy(this); return; }
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
            ModLogger.Log($"{ModConstant.ModName} 初始化");
        }

        private void OnEnable()
        {
            if (HarmonyLoader.LoadHarmony(info.path) == null)
            {
                ModLogger.LogError("缺失 Harmony 依赖，模组已禁用。");
                enabled = false;
                return;
            }

            InitializeHarmony();
        }

        protected override void OnAfterSetup()
        {
            base.OnAfterSetup();
            new GameObject("QuackSpawner").AddComponent<QuackSpawner>();
            new GameObject("DelayedBuffManager").AddComponent<DelayedBuffManager>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            ModLogger.Log($"{ModConstant.ModName} 已准备就绪。");
        }

        private void OnDisable()
        {
            Cleanup();
            ModLogger.Log($"{ModConstant.ModName} 已禁用。");
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Base" && !_hasSelfChecked)
            {
                StartCoroutine(WaitAndSelfCheck());
            }
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (Instance == this)
            {
                Cleanup();
                Instance = null;
            }
        }

        #endregion

        #region Core Logic

        private void InitializeHarmony()
        {
            if (_isPatched) return;
            try
            {
                _harmony ??= new Harmony(ModConstant.ModId);
                _harmony.PatchAll(GetType().Assembly);
                _isPatched = true;
                ModLogger.Log("Harmony 补丁注入成功。");
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"Harmony 补丁注入失败: {ex.Message}");
            }
        }
        
        private void Cleanup()
        {
            if (_isPatched && _harmony != null)
            {
                _harmony.UnpatchAll(ModConstant.ModId);
                _isPatched = false;
            }
        }
        
        #endregion
        
        private IEnumerator WaitAndSelfCheck()
        {
            float timer = 0f;
            while (CharacterMainControl.Main == null && timer < 10f)
            {
                timer += Time.deltaTime;
                yield return null; 
            }

            if (CharacterMainControl.Main == null)
            {
                ModLogger.LogError("系统自检失败：超时未找到玩家实例。");
                yield break;
            }

            yield return new WaitForEndOfFrame();
            CharacterModifier.SelfCheck();
            _hasSelfChecked = true; 
        }
    }
}