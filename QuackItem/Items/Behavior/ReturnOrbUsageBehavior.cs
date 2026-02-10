using ItemStatsSystem;
using Cysharp.Threading.Tasks;
using Duckov.Utilities;

namespace QuackItem.Items.Behavior
{
    public class ReturnOrbBehavior : ItemStatsSystem.UsageBehavior
    {
        public override DisplaySettingsData DisplaySettings
        {
            get
            {
                var settings = new DisplaySettingsData { display = true, description = "" };
                

                return settings;
            }
        }

        // 不允许在基地使用回归球
        public override bool CanBeUsed(Item item, object user)
        {
            if (LevelManager.Instance == null) return false;
            if (LevelManager.Instance.IsBaseLevel) return false;
            if (LevelManager.Instance.MainCharacter == null) return false;

            return true;
        }

        protected override void OnUse(Item item, object user)
        {
            if (LevelManager.Instance != null && LevelManager.Instance.MainCharacter != null)
            {
                ExecuteEvac().Forget();
            }
            else
            {
                SceneLoader.LoadingComment = "无法使用回归球！";
            }
        }

        private async UniTaskVoid ExecuteEvac()
        {
            await UniTask.Yield();

            SceneLoader.LoadingComment = "使用回归球";
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadBaseScene(GameplayDataSettings.SceneManagement.EvacuateScreenScene, true).Forget();
            }
        }
    }
}