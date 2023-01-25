using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectDetector : MonoBehaviour
{
    public GridObjectActor relatedGridObjectActor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //print("DEGEn: "+relatedGridObjectActor.name + " degdi: "+ other.GetComponent<GridObjectDetector>().relatedGridObjectActor.name);
        //print("DEGEn: " + relatedGridObjectActor.name + " degdi: " + other.name);
        if (other.tag == "Detector" && !relatedGridObjectActor.survived)
        {
            //print("DIGERINE DEGDI me: "+ relatedGridObjectActor.order + " it: "+other.GetComponent<GridObjectDetector>().relatedGridObjectActor.order);
            if (relatedGridObjectActor.order > other.GetComponent<GridObjectDetector>().relatedGridObjectActor.order)
            {
                Destroy(relatedGridObjectActor.gameObject);
            }           
        }

        if(other.gameObject == relatedGridObjectActor.ownerGridActor.gridGroundObject) //(other.tag == relatedGridObjectActor.ownerGridActor.gridGround)
        {
            //print("SURVIVED");
            StartCoroutine(SurviveDelay());
        }
        
    }

    IEnumerator SurviveDelay() 
    {
        yield return new WaitForSeconds(relatedGridObjectActor.ownerGridActor.neighbourTriggerDelay / 4);
        relatedGridObjectActor.survived = true;
    }

}
