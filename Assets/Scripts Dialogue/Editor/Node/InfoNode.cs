using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class InfoNode : BaseNode
{
    public string nodeTitle;

    public string personName;

    public string emotion; 

    public string soundPath;

    public List<InfoVariables> variables = new List<InfoVariables>();
}
