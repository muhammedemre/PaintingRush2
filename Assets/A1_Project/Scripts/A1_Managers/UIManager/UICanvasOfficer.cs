using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasOfficer : MonoBehaviour
{
    public SplashScreenActor splashScreenActor;
    public SettingsActor settingsActor;
    public GameObject inGameScreen, nextButton, bgMusicButton, soundButton, vibrationButton;
    public float splashScreenDuration;
    public ColorPaletteActor colorPaletteActor;
    public GameObject fullyPaintButton, coloringScreen;


    public void DisplaySplashScreen()
    {
        splashScreenActor.SplashProcess(splashScreenDuration, () => AfterSplashScreenProcess());
    }

    void AfterSplashScreenProcess()
    {
        ActivateInGameScreen();
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.LevelInstantiate);

    }

    void ActivateInGameScreen()
    {
        splashScreenActor.gameObject.SetActive(false);
        inGameScreen.SetActive(true);
    }

    public void EnableAndDisableFullyPaintButton(bool state) 
    {
        fullyPaintButton.SetActive(state);
    }

    public void EnableAndDisableColoringScreen(bool state) 
    {
        coloringScreen.SetActive(state);
    }
}
