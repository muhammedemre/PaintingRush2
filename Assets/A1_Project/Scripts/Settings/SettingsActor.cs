using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsActor : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void PlayOpen()    
    {
        animator.SetInteger("State", 1);
    }

    public void PlayClose() 
    {
        animator.SetInteger("State", 0);
    }
}
