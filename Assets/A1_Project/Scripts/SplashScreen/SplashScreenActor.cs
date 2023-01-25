using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class SplashScreenActor : MonoBehaviour
{
    [SerializeField] Image loadingBarFill;

    public void SplashProcess(float splashScreenDuration, UnityAction onComplete) 
    {
        LoadingBarStart(splashScreenDuration, onComplete);
    }

    void LoadingBarStart(float duration, UnityAction onComplete)
    {
        loadingBarFill.fillAmount = 0f;
        DOTween.To(x =>
        {
            loadingBarFill.fillAmount = x;
        }, 0, 1, duration).SetEase(Ease.OutSine).OnComplete(() => onComplete?.Invoke());
    }
}
