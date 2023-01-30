using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePartActor : MonoBehaviour
{
    [SerializeField] GameObject taraliAlan;
    [SerializeField] GameObject paintManger;
    public Color partColor;
    public List<Color> fakeColors = new List<Color>();

    public void ActivateAndDeactivateTaraliAlan(bool state) 
    {
        taraliAlan.SetActive(state);
    }

    public void ActivateAndDeactivatePaintManager(bool state) 
    {
        paintManger.SetActive(state);
    }

    public void FullyPaint() 
    {
        GetComponent<SpriteRenderer>().color = LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().paintController.Brush.Color;
    }
}
