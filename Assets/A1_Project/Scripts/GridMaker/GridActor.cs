using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GridActor : MonoBehaviour
{
    public GameObject gridObjectPrefab;
    public Transform gridObjectContainer;
    public float gridSize;
    public float neighbourTriggerDelay;
    public GameObject gridGroundObject;
    public bool createWithMesh;
    [SerializeField] int totalAmountOfGridObjectsAtTheBeginning;
    [SerializeField] float completeRatio = 0;

    private void Start()
    {
        totalAmountOfGridObjectsAtTheBeginning = gridObjectContainer.childCount;
    }

    public void AssignToGridObjectList(GridObjectActor newGridObject) 
    {
        newGridObject.transform.SetParent(gridObjectContainer);
        newGridObject.order = gridObjectContainer.childCount;
        newGridObject.name = gridGroundObject.name+"_" + newGridObject.order;
        
    }

    void CreateGrids()   
    {
        GameObject tempNeighbour = Instantiate(gridObjectPrefab, gridObjectContainer);
        tempNeighbour.GetComponent<GridObjectActor>().AssignAndPrepareGridObjectValues(gridSize, this);
        tempNeighbour.GetComponent<GridObjectActor>().startPoint = true;
        tempNeighbour.name = gridGroundObject.name + "_" + gridObjectContainer.childCount;
    }

    void CleanAllGrids() 
    {
        while (gridObjectContainer.childCount > 0)
        {
            DestroyImmediate(gridObjectContainer.GetChild(0).gameObject);
        }
    }

    public void RemoveMe(GridObjectActor gridObjectToRemove) 
    {
        Destroy(gridObjectToRemove.gameObject);
        CalculateCompleteRatio();
    }

    void CalculateCompleteRatio() 
    {
        completeRatio = (totalAmountOfGridObjectsAtTheBeginning - gridObjectContainer.childCount) / (float)totalAmountOfGridObjectsAtTheBeginning;
        DrawImageActor drawImageActor = LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().drawImageActor;
        if (completeRatio > drawImageActor.acceptedSuccessRate)
        {
            SuccessfulyPainted(drawImageActor);
        }
    }

    void SuccessfulyPainted(DrawImageActor _drawImageActor) 
    {
        FullyPaintTheImagePart();
        _drawImageActor.ActivateNextPathAndImagePartStep();
    }

    void FullyPaintTheImagePart() 
    {
    
    }

    [Button("CraeteGrids")]
    void ButtonCreateGrids() 
    {
        CreateGrids();
    }

    [Button("CleanGrids")]
    void ButtonCleanGrids()
    {
        CleanAllGrids();
    }
}
