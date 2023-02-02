using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using XDPaint.Controllers;
using XDPaint;

public class DrawImageActor : MonoBehaviour
{
    public LevelActor levelActor;
    public ImagePathOfficer imagePathOfficer;
    public int imageCompleteIndex = -1;
    public Transform imagePartsContainer, outlinesContainer;
    public PathCreator activePath;

    public DrawState currentDrawState;

    [SerializeField] GameObject canvasOutlinePrefab;
    [SerializeField] GameObject currentOutlineCanvas;
    [SerializeField] float brushSizeForPaint;
    public GameObject currentImagePart;
    public PaintManager paintManagerOutline;

    [SerializeField] Transform xPoint, nodePoint;
    public Sprite imagePreview;
    [SerializeField] GameObject confettiPrefab, popUpTextPrefab;

    private void Start()
    {
        paintManagerOutline.ObjectForPainting = currentOutlineCanvas;
        paintManagerOutline.Init();
    }

    public enum DrawState
    {
        NotStarted, Coloring, Outlining, Completed
    }

    public void ActivateNextPathAndImagePartStep() 
    {
        //FullyPaintIfNeeded();
        imageCompleteIndex++;

        currentDrawState = imageCompleteIndex >= imagePathOfficer.pathContainer.childCount ? DrawState.Coloring : DrawState.Outlining;

        levelActor.pencilActor.pencilDrawProcessOfficer.EnableOutlining((currentDrawState == DrawState.Outlining) ? true : false);
        //InputController.Instance.EnableOutlining((currentDrawState == DrawState.Outlining)? true : false);

        if (currentDrawState == DrawState.Outlining)
        {
            InputController.Instance.enabled = false;
            levelActor.pencilActor.BecomePen();
            ActivatePath();
        }
        else if(currentDrawState == DrawState.Coloring)
        {
            InputController.Instance.enabled = true;
            levelActor.pencilActor.BecomeBrush();
            ActivateImagePart();
        }
        
    }

    public void FullyPaintIfNeeded() 
    {
        if (currentDrawState == DrawState.Coloring)
        {
            currentImagePart.GetComponent<ImagePartActor>().FullyPaint();
        }
    }

    public void CleanTheCurrentImagePart() 
    {
        currentImagePart.GetComponent<ImagePartActor>().CleanThePaint();
    }

    void ActivatePath() 
    {
        activePath = imagePathOfficer.pathContainer.GetChild(imageCompleteIndex).GetComponent<PathCreator>();
        levelActor.pencilActor.pencilDrawProcessOfficer.StartNewPath();
        GetActivePathBeginningPosition();
    }

    void ActivateImagePart() 
    {
        int index = imageCompleteIndex - imagePathOfficer.pathContainer.childCount;
        if (index == 0)
        {
            GetReadyForPainting();
        }
        else if (index >= imagePartsContainer.childCount)
        {
            ImageIsCompleted();
            return;
        }
        currentImagePart = imagePartsContainer.GetChild(index).gameObject;
        currentImagePart.transform.GetChild(0).GetComponent<PaintManager>().ObjectForPainting = currentImagePart;
        currentImagePart.transform.GetChild(0).GetComponent<PaintManager>().Init();
        DeactivateAllRequireds();
        currentImagePart.GetComponent<SpriteMask>().enabled = true;
        currentImagePart.GetComponent<ImagePartActor>().ActivateAndDeactivateTaraliAlan(true);
        currentImagePart.GetComponent<ImagePartActor>().ActivateAndDeactivatePaintManager(true);
        AssignThePartColors(currentImagePart.GetComponent<ImagePartActor>());
        UIManager.instance.uICanvasOfficer.EnableAndDisableColorPalette(true);
    }

    void AssignThePartColors(ImagePartActor _currentImagePartActor) 
    {
        List<Color> allColors = new List<Color>();
        allColors.Add(_currentImagePartActor.partColor);
        foreach (Color fakeColor in _currentImagePartActor.fakeColors)
        {
            allColors.Add(fakeColor);
        }
        ShuffleList.Shuffle(allColors);
        UIManager.instance.uICanvasOfficer.colorPaletteActor.AssignColors(allColors);
    }

    void DeactivateAllRequireds() 
    {
        foreach (Transform imagePart in imagePartsContainer)
        {
            imagePart.GetComponent<SpriteMask>().enabled = false;
            imagePart.GetComponent<ImagePartActor>().ActivateAndDeactivateTaraliAlan(false);
            imagePart.GetComponent<ImagePartActor>().ActivateAndDeactivatePaintManager(false);
        }
    }

    void GetReadyForPainting() 
    {
        UIManager.instance.uICanvasOfficer.EnableAndDisableColoringScreen(true);
        DeactivateAll(imagePathOfficer.pathContainer);
        paintManagerOutline.gameObject.SetActive(false);
        foreach (Transform imagePart in imagePartsContainer)
        {
            imagePart.gameObject.SetActive(true);
        }
        levelActor.paintController.Brush.Size = brushSizeForPaint;
    }

    public Vector3 GetActivePathBeginningPosition() 
    {
        PathCreator path = activePath.GetComponent<PathCreator>();
        Vector3 beginningPos = path.path.GetPointAtDistance(0, EndOfPathInstruction.Stop);
        PlaceXPointAtTheBeginningOfThePath(beginningPos);
        return beginningPos;
    }

    public Vector3 GetActivePathEndPosition() 
    {
        PathCreator path = activePath.GetComponent<PathCreator>();
        Vector3 endPos = path.path.GetPointAtDistance(path.path.length, EndOfPathInstruction.Stop);
        return endPos;
    }

    void PlaceXPointAtTheBeginningOfThePath(Vector3 xPos) 
    {
        XPosStateChange(true);
        xPoint.position = xPos;
    }

    public void XPosStateChange(bool state) 
    {
        if (xPoint.gameObject.activeSelf != state)
        {
            xPoint.gameObject.SetActive(state);
        }       
    }

    public IEnumerator NodePointPop() 
    {
        nodePoint.gameObject.SetActive(true);
        GameObject tempConfetti = Instantiate(confettiPrefab, nodePoint.transform.position, Quaternion.identity, levelActor.transform);
        GameObject tempPopUpText = Instantiate(popUpTextPrefab, nodePoint.transform.position, Quaternion.identity, levelActor.transform);
        nodePoint.position = GetActivePathEndPosition();
        yield return new WaitForSeconds(0.1f);
        nodePoint.gameObject.SetActive(false);
    }

    public void DeactivateAll(Transform itemContainer)
    {
        foreach (Transform item in itemContainer)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void CompletedOutlinePart()
    {
        ActivateNextPathAndImagePartStep();
    }

    public void ImageIsCompleted() 
    {
        currentDrawState = DrawState.Completed;
        UIManager.instance.uICanvasOfficer.EnableAndDisableColoringScreen(false);
    }
}

public static class ShuffleList
{
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
