using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagInfo : MonoBehaviour
{
    public string tagName { get; set; }
    public bool active { get; set; }

    public TagInfo(string tagName, bool active)
    {
        this.tagName = tagName;
        this.active = active;
    }

    public override string ToString()
    {
        return tagName + " " + active.ToString();
    }
}
