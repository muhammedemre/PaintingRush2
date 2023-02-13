using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Setup;
using GameAnalyticsSDK.Events;

public class SDKManager : MonoBehaviour
{
    public static SDKManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void Start()
    {
        GameAnalytics.Initialize();
    }

    public void SendGameAnalyticsDesignEvent(string eventName) 
    {
        GameAnalytics.NewDesignEvent("eventName");
    }
    public void SendGameAnalyticsProgressionEvent(string eventName, GAProgressionStatus progressionStatus) 
    {
        GameAnalytics.NewProgressionEvent(progressionStatus, eventName);
    }
}
