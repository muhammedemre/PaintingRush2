using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ModelOfficer : MonoBehaviour
{
    public Transform modelContainer;
    public int selectedModelIndex;

    public void ActivateModel(int modelIndex) 
    {
        selectedModelIndex = modelIndex;
        DeactivateAll(modelContainer);
        modelContainer.GetChild(modelIndex).gameObject.SetActive(true);
    }

    void DeactivateAll(Transform itemContainer)
    {
        foreach (Transform item in itemContainer)
        {
            item.gameObject.SetActive(false);
        }
    }

    [Button("ActivateModel")]
    void ButtonModelActivate(int modelIndex) 
    {
        ActivateModel(modelIndex);
    }
}
