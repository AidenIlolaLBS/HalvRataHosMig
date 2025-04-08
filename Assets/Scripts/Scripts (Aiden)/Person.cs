using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public Tyckeromdigmätare tyckeromdigmätare;
    string _personName;
    NodeContainer dialogue;

    [SerializeField] private DialogueList[] DialogueList;

    public string PersonName
    {
        get { return _personName; }
    }

    public Person()
    {

    }

    public void Talk()
    {

    }

    private void Start()
    {
        tyckeromdigmätare = new(gameObject.GetComponent<InGameItemTags>());
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(LikeLevel));
        Array.Resize(ref DialogueList, names.Length);
        for (int i = 0; i < DialogueList.Length; i++)
        {
            DialogueList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct DialogueList
{
    [HideInInspector] public string name;
    [SerializeField] private NodeContainer[] dialogueOptions;
    public NodeContainer[] DialogueOptions
    {
        get { return dialogueOptions; }
    }
}
