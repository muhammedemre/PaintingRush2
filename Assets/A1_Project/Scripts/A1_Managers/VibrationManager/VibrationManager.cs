using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lofelt.NiceVibrations;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;
    public bool ableToVibrate = true;
    [SerializeField] List<Sprite> vibrationButtonSprites = new List<Sprite>();

    private void Awake()
    {
        SingletonCheck();
        Application.targetFrameRate = 60;
    }

    void SingletonCheck()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void Vibrate(bool success) 
    {
        if (!ableToVibrate)
        {
            return;
        }
        HapticPatterns.PresetType vibrationType = success ? HapticPatterns.PresetType.Success : HapticPatterns.PresetType.Failure;
        HapticPatterns.PlayPreset(vibrationType);
    }

    public void ChangeVibrationState() 
    {
        ableToVibrate = !ableToVibrate;
        SDKManager.instance.SendGameAnalyticsDesignEvent("VibrationState_" + ableToVibrate.ToString());
        HandleStateChange();
    }

    void HandleStateChange() 
    {
        int stateIndex = ableToVibrate ? 0 : 1;
        UIManager.instance.uICanvasOfficer.vibrationButton.GetComponent<Image>().sprite = vibrationButtonSprites[stateIndex];
    }

    public void DataLoadProcess(bool _vibrationState)
    {
        ableToVibrate = _vibrationState;
        HandleStateChange();
    }

}
