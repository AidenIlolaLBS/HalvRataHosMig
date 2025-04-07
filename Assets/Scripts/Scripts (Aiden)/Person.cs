using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    Tyckeromdigmätare tyckeromdigmätare;
    string _personName;
    NodeContainer dialogue;
    

    public string PersonName
    {
        get { return _personName; }
    }

    public Person(int person)
    {
        switch (person)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                break;
        }
    }

    public void Talk()
    {

    }
}
