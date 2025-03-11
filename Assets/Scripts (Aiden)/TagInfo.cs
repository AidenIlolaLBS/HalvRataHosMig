using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagInfo : MonoBehaviour
{
    public string TagName { get; set; }
    public bool Active { get; set; }

    public TagInfo(string tagName, bool active)
    {
        TagName = tagName;
        Active = active;
    }

    public override string ToString()
    {
        return TagName + " " + Active.ToString();
    }
}
