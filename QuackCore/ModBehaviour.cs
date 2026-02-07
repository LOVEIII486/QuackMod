using System;
using HarmonyLib;
using QuackCore.Constants;
using UnityEngine.SceneManagement;

namespace QuackCore
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static ModBehaviour Instance { get; private set; }
        
        private Harmony _harmony;
        private bool _isPatched = false;
        private bool _sceneHooksInitialized = false;

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
            InitializeSceneHooks();
        }

        protected override void OnAfterSetup()
        {
            base.OnAfterSetup();
            ModLogger.Log($"{ModConstant.ModName} 游戏数据已准备就绪。");
        }

        private void OnDisable()
        {
            Cleanup();
            ModLogger.Log($"{ModConstant.ModName} 已禁用。");
        }

        private void OnDestroy()
        {
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

        private void InitializeSceneHooks()
        {
            if (_sceneHooksInitialized) return;
            SceneManager.sceneLoaded += OnSceneLoaded;
            _sceneHooksInitialized = true;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ModLogger.Log($"场景载入完成: {scene.name}");
        }

        private void Cleanup()
        {
            if (_isPatched && _harmony != null)
            {
                _harmony.UnpatchAll(ModConstant.ModId);
                _isPatched = false;
            }

            if (_sceneHooksInitialized)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                _sceneHooksInitialized = false;
            }
        }

        #endregion
    }
}