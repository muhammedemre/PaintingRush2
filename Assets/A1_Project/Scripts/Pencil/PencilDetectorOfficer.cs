using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilDetectorOfficer : MonoBehaviour
{
    [SerializeField] PencilActor pencilActor;
    [SerializeField] CircleCollider2D collider;
    public void SetDetectorSize(float detectorSize) 
    {
        collider.radius = detectorSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GridVisitCheck(other.transform);
    }

    void GridVisitCheck(Transform other)
    {
        string partName = pencilActor.relatedLevelActor.drawImageActor.currentImagePart.name;
        if (other.transform.GetComponent<GridObjectDetector>())
        {
            if (other.GetComponent<GridObjectDetector>().relatedGridObjectActor.transform.name.Contains(partName))
            {
                other.GetComponent<GridObjectDetector>().relatedGridObjectActor.GetComponent<GridObjectActor>().IAmVisited();
            }
        }
    }
}
