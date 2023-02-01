using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelLandingPageActor : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelQuestion;
    [SerializeField] Image levelSprite, levelPerson;
    [SerializeField] List<Sprite> personSpriteList = new List<Sprite>();

    public void SetLevelQuestion(string _levelQuestion) 
    {
        levelQuestion.text = _levelQuestion;
    }

    public void SetLevelPersonImage(int imageIndex, bool random) 
    {
        int randomIndex = Random.Range(0, personSpriteList.Count);
        imageIndex = random ? randomIndex : imageIndex;

        levelPerson.sprite = personSpriteList[imageIndex];
    }

    public void SetLevelSprite(Sprite _levelSprite)
    {
        levelSprite.sprite = _levelSprite;
    }
}
