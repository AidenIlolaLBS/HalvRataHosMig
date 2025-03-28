using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    List<Person> allPersons = new() { };
    List<Person> selectedPersons = new();
    int maxSelectedPersons = 3;

    public void SelectPerson()
    {
        System.Random rnd = new();
        List<Person> availiblePersons = new();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
