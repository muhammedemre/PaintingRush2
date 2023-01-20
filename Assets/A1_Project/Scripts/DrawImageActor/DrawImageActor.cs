using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawImageActor : MonoBehaviour
{
    public ImagePathOfficer imagePathOfficer;
    int imageCompleteIndex = 0;
    public Transform imagePartsContainer;
    bool pathSteps = true;
    
    public void ActivateNextPathStep() 
    {
        imageCompleteIndex++;

        pathSteps = imageCompleteIndex >= imagePathOfficer.pathContainer.childCount ? false : true;

        if (pathSteps)
        {
            ActivatePath();
        }
        else
        {
            ActivateImagePart();
        }
        
    }

    void ActivatePath() 
    {
        DeactivateAll(imagePathOfficer.pathContainer);
        imagePathOfficer.pathContainer.GetChild(imageCompleteIndex).gameObject.SetActive(true);
    }
    void ActivateImagePart() 
    {
        DeactivateAll(imagePartsContainer);
        imagePartsContainer.GetChild(imageCompleteIndex).gameObject.SetActive(true);
    }

    void DeactivateAll(Transform itemContainer) 
    {
        foreach (Transform item in itemContainer)
        {
            item.gameObject.SetActive(false);
        }
    }
}
