using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using Sirenix.Serialization;

public class DataSaveAndLoadOfficer : MonoBehaviour
{
    public bool dataLoaded = false;

    public void SaveTheData()
    {
        PrepareTheDataPackage();
    }
    public void LoadTheData()
    {
        LoadState();
    }


    void PrepareTheDataPackage()
    {
        GameData gameData = new GameData();
        gameData.loaded = true;

        if (Time.time > 5f) // need to be sure game is not saved before its loaded - will be changed with splashScreenDuration
        {
            ES3.Save("gameData", gameData);
        }
    }

    IEnumerator UnpackTheDataPackage()
    {
        yield return new WaitForSeconds(1f); // will be set according to the splashScreenDuration
        if (ES3.KeyExists("gameData"))
        {
            GameData gameData = ES3.Load("gameData", new GameData());
            if (gameData.loaded)
            {
                Debug.Log("Data successfuly Loaded");
                GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.GameStart);
            }
            else
            {
                Debug.Log("GAME DATA couldn't loaded so game is not gonnabe started");
            }
        }
        else // first time game is played so nothing to load yet.
        {
            GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.GameStart);
        }
        
        
    }

    public void LoadState()
    {
        StartCoroutine(UnpackTheDataPackage());
    }

    public void RefreshTheData()
    {
        List<string> keyList = new List<string>() { }; //{ "playerCapacityLevel", "playerSpeedLevel", "playerMoney", "tutorialFinished", "activeRoomsData", "musicGame", "soundGame", "vibrationGame"};
        foreach (string key in keyList)
        {
            ES3.DeleteKey(key);
        }
        
    }
    public void DisplayTheData()
    {
        List<string> keyList = new List<string>() { }; //{ "playerCapacityLevel", "playerSpeedLevel", "playerMoney", "tutorialFinished", "activeRoomsData", "musicGame", "soundGame", "vibrationGame" };
        foreach (string key in keyList)
        {
            print(key+ " : " + ES3.Load(key)) ;
        }
    }

    // Easy Save Version //


    #region Button

    [Title("Refresh The Data")]
    [Button("Refresh The Data", ButtonSizes.Large)]
    void ButtonRefreshTheData()
    {
        RefreshTheData();
    }

    [Title("Display The Data")]
    [Button("Display The Data", ButtonSizes.Large)]
    void ButtonDisplayTheData()
    {
        DisplayTheData();
    }
    #endregion
}
