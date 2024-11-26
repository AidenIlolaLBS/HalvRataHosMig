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

    public DefaultAsset soundFolder;

    public List<InfoVariables> variables = new List<InfoVariables>();
}
