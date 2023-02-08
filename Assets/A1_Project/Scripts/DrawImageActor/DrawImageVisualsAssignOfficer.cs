using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DrawImageVisualsAssignOfficer : MonoBehaviour
{
    [SerializeField] DrawImageActor relatedDrawImage;
    [SerializeField] int levelIndex = 1;
    [SerializeField] Sprite[] outlineSprites, imagePartSprites;
    [SerializeField] GameObject pathPrefab, imagePart;
    [SerializeField] Shader alphaMaskShader;


    // IMAGEPARTS
    void AssignImageParts() 
    {
        string imagePartPath = "LevelImages/" + "Level" + levelIndex.ToString() + "/ImageParts";
        imagePartSprites = Resources.LoadAll<Sprite>(imagePartPath);

        CleanAllImagePartRelateds();
        AssignPaths();
        AssignFailPaths();

        for (int i = 0; i < imagePartSprites.Length; i++)
        {
            GameObject tempimagePartPrefab = Instantiate(imagePart, relatedDrawImage.imagePartsContainer);
            tempimagePartPrefab.name = "ImagePart_" + i;

            tempimagePartPrefab.GetComponent<SpriteRenderer>().sprite = imagePartSprites[i];
            AssignImageMask(tempimagePartPrefab.GetComponent<SpriteMask>(), imagePartSprites[i]);
            tempimagePartPrefab.AddComponent<PolygonCollider2D>().usedByComposite = true;          
        }

        string previewtPath = "LevelImages/" + "Level" + levelIndex.ToString() + ("/Level"+ levelIndex.ToString() + "Preview");
        print("previewtPath: "+ previewtPath);
        relatedDrawImage.imagePreview = Resources.Load<Sprite>(previewtPath);
    }

    public void AssignMaterials() 
    {
        for (int i = 0; i < relatedDrawImage.imagePartsContainer.childCount; i++)
        {
            Material newMaterial = new Material(alphaMaskShader);
            newMaterial.mainTexture = imagePartSprites[i].texture;
            newMaterial.SetTexture("_MaskTex", imagePartSprites[i].texture);
            Transform imagePart = relatedDrawImage.imagePartsContainer.GetChild(i);
            imagePart.GetComponent<SpriteRenderer>().material = newMaterial;
        }
    }

    void AssignImageMask(SpriteMask spriteMask, Sprite imagePartSprite) 
    {
        spriteMask.sprite = imagePartSprite;
    }


    void AssignPaths() 
    {
        for (int i = 0; i < relatedDrawImage.outlinesContainer.childCount; i++)
        {
            GameObject tempPathPrefab = Instantiate(pathPrefab, relatedDrawImage.imagePathOfficer.pathContainer);
            tempPathPrefab.name = "Path_" + i;
            tempPathPrefab.SetActive(false);
        }
    }

    void AssignFailPaths() 
    {
        for (int i = 0; i < relatedDrawImage.outlinesContainer.childCount; i++)
        {
            GameObject tempPathPrefab = Instantiate(pathPrefab, relatedDrawImage.imagePathOfficer.failPathContainer);
            tempPathPrefab.name = "FailPath_" + i;
            tempPathPrefab.SetActive(false);
        }
    }

    void CleanAllImagePartRelateds() 
    {
        for (int i = relatedDrawImage.imagePartsContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = relatedDrawImage.imagePartsContainer.GetChild(i);
            GameObject.DestroyImmediate(child.gameObject);
        }
        for (int i = relatedDrawImage.imagePathOfficer.pathContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = relatedDrawImage.imagePathOfficer.pathContainer.GetChild(i);
            GameObject.DestroyImmediate(child.gameObject);
        }
        for (int i = relatedDrawImage.imagePathOfficer.failPathContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = relatedDrawImage.imagePathOfficer.failPathContainer.GetChild(i);
            GameObject.DestroyImmediate(child.gameObject);
        }
    }
    // IMAGEPARTS //

    // OUTLINE
    void AssignOutlines() 
    {
        string outlinePath = "LevelImages/" + "Level" + levelIndex.ToString()+ "/ImageOutline";
        outlineSprites = Resources.LoadAll<Sprite>(outlinePath);

        CleanAllOutlines();
        for (int i = 0; i < outlineSprites.Length; i++)
        {
            GameObject tempOutline = new GameObject();
            tempOutline.transform.SetParent(relatedDrawImage.outlinesContainer);
            tempOutline.transform.localPosition = Vector3.zero;
            tempOutline.name = "Outline_" + i;
            tempOutline.AddComponent<SpriteRenderer>().sprite = outlineSprites[i];
            tempOutline.GetComponent<SpriteRenderer>().sortingOrder = 11;
            tempOutline.gameObject.SetActive(false);
        }     
    }

    void CleanAllOutlines() 
    {
        for (int i = relatedDrawImage.outlinesContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = relatedDrawImage.outlinesContainer.GetChild(i);
            GameObject.DestroyImmediate(child.gameObject);
        }
    }
    // OUTLINE //

    [Button("Assign_Outlines")]
    void ButtonAssignOutlines() 
    {
        AssignOutlines();
    }
    [Button("Assign_ImageParts")]
    void ButtonAssignImageParts()
    {
        AssignImageParts();
    }
}
