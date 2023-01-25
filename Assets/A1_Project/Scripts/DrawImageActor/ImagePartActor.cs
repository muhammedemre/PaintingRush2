using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePartActor : MonoBehaviour
{
    [SerializeField] GameObject taraliAlan;
    [SerializeField] GameObject paintManger;

    public void ActivateAndDeactivateTaraliAlan(bool state) 
    {
        taraliAlan.SetActive(state);
    }

    public void ActivateAndDeactivatePaintManager(bool state) 
    {
        paintManger.SetActive(state);
    }
}
