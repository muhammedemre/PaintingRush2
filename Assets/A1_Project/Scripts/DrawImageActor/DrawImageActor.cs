using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using XDPaint.Controllers;

public class DrawImageActor : MonoBehaviour
{
    public LevelActor levelActor;
    public ImagePathOfficer imagePathOfficer;
    int imageCompleteIndex = -1;
    public Transform imagePartsContainer, outlinesContainer;
    public PathCreator activePath;

    public DrawState currentDrawState;

    public enum DrawState
    {
        Coloring, Outlining, Completed
    }

    public void ActivateNextPathStep() 
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
        DeactivateAll(imagePathOfficer.pathContainer);
        DeactivateAll(outlinesContainer);
        activePath = imagePathOfficer.pathContainer.GetChild(imageCompleteIndex).GetComponent<PathCreator>();
        activePath.gameObject.SetActive(true);

        GameObject drawOutline = outlinesContainer.GetChild(imageCompleteIndex).gameObject;
        levelActor.paintManager.ObjectForPainting = drawOutline;
        drawOutline.SetActive(true);

    }
    void ActivateImagePart() 
    {
        DeactivateAll(imagePartsContainer);
        imagePartsContainer.GetChild(imageCompleteIndex).gameObject.SetActive(true);
    }

    public Vector3 GetActivePathBeginningPosition() 
    {
        PathCreator path = activePath.GetComponent<PathCreator>();
        Vector3 beginningPos = path.path.GetPointAtDistance(0, EndOfPathInstruction.Stop);
        return beginningPos;
    }

    void DeactivateAll(Transform itemContainer) 
    {
        foreach (Transform item in itemContainer)
        {
            item.gameObject.SetActive(false);
        }
    }
}
