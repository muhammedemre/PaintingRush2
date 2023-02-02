using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PathActor : MonoBehaviour
{
    void CleanPathPoints() 
    {
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
    
    [Button("CleanPathPoints")]
    void ButtonCleanPathPoints() 
    {
        CleanPathPoints();
    }
}
