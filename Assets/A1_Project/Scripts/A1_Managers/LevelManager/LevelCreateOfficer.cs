using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreateOfficer : MonoBehaviour
{
    [SerializeField] private GameObject levelContainer;
    public GameObject currentLevel;

    int levelCounter = 0;

    public void CreateLevelProcess()
    {
        string levelPath = "LevelPrefabs/" + "Level" + levelCounter.ToString();
        GameObject levelPrefab = Resources.Load<GameObject>(levelPath);
        CreateTheLevel(levelPrefab);
    }

    public void CreateTheLevel(GameObject levelPrefab)
    {
        GameObject tempLevel = Instantiate(levelPrefab, levelContainer.transform.position, Quaternion.identity, levelContainer.transform);
        currentLevel = tempLevel;
        currentLevel.GetComponent<LevelActor>().levelIndex = levelCounter;
        //currentLevel.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PostGameStart);

        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PreLevelInstantiate); // Can be arranged according to PostGameStart;
    }
}
