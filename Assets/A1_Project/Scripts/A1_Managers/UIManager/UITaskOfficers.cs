﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITaskOfficers : MonoBehaviour
{
    public void SettingsOpen()
    {
        UIManager.instance.uICanvasOfficer.settingsActor.PlayOpen();
    }

    public void SettingsClose()
    {
        UIManager.instance.uICanvasOfficer.settingsActor.PlayClose();
    }

    public void AdminCheatNextLevel(bool next)
    {
        if (next)
        {
            LevelManager.instance.levelMoveOfficer.GoNextLevel();
        }
        else
        {
            LevelManager.instance.levelMoveOfficer.GoPreviousLevel();
        }
        UIManager.instance.uICanvasOfficer.ActivateInGameScreen();
    }

    public void NextButton()
    {
        LevelManager.instance.levelMoveOfficer.GoNextLevel();
        UIManager.instance.uICanvasOfficer.nextButton.SetActive(false);
    }

    public void BGmusicStateChange()
    {
        AudioManager.instance.ChangeMusicState();
        DataManager.instance.DataSaveAndLoadOfficer.SaveTheData();
    }
    public void SFXStateChange()
    {
        AudioManager.instance.ChangeSFXState();
        DataManager.instance.DataSaveAndLoadOfficer.SaveTheData();
    }

    public void VibrationButton()
    {
        VibrationManager.instance.ChangeVibrationState();
        DataManager.instance.DataSaveAndLoadOfficer.SaveTheData();
    }

    public void UndoButton() 
    {
        LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().drawImageActor.CleanTheCurrentImagePart();
        UIManager.instance.uICanvasOfficer.EnableAndDisableColorPalette(true);
        UIManager.instance.uICanvasOfficer.fullyPaintButton.SetActive(false);
    }
    public void CompleteThePaint() 
    {
        SDKManager.instance.SendGameAnalyticsDesignEvent("Undo");
        LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().drawImageActor.FullyPaintIfNeeded();
        LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().drawImageActor.ActivateNextPathAndImagePartStep();
        UIManager.instance.uICanvasOfficer.EnableAndDisableFullyPaintButton(false);
    }

    public void StartButton() 
    {
        UIManager.instance.uICanvasOfficer.EnableAndDisableLevelLandingPage(false);
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PostLevelInstantiate);
    }

    public void ContinueButton() 
    {
        LevelManager.instance.levelMoveOfficer.GoNextLevel();
        UIManager.instance.uICanvasOfficer.ActivateInGameScreen();
    }
}
