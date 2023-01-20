using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager instance;
    public GameManagerObserverOfficer gameManagerObserverOfficer;
    [SerializeField] private float preGameStartDelay;
    public ObserverSubjects currentGameState = (ObserverSubjects)0;

    public string gameName;

    private void Awake()
    {
        SingletonCheck();
        
        gameManagerObserverOfficer.CreateSubjects();
        StartCoroutine(PreGameStartDelay());
    }

    IEnumerator PreGameStartDelay()
    {
        yield return new WaitForSeconds(preGameStartDelay);
        gameManagerObserverOfficer.Publish(ObserverSubjects.PreGameStart);
    }
    

    void SingletonCheck()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }
    
}
