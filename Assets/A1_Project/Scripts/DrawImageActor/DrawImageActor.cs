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
    public float acceptedSuccessRate;
    public PaintManager paintManagerOutline;

    private void Start()
    {
        paintManagerOutline.ObjectForPainting = currentOutlineCanvas;
        paintManagerOutline.Init();
    }

    public enum DrawState
    {
        Coloring, Outlining, Completed
    }

    public void ActivateNextPathAndImagePartStep() 
    {
        imageCompleteIndex++;

        currentDrawState = imageCompleteIndex >= imagePathOfficer.pathContainer.childCount ? DrawState.Coloring : DrawState.Outlining;
        if (currentDrawState == DrawState.Outlining)
        {
            InputController.Instance.EnableOutlining(true);
        }
        else
        {
            InputController.Instance.EnableOutlining(false);
        }

        if (currentDrawState == DrawState.Outlining)
        {
            ActivatePath();
        }
        else if(currentDrawState == DrawState.Coloring)
        {
            ActivateImagePart();
        }
        
    }

    void ActivatePath() 
    {
        //DeactivateAll(imagePathOfficer.pathContainer);
        activePath = imagePathOfficer.pathContainer.GetChild(imageCompleteIndex).GetComponent<PathCreator>();
        //activePath.gameObject.SetActive(true);
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
        //levelActor.paintManagerPaint.gameObject.SetActive(true);
        currentImagePart = imagePartsContainer.GetChild(index).gameObject;
        currentImagePart.transform.GetChild(0).GetComponent<PaintManager>().ObjectForPainting = currentImagePart;
        currentImagePart.transform.GetChild(0).GetComponent<PaintManager>().Init();
    }

    void GetReadyForPainting() 
    {
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
        return beginningPos;
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

}
