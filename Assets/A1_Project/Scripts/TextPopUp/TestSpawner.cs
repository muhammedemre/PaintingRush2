using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] float destroyDelay;
    [SerializeField] bool destroy;
    void Spawn() 
    {
        GameObject tempObject = Instantiate(prefabToSpawn);
        if (destroy)
        {
            Destroy(tempObject, destroyDelay);
        }        
    }

    [Button("Spawn")]
    void ButtonSpawn() 
    {
        Spawn();
    }
}
