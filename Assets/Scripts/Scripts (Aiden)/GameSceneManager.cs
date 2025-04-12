using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    int sceneIndex = 1;
    int maxGameLoop = 3;
    static int currentGameLoop = 0;

    static public int CurrentGameLoop
    {
        get { return currentGameLoop; }
    }

    bool recentlySwitchedDining = false;

    public void NextScene()
    {
        sceneIndex++;
        if (currentGameLoop < maxGameLoop - 1)
        {
            if (sceneIndex > 2)
            {
                sceneIndex = 1;
                currentGameLoop++;
                gameObject.GetComponent<PersonManager>().DeactivatePeople();
            }
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>().partOfGame = sceneIndex;
        }
        else
        {
            List<GameObject> persons = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PersonManager>().selectedPersons;
            if (persons.Count == 1)
            {
                if (persons[0].GetComponent<Person>().tyckeromdigmätare.likeLevel != LikeLevel.ReallyLikes)
                {
                    sceneIndex++;
                }
            }
        }


        if (sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            sceneIndex = 0;
            currentGameLoop = 0;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>().partOfGame = sceneIndex;
        }

        SceneManager.LoadScene(sceneIndex);
        if (sceneIndex == 2)
        {
            recentlySwitchedDining = true;
        }
    }

    private void Update()
    {
        if (recentlySwitchedDining && SceneManager.GetActiveScene().buildIndex == 2)
        {
            recentlySwitchedDining = false;
            gameObject.GetComponent<PersonManager>().SpawnPersons();
            gameObject.GetComponent<MealManager>().SpawnMeals();
        }
    }
}
