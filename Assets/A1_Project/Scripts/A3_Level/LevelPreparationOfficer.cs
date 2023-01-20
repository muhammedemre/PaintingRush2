using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreparationOfficer : MonoBehaviour
{
    [SerializeField] LevelActor levelActor;
    [SerializeField] float afterReadyDelay;

    private void Start()
    {
        PrepareTheLevel();
    }

    void PrepareTheLevel()
    {
        AssignTheDrawImage();

        StartCoroutine(LevelIsReadyDelay());
    }

    IEnumerator LevelIsReadyDelay()
    {
        yield return new WaitForSeconds(afterReadyDelay);
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PostLevelInstantiate);
    }

    void AssignTheDrawImage() 
    {
        string imagePath = "ImagePrefabs/" + "LevelImage_" + levelActor.levelIndex.ToString();
        print("imagePath: "+imagePath);
        GameObject drawImagePrefab = Resources.Load<GameObject>(imagePath);
        GameObject tempDrawImage = Instantiate(drawImagePrefab, transform);
        levelActor.drawImageActor = tempDrawImage.GetComponent<DrawImageActor>();

        levelActor.drawImageActor.ActivateNextPathStep();
    }
}
