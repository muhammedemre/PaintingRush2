using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPaletteActor : MonoBehaviour
{
    [SerializeField] List<Image> colorPalette = new List<Image>();

    public void AssignColors(List<Color> colors) 
    {     
        for (int i = 0; i < colors.Count; i++)
        {     
            colorPalette[i].color = colors[i];
        }
    }

    public void SetTheBrushColor(int buttonIndex)
    {
        LevelManager.instance.paintController.Brush.SetColor(colorPalette[buttonIndex].GetComponent<Image>().color);
        UIManager.instance.uICanvasOfficer.EnableAndDisableColorPalette(false);
    }
}
