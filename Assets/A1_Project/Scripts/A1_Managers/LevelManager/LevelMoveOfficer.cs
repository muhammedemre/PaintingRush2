using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMoveOfficer : MonoBehaviour
{
    public void GoNextLevel()
    {
        if (LevelManager.instance.levelAmount > LevelManager.instance.levelCreateOfficer.LevelCounter)
        {
            //Destroy(LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().paintController);
            Destroy(LevelManager.instance.levelCreateOfficer.currentLevel.gameObject);
            LevelManager.instance.levelCreateOfficer.LevelCounter++;
            LevelManager.instance.levelCreateOfficer.CreateLevelProcess();
        }
        
    }
    public void GoPreviousLevel()
    {
        if (1 < LevelManager.instance.levelCreateOfficer.LevelCounter)
        {
            //Destroy(LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().paintController);
            Destroy(LevelManager.instance.levelCreateOfficer.currentLevel.gameObject);
            LevelManager.instance.levelCreateOfficer.LevelCounter--;
            LevelManager.instance.levelCreateOfficer.CreateLevelProcess();
        }
    }
}
