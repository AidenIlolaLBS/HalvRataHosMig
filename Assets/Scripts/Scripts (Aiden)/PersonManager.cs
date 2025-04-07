using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    public List<GameObject> allPersons = new();
    List<GameObject> selectedPersons = new();
    int maxSelectedPersons = 3;

    public void SelectPerson()
    {
        System.Random rnd = new();
        List<GameObject> availiblePersons = new();
        foreach (var item in allPersons)
        {
            availiblePersons.Add(item);
        }

        for (int i = 0; i < maxSelectedPersons; i++)
        {
            int selectedIndex = rnd.Next(0, availiblePersons.Count);
            selectedPersons.Add(availiblePersons[selectedIndex]);
            availiblePersons.RemoveAt(selectedIndex);
        }
    }

    public void RemovePerson(Person person)
    {
        for (int i = 0; i < selectedPersons.Count; i++)
        {
            if (selectedPersons[i] == person)
            {
                selectedPersons.RemoveAt(i);
                return;
            }
        }
    }

    public void SpawnPersons()
    {
        List<GameObject> spawnPos = GameObject.FindGameObjectsWithTag("PeopleSpawnPos").ToList();
        for (int i = 0; i < ; i++)
        {

        }
    }
}
