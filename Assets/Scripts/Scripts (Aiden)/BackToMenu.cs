using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void SwitchScene()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSceneManager>().NextScene();
    }
}
