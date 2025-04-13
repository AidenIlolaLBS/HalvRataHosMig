using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    public List<GameObject> allPersons = new();
    public List<GameObject> selectedPersons = new();
    int maxSelectedPersons = 3;

    bool haveSelected = false;

    List<GameObject> persons = new();

    private void Start()
    {
        if (!haveSelected)
        {
            SelectPersons();
            haveSelected = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (GameObject person in selectedPersons)
            {
                Debug.Log(person.name + " " + person.GetComponent<Tyckeromdigmätare>().likeLevel);
            }
        }
    }

    public void SelectPersons()
    {
        selectedPersons.Clear();
        System.Random rnd = new();
        List<GameObject> availiblePersons = new();
        foreach (var item in allPersons)
        {
            availiblePersons.Add(item);
        }

        for (int i = 0; i < maxSelectedPersons; i++)
        {
            int selectedIndex = rnd.Next(0, availiblePersons.Count);
            selectedPersons.Add(Instantiate(availiblePersons[selectedIndex]));
            DontDestroyOnLoad(selectedPersons[selectedPersons.Count-1]);
            availiblePersons.RemoveAt(selectedIndex);
        }
        DeactivatePeople();
    }

    public void RemovePerson(Person person)
    {
        for (int i = 0; i < selectedPersons.Count; i++)
        {
            if (selectedPersons[i] == person)
            {
                Debug.Log("person left");
                selectedPersons.RemoveAt(i);
                Debug.Log(selectedPersons.Count);
                return;
            }
        }
    }

    public void SpawnPersons()
    {
        persons = new();
        List<GameObject> spawnPos = GameObject.FindGameObjectsWithTag("PersonSpawnPos").ToList();

        for (int i = 0; i < selectedPersons.Count; i++)
        {
            selectedPersons[i].GetComponent<Person>().Start();
            Debug.Log("serving food");
            selectedPersons[i].GetComponent<Person>().tyckeromdigmätare.ServeFood(gameObject.GetComponent<MealManager>().currentMeal);
        }
        foreach (GameObject go in selectedPersons)
        {
            Debug.Log(go.GetComponent<Person>().tyckeromdigmätare.likeLevel);
        }
        for (int i = 0; i < selectedPersons.Count; i++)
        {
            selectedPersons[i].SetActive(true);
            selectedPersons[i].transform.position = spawnPos[i].transform.position;
            selectedPersons[i].transform.rotation = spawnPos[i].transform.rotation;
            //Instantiate(selectedPersons[i], spawnPos[i].transform.position, spawnPos[i].transform.rotation);
        }
    }

    public void DeactivatePeople()
    {
        foreach (GameObject person in selectedPersons)
        {
            person.SetActive(false);
            person.GetComponent<Person>().haveTalked = 0;
        }
        //for (int i = 0; i < selectedPersons.Count; i++)
        //{
        //    Debug.Log(gameObject.name);
        //    Debug.Log(persons[i].GetComponent<Person>().tyckeromdigmätare.likeLevel);
        //    Debug.Log(selectedPersons[i].GetComponent<Person>().tyckeromdigmätare.likeLevel);
        //    selectedPersons[i].GetComponent<Person>().tyckeromdigmätare.likeLevel = persons[i].GetComponent<Person>().tyckeromdigmätare.likeLevel;
        //    Debug.Log(selectedPersons[i].GetComponent<Person>().tyckeromdigmätare.likeLevel);
        //}
        persons = new();
    }
}
