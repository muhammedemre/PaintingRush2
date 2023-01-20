using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using XDPaint.Core;
using XDPaint.Tools;
using XDPaint.Utils;

public class ColorCheckOfficer : MonoBehaviour
{
    [SerializeField] float colorCheckFrequency, successRate;
    float nextColorCheck;
    [SerializeField] SpriteRenderer selectedSpriteRenderer;
    [SerializeField] Color selectedColor;

    private void Update()
    {
        ColorCheck();
    }

    void ColorCheck() 
    {
        if (nextColorCheck < Time.time)
        {
            print("CHECK");
            nextColorCheck = Time.time + colorCheckFrequency;
            successRate = SuccessRate();           
        }
    }

    float SuccessRate() 
    {
        //var averageColorTexture = new Texture2D(percentRenderTexture.width, percentRenderTexture.height, TextureFormat.ARGB32, false, true);
        //averageColorTexture.ReadPixels(new Rect(0, 0, percentRenderTexture.width, percentRenderTexture.height), 0, 0);
        //averageColorTexture.Apply();
        //RenderTexture.active = prevRenderTextureT;
        //var averageColor = averageColorTexture.GetPixel(0, 0);

        int selectedColorPixelCount = 0;
        Color[] pixels = selectedSpriteRenderer.sprite.texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].Equals(selectedColor))
            {
                selectedColorPixelCount++;
            }
        }
        float percentage = (float)selectedColorPixelCount / pixels.Length * 100;
        return percentage;
    }

    //float SuccessRate() 
    //{
    //    int selectedColorPixelCount = 0;
    //    Color[] pixels = selectedSpriteRenderer.sprite.texture.GetPixels();
    //    for (int i = 0; i < pixels.Length; i++)
    //    {
    //        if (pixels[i].Equals(selectedColor))
    //        {
    //            selectedColorPixelCount++;
    //        }
    //    }
    //    float percentage = (float)selectedColorPixelCount / pixels.Length * 100;
    //    return percentage;
    //}
}
