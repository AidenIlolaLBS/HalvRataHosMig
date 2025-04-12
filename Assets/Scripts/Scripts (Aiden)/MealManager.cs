using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MealManager : MonoBehaviour
{
    GameObject _CurrentMeal;
    public GameObject currentMeal
    {
        get { return _CurrentMeal; }
        set 
        {
            if (_CurrentMeal != null)
            {
                Destroy(_CurrentMeal);
            }
            _CurrentMeal = value;
            DontDestroyOnLoad(_CurrentMeal);
            _CurrentMeal.SetActive(false);
        }
    }
    List<GameObject> spawnLocations;
    public void SpawnMeals()
    {
        spawnLocations = GameObject.FindGameObjectsWithTag("MealSpawn").ToList();
        currentMeal.SetActive(true);

        Rigidbody rb = currentMeal.GetComponent<Rigidbody>();
        rb.constraints =
            RigidbodyConstraints.FreezePositionX |
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;
        currentMeal.tag = "Untagged";
        foreach (var item in spawnLocations)
        {
            Debug.Log("Spawning meal");
            Instantiate(currentMeal, item.transform.position, item.transform.rotation);
        }
        currentMeal.SetActive(false);
    }
}
