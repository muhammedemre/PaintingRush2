using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GridObjectActor : MonoBehaviour
{
    public GridActor ownerGridActor;
    [SerializeField] Transform neighbourPositions;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] float halfOfTheEdge;
    List<Vector3> cornerList = new List<Vector3>();
    [SerializeField] BoxCollider2D gridCollider;
    [SerializeField] GameObject detector;
    public int order;
    public bool startPoint = false;
    public bool survived = false;

    void PrepareNeighbourPositions() 
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject neighbour = new GameObject();
            neighbour.transform.SetParent(neighbourPositions);
        }

        cornerList.Add(new Vector3(transform.position.x + halfOfTheEdge, transform.position.y + halfOfTheEdge));
        cornerList.Add(new Vector3(transform.position.x + halfOfTheEdge, transform.position.y - halfOfTheEdge));
        cornerList.Add(new Vector3(transform.position.x - halfOfTheEdge, transform.position.y - halfOfTheEdge));
        cornerList.Add(new Vector3(transform.position.x - halfOfTheEdge, transform.position.y + halfOfTheEdge));

        neighbourPositions.GetChild(0).localPosition = new Vector3(halfOfTheEdge*2.1f, 0f, 0f); // 2.1 in order to avoid immediate collisions
        neighbourPositions.GetChild(1).localPosition = new Vector3(0f, -halfOfTheEdge * 2.1f, 0f);
        neighbourPositions.GetChild(2).localPosition = new Vector3(-halfOfTheEdge * 2.1f, 0f, 0f);
        neighbourPositions.GetChild(3).localPosition = new Vector3(0f, halfOfTheEdge * 2.1f, 0f);
    }

    void PrepareTheMesh() 
    {    
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[cornerList.Count];
        for (int i = 0; i< cornerList.Count; i++)
        {
            vertices[i] = cornerList[i];
        }
        mesh.vertices = vertices;

        int[] triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void PaintTheMesh() 
    {
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);
        float alpha = 1f;

        Color newColor = new Color(red, green, blue, alpha);
        meshRenderer.material.color = newColor;
    }

    void PrepareTheCollider()    
    {
        gridCollider = detector.AddComponent<BoxCollider2D>();
        float size = ownerGridActor.gridSize * 0.9f;
        gridCollider.size = new Vector2(size, size);
        gridCollider.isTrigger = true;
    }

    public void AssignAndPrepareGridObjectValues(float gridSize, GridActor owner) 
    {
        transform.localScale = new Vector3(gridSize, gridSize, gridSize);
        ownerGridActor = owner;
        PrepareTheCollider();
        halfOfTheEdge = transform.localScale.x / 2;
        PrepareNeighbourPositions();
        if (owner.createWithMesh)
        {
            PrepareTheMesh();
            PaintTheMesh();
        }
        StartCoroutine(triggerTheNeighbour());
    }

    IEnumerator triggerTheNeighbour() 
    {
        yield return new WaitForSeconds(ownerGridActor.neighbourTriggerDelay/2);
        if (!survived) // to check if on gridground
        {
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(ownerGridActor.neighbourTriggerDelay);

        int neighbourCounter = 0;
        foreach (Transform neighbourPosition in neighbourPositions) 
        {
            GameObject tempNeighbour = Instantiate(ownerGridActor.gridObjectPrefab, ownerGridActor.gridObjectContainer.transform);
            tempNeighbour.GetComponent<GridObjectActor>().AssignAndPrepareGridObjectValues(ownerGridActor.gridSize, ownerGridActor);
            tempNeighbour.transform.position = neighbourPositions.GetChild(neighbourCounter).position;
            ownerGridActor.AssignToGridObjectList(tempNeighbour.GetComponent<GridObjectActor>());
            neighbourCounter++;
        }
    }

    public void IAmVisited() 
    {
        ownerGridActor.RemoveMe(this);
    }

    [Button("I am Visited")]
    void ButtonIAmVisited() 
    {
        IAmVisited();
    }
}
