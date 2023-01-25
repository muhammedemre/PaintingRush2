using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XDPaint;

public class PathFailActor : MonoBehaviour
{
    [SerializeField] GameObject paintManagerPrefab, failCanvasPrefab;
    [SerializeField] PaintManager myPaintManager;
    private void Start()
    {
        GetPrepared();
    }

    void GetPrepared() 
    {
        CreateMyPaintManager();
        CreateCanvasPrefab();
        ChangePaintManagerState(false);
    }
    void CreateMyPaintManager() 
    {
        myPaintManager = Instantiate(paintManagerPrefab, transform).GetComponent<PaintManager>();
    }
    void CreateCanvasPrefab() 
    {
        myPaintManager.ObjectForPainting = Instantiate(failCanvasPrefab, transform);
    }
    public void ChangePaintManagerState(bool state) 
    {
        myPaintManager.gameObject.SetActive(state);
    }
}
