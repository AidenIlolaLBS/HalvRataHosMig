using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour, IInteractable
{
    [SerializeField] int SceneIndex;

    public void Interact()
    {
        SceneManager.LoadScene(sceneBuildIndex: SceneIndex);
    }
}