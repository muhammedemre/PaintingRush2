using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PencilActor : MonoBehaviour
{
    public LevelActor relatedLevelActor;
    public ModelOfficer pencilModelOfficer;
    public PencilDrawProcessOfficer pencilDrawProcessOfficer;
    public PencilDetectorOfficer pencilDetectorOfficer;

    public void BecomeBrush() 
    {
        if (pencilModelOfficer.selectedModelIndex != 0)
        {
            pencilModelOfficer.ActivateModel(0);
        }      
    }

    public void BecomePen() 
    {
        if (pencilModelOfficer.selectedModelIndex != 1)
        {
            pencilModelOfficer.ActivateModel(1);
        }          
    }

}
