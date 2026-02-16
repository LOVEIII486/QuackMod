using System.IO;
using UnityEngine;

namespace QuackCore.Utils
{
    public static class SpriteLoader
    {
        public static Sprite LoadSprite(string filePath)
        {
            if (!File.Exists(filePath))
            {
                ModLogger.LogError("图片文件未找到: " + filePath);
                return null;
            }

            try
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData))
                {
                    return Sprite.Create(texture, 
                        new Rect(0, 0, texture.width, texture.height), 
                        new Vector2(0.5f, 0.5f));
                }
            }
            catch (System.Exception)
            {
                ModLogger.LogError("加载图片失败: " + filePath);
            }

            return null;
        }
    }
}