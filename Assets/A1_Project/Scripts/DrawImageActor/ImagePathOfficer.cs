using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Sirenix.OdinInspector;

public class ImagePathOfficer : MonoBehaviour
{
    public Transform pathContainer;
    [SerializeField] float spacing, minSpacing;
    [SerializeField] GameObject pathPointPrefab;
    public  Transform failPathContainer;

    void DrawAllPaths()
    {
        foreach (Transform pathObject in pathContainer)
        {
            DrawPathPointsForAPath(pathObject.gameObject);
        }
    }

    void DrawPathPointsForAPath(GameObject pathObject)
    {
        PathCreator pathCreator = pathObject.GetComponent<PathCreator>();
        VertexPath path = pathCreator.path;

        spacing = Mathf.Max(minSpacing, spacing);
        float dst = 0;

        while (dst < path.length)
        {
            Vector3 point = path.GetPointAtDistance(dst);
            //Quaternion rot = path.GetRotationAtDistance(dst);
            Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
            Instantiate(pathPointPrefab, point, rot, pathObject.transform);
            dst += spacing;
        }
    }


    void DeletePaths()
    {
        foreach (Transform path in pathContainer)
        {
            while (path.childCount > 0)
            {
                DestroyImmediate(path.GetChild(0).gameObject);
            }
        }
    }

    #region buttons
    [Button("DrawPaths")]
    void ButtonDrawPaths()
    {
        DrawAllPaths();
    }
    [Button("DeletePaths")]
    void ButtonDeletePaths()
    {
        DeletePaths();

    }
    #endregion
}
