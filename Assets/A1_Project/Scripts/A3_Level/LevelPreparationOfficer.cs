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
        PrepareTheLandingPage();
    }

    IEnumerator LevelIsReadyDelay()
    {
        yield return new WaitForSeconds(afterReadyDelay);
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PostLevelInstantiate);
    }

    void AssignTheDrawImage() 
    {
        string imagePath = "ImagePrefabs/" + "LevelImage_" + levelActor.levelIndex.ToString();
        //print("imagePath: "+imagePath);
        GameObject drawImagePrefab = Resources.Load<GameObject>(imagePath);
        GameObject tempDrawImage = Instantiate(drawImagePrefab, transform);
        levelActor.drawImageActor = tempDrawImage.GetComponent<DrawImageActor>();
        tempDrawImage.GetComponent<DrawImageActor>().levelActor = levelActor;

        levelActor.drawImageActor.ActivateNextPathAndImagePartStep();

        Vector3 penPosition = levelActor.drawImageActor.GetActivePathBeginningPosition();
        levelActor.pencilActor.pencilDrawProcessOfficer.PositionThePen(penPosition, levelActor.pencilActor.pencilDrawProcessOfficer.positioningDuration, true);
    }

    void PrepareTheLandingPage() 
    {
        UIManager.instance.uICanvasOfficer.levelLandingPageActor.SetLevelPersonImage(0, true); // true means random
        UIManager.instance.uICanvasOfficer.levelLandingPageActor.SetLevelSprite(levelActor.drawImageActor.imagePreview);
        UIManager.instance.uICanvasOfficer.EnableAndDisableLevelLandingPage(true);
    }
}
