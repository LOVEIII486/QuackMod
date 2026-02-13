using System.Text;

namespace QuackCore.Utils
{
    public static class PlayerStatDebugger
    {
        public static void LogAllPlayerStats()
        {
            var main = CharacterMainControl.Main;
            
            if (main?.CharacterItem?.Stats == null)
            {
                ModLogger.LogWarning("无法获取属性：玩家实例或属性列表尚未加载。");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n--- [QuackCore] 玩家实时属性数值列表 ---");

            foreach (var s in main.CharacterItem.Stats)
            {
                float currentVal = s.Value; 
                
                sb.AppendLine($"[{s.Key}] {s.DisplayName} = {currentVal}");
            }

            sb.AppendLine("---------------------------------------");
            
            ModLogger.Log(sb.ToString());
        }
    }
}