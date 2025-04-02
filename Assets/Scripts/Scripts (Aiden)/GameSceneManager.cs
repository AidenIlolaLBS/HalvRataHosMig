using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    int sceneIndex = 0;
    int gameLoop = 3;
    int currentGameLoop = 0;

    public void NextScene()
    {
        sceneIndex++;
        if (currentGameLoop < gameLoop - 1)
        {
            if (sceneIndex > 2)
            {
                sceneIndex = 1;
            }
        }
        if (sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            sceneIndex = 0;
            currentGameLoop = 0;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }
}
