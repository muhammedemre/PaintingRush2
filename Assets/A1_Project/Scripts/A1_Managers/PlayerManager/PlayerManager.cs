using System.Collections;
using UnityEngine;

public class PlayerManager : Manager
{
    public static PlayerManager instance;

    private void Awake()
    {
        SingletonCheck();
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
