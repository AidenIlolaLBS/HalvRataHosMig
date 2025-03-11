using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;
using static UnityEditor.LightingExplorerTableColumn;

[Serializable]
public class InfoNodeData : BaseNodeData
{
    public string nodeTitle;

    public string personName;

    public string emotion;

    public DefaultAsset soundFolder;

    public List<InfoVariables> variables = new List<InfoVariables>();
}
