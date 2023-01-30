using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Manager
{
    public static AudioManager instance;
    [SerializeField] AudioClip backgroundMusic;

    public bool bGmusicState = true, soundFXState = true;
    [SerializeField] List<Sprite> bgMusicButtonSprites = new List<Sprite>();
    [SerializeField] List<Sprite> sfxButtonSprites = new List<Sprite>();
    [SerializeField] AudioSource bgMusicAudioSource;
    [SerializeField] Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
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

    public override void GameStartProcess()
    {
        print("GameStartProcess AudioManager");
        MusicActivateCheck();
    }

    void MusicActivateCheck()
    {
        MusicState();
    }

    public void ChangeMusicState()
    {
        bGmusicState = !bGmusicState;
        MusicState();
    }
    public void ChangeSFXState() 
    {
        soundFXState = !soundFXState;
        SFXState();
    }

    public void MusicState()
    {
        int stateIndex = bGmusicState ? 0 : 1;
        UIManager.instance.uICanvasOfficer.bgMusicButton.GetComponent<Image>().sprite = bgMusicButtonSprites[stateIndex];
        if (bGmusicState)
        {
            bgMusicAudioSource.Play();
        }
        else
        {
            bgMusicAudioSource.Stop();
        }
        //DataManager.instance.DataSaveAndLoadOfficer.SaveTheData();
    }

    public void SFXState() 
    {
        int stateIndex = soundFXState ? 0 : 1;
        UIManager.instance.uICanvasOfficer.soundButton.GetComponent<Image>().sprite = sfxButtonSprites[stateIndex];
    }

    public void PlayASound(string clipName)
    {
        if (!soundFXState)
        {
            return;
        }
        if (clipDictionary.ContainsKey(clipName))
        {
            CreateASound(clipDictionary[clipName], clipName);
        }
        else
        {
            print("no sound object");
        }
    }

    void CreateASound(AudioClip clip, string clipName) 
    {
        GameObject soundObject = new GameObject();
        soundObject.name = clipName + "_SFX";
        soundObject.transform.SetParent(transform);
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        AudioSourceActor audioSourceActor = soundObject.AddComponent<AudioSourceActor>();
        audioSourceActor.PrepareTheAudioSource(audioSource, clip);
    }

    public void DataLoadProcess(bool musicState, bool sfxState) 
    {
        bGmusicState = musicState;
        MusicState();

        soundFXState = sfxState;
        SFXState();
    }
}
