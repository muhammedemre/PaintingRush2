using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePartActor : MonoBehaviour
{
    [SerializeField] GameObject taraliAlan;
    public GameObject paintManger;
    public Color partColor;
    public List<Color> fakeColors = new List<Color>();
    public float acceptedSuccessRate = 50;

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
        paintManger.SetActive(false);
        paintManger.GetComponent<XDPaint.PaintManager>().Init();
        GetComponent<SpriteRenderer>().color = LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().paintController.Brush.Color;
        PaintSuccessCheck();
    }

    public void CleanThePaint() 
    {
        paintManger.GetComponent<XDPaint.PaintManager>().DoDispose();
        paintManger.GetComponent<XDPaint.PaintManager>().Init();
    }

    void PaintSuccessCheck() 
    {
        if (GetComponent<SpriteRenderer>().color == partColor)
        {
            print("PAINT SUCCESS");
        }
        else
        {
            print("PAINT FAIL");
        }
    }
}
