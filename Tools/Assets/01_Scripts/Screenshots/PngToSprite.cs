using System.IO;
using UnityEngine;

public static class PngToSprite
{
    public static Sprite ConvertToSprite(int _width, int _height, string _pngPath)
    {
        byte[] pngData = File.ReadAllBytes(_pngPath);
        Texture2D screenshotTexture = new Texture2D(_width, _height);
        if (screenshotTexture.LoadImage(pngData))
        {
            return Sprite.Create(screenshotTexture, 
                                    new Rect(0, 0, screenshotTexture.width, screenshotTexture.height), 
                                    new Vector2(0, 0));
        }

        Debug.Log("Failed to create sprite");
        return null;
    }
}
