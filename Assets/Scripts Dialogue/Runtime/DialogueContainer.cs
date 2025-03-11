using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodeContainer : ScriptableObject
{
    [SerializeReference]
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();

    [SerializeReference]
    public List<BaseNodeData> baseNodeData = new List<BaseNodeData>();

    public NodeContainer() 
    {    
    }

    public NodeContainer(NodeContainer nodeContainer)
    {
        NodeLinks = nodeContainer.NodeLinks;
        baseNodeData = nodeContainer.baseNodeData;
    }
}
