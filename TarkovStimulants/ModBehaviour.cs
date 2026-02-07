using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TarkovStimulants.Constants;
using UnityEngine.SceneManagement;
using FastModdingLib;
using TarkovStimulants.Buffs;
using TarkovStimulants.Items;

namespace TarkovStimulants
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static ModBehaviour Instance { get; private set; }
        
        private string _dllPath => Assembly.GetExecutingAssembly().Location;
        
        private Harmony _harmony;
        private bool _isPatched = false;
        private bool _sceneHooksInitialized = false;

        #region Unity Lifecycle

        private void Awake()
        {
            if (Instance != null) { Destroy(this); return; }
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
            
            if (IsAssemblyLoaded(ModConstant.FmlAssemblyName))
            {
                I18n.InitI18n(_dllPath);
                ModLogger.Log("初始化FML本地化支持");
            }
            else
            {
                ModLogger.LogError("检测到缺失FML前置");
            }
            
            ModLogger.Log($"{ModConstant.ModName} 模组初始化");
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

            ModLogger.Log($"{ModConstant.ModName} 模组已启用。");
        }

        protected override void OnAfterSetup()
        {
            base.OnAfterSetup();
            
            //必须手动加载一次
            I18n.loadFileJson(_dllPath, $"/{I18n.localizedNames[SodaCraft.Localizations.LocalizationManager.CurrentLanguage]}");
            
            RegisterBuffs();
            RegisterItems();
            RegisterQuests();
            RegisterShopGoods();
            RegisterFormulas();
            
            ModLogger.Log($"{ModConstant.ModName} 模组已准备就绪。");
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

        #region 核心

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
            if (string.IsNullOrEmpty(name)) return false;
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.GetName().Name.Contains(name));
        }

        #endregion
        
        #region 注册
        
        private void RegisterBuffs()
        {
            BuffRegistry.RegisterAll();
        }

        private void RegisterItems()
        {
            ItemRegistry.RegisterAll(_dllPath);
        }

        private void RegisterQuests()
        {
            // QuestUtils.RegisterQuest(MyQuests.FirstQuest);
        }

        private void RegisterShopGoods()
        {
            /*
            ShopGoodsData goods = new ShopGoodsData { ... };
            ShopUtils.AddGoods(goods);
            */
        }

        private void RegisterFormulas()
        {
            // CraftingUtils
        }

        #endregion
    }
}