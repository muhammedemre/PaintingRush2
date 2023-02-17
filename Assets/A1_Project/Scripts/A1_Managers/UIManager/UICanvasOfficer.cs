using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICanvasOfficer : MonoBehaviour
{
    public SplashScreenActor splashScreenActor;
    public SettingsActor settingsActor;
    public GameObject inGameScreen, nextButton, bgMusicButton, soundButton, vibrationButton;
    public float splashScreenDuration;
    public ColorPaletteActor colorPaletteActor;
    public GameObject fullyPaintButton, coloringScreen, colorContainer, completeScreen, confettiContainer;
    public TextMeshProUGUI levelCounterLabel;
    public LevelLandingPageActor levelLandingPageActor;


    public void DisplaySplashScreen()
    {
        splashScreenActor.SplashProcess(splashScreenDuration, () => AfterSplashScreenProcess());
    }

    void AfterSplashScreenProcess()
    {
        ActivateInGameScreen();
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.LevelInstantiate);

    }

    public void ActivateInGameScreen()
    {
        splashScreenActor.gameObject.SetActive(false);
        completeScreen.SetActive(false);
        inGameScreen.SetActive(true);
        EnableAndDisableColoringScreen(false);
    }

    public void EnableAndDisableFullyPaintButton(bool state) 
    {
        fullyPaintButton.SetActive(state);
    }

    public void EnableAndDisableColoringScreen(bool state) 
    {
        coloringScreen.SetActive(state);
        EnableAndDisableColorPalette(true);
    }

    public void EnableAndDisableColorPalette(bool state) 
    {
        colorContainer.SetActive(state);
    }

    public void LevelCounterUpdate(int levelIndex)
    {
        levelCounterLabel.text = "LEVEL " + levelIndex.ToString();
    }
    public void EnableAndDisableLevelLandingPage(bool state) 
    {
        levelLandingPageActor.gameObject.SetActive(state);
    }

    public void ActivateCompleteScreen() 
    {
        completeScreen.SetActive(true);
        inGameScreen.SetActive(false);
        EnableAndDisableColoringScreen(false);
    }

}
