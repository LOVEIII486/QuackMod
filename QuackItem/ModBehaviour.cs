using System;
using System.Linq;
using Duckov.Buffs;
using HarmonyLib;
using QuackCore.BuffSystem;
using QuackCore.BuffSystem.Effects;
using QuackCore.Constants;
using QuackItem.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuackItem
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

            if (!string.IsNullOrEmpty(ModConstant.FmlAssemblyName) && !IsAssemblyLoaded(ModConstant.FmlAssemblyName))
            {
                ModLogger.LogWarning($"未检测到 {ModConstant.FmlAssemblyName}，部分功能可能受限。");
            }

            InitializeHarmony();
            InitializeSceneHooks();
        }

        protected override void OnAfterSetup()
        {
            base.OnAfterSetup();
            
            var config = new QuackBuffFactory.BuffConfig("QuackItem", "Berserk", 10f,"狂暴", "反应时间减半，移动速度提升。");

            var berserkDef = new QuackBuffDefinition(config)
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.WalkSpeed, 0.5f, true))
                .AddEffect(new AttributeModifierEffect(ModifierKeyConstant.Stat.RunSpeed, 0.5f, true));

            QuackBuffRegistry.Instance.Register(berserkDef);
            
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
        
        private void Update()
        {
            // 检测 F5 按键按下
            if (Input.GetKeyDown(KeyCode.F5))
            {
                TestApplyBuff();
            }
        }
        
        private void TestApplyBuff()
        {
            // 1. 获取玩家对象
            // 注意：这里需要根据游戏的实际 API 获取玩家的 CharacterMainControl
            // 常见写法可能是 PlayerControl.Instance 或类似的单例
            var player = CharacterMainControl.Main; 

            if (player == null)
            {
                ModLogger.LogWarning("未找到玩家对象，无法添加 Buff");
                return;
            }

            QuackBuffFactory.Apply(player, "QuackItem_Berserk");
            ModLogger.Log("[Test] F5 指令：已请求施加自定义文字 Buff");
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

        private bool IsAssemblyLoaded(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.GetName().Name.Contains(name));
        }

        #endregion
    }
}