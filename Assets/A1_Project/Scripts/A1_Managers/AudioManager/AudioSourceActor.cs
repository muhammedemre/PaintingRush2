using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceActor : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float audioLength;
    public void PrepareTheAudioSource(AudioSource _audioSource, AudioClip audioClip) 
    {
        audioSource = _audioSource;
        audioSource.clip = audioClip;
        audioLength = audioClip.length;
        StartCoroutine(AudioRunProcess());
    }

    IEnumerator AudioRunProcess() 
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioLength+3);
        Destroy(gameObject);
    }
}
