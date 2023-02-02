using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextPopUpActor : MonoBehaviour
{
    [SerializeField] float travelDistance, travelDuration, scaleUpDuration;
    [SerializeField] List<GameObject> textList = new List<GameObject>();

    private void Start()
    {
        RandomlyOpenAText();
        StartCoroutine(animationStarts());
    }
    
    IEnumerator animationStarts() 
    {
        ScaleUp();
        yield return new WaitForSeconds(scaleUpDuration);
        TravelStart();
        yield return new WaitForSeconds(travelDuration);
        Destroy(gameObject);
    }

    void ScaleUp() 
    {
        transform.DOScale(1, scaleUpDuration).SetEase(Ease.OutElastic);
    }

    void ScaleDown(float scaleDownDuration)
    {
        transform.DOScale(0, scaleDownDuration).SetEase(Ease.OutSine);
    }

    void TravelStart() 
    {
        transform.DOMoveY(travelDistance, travelDuration).SetEase(Ease.OutSine);
        ScaleDown(travelDuration);
    }

    void RandomlyOpenAText() 
    {
        int randomIndex = Random.Range(0,textList.Count);
        textList[randomIndex].gameObject.SetActive(true);
    }

}
