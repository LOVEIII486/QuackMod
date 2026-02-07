using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using QuackItem.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using FastModdingLib;
using QuackItem.Buffs;
using QuackItem.Items;
using SodaCraft.Localizations;

namespace QuackItem
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        public static ModBehaviour Instance { get; private set; }
        
        private string _dllPath => Assembly.GetExecutingAssembly().Location;
        
        private Harmony _harmony;
        private bool _isPatched = false;
        private bool _sceneHooksInitialized = false;
        private bool _isI18nInitialized = false;

        #region Unity Lifecycle

        private void Awake()
        {
            if (Instance != null) { Destroy(this); return; }
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
            
            if (IsAssemblyLoaded(ModConstant.FmlAssemblyName))
            {
                I18n.InitI18n(_dllPath);
            }
            else
            {
                ModLogger.Log("检测到缺失 FML，跳过本地化初始化。");
            }
            
            ModLogger.Log($"{ModConstant.ModName} 模组初始化中...");
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

            RegisterItems();
            RegisterQuests();
            RegisterShopGoods();
            RegisterFormulas();
            
            BuffRegistry.RegisterAll();
            
            ItemUtils.CreateCustomItem(_dllPath, QuackItems.Cookie, "QuackItem");
            
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
            // 清理 Harmony
            if (_isPatched && _harmony != null)
            {
                _harmony.UnpatchAll(ModConstant.ModId);
                _isPatched = false;
            }

            // 清理场景钩子
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
        
        #region Content Registration

        private void RegisterItems()
        {
            //ItemUtils.RegisterGun(bundle, "PrefabName", ItemID, ModID);
        }

        private void RegisterQuests()
        {
            // 使用 QuestUtils 注册任务
            // QuestUtils.RegisterQuest(MyQuests.FirstQuest);
        }

        private void RegisterShopGoods()
        {
            // 使用 ShopUtils 添加商店物品
            /*
            ShopGoodsData goods = new ShopGoodsData { ... };
            ShopUtils.AddGoods(goods);
            */
        }

        private void RegisterFormulas()
        {
            // 使用 CraftingUtils 注册合成表
        }

        #endregion
    }
}