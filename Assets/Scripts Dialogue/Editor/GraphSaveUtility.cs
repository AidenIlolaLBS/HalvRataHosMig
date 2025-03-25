using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;

    [SerializeReference]
    private NodeContainer _containerCache;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<BaseNode> Nodes => _targetGraphView.nodes.ToList().Cast<BaseNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {

        if (!Edges.Any()) return;

        var nodeContainer = ScriptableObject.CreateInstance<NodeContainer>();

        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as BaseNode;
            var inputNode = connectedPorts[i].input.node as BaseNode;

            nodeContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGUID = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGUID = inputNode.GUID
            });
        }

        foreach (BaseNode baseNode in Nodes.Where(node => node.GetType().IsSubclassOf(typeof(BaseNode)) && !node.EntryPoint))
        {
            var baseNodeData = new BaseNodeData
            {
                Guid = baseNode.GUID,
                Position = baseNode.GetPosition().position
            };

            if (baseNode is DialogueNode dialogueNode)
            {
                var data = new DialogueNodeData
                {
                    Guid = dialogueNode.GUID,
                    Position = dialogueNode.GetPosition().position,
                    DialogueText = dialogueNode.DialogueText
                };
                nodeContainer.baseNodeData.Add(data);
            }
            else if (baseNode is ChoiceNode choiceNode)
            {
                var data = new ChoiceNodeData
                {
                    Guid = choiceNode.GUID,
                    Position = choiceNode.GetPosition().position
                };  
                nodeContainer.baseNodeData.Add(data);
            }
            else if (baseNode is InfoNode infoNode)
            {
                var data = new InfoNodeData
                {
                    Guid = infoNode.GUID,
                    nodeTitle = infoNode.nodeTitle,
                    personName = infoNode.personName,
                    emotion = infoNode.emotion,
                    Position = infoNode.GetPosition().position,
                    soundPath = infoNode.soundPath
                };
                nodeContainer.baseNodeData.Add(data);
            }
            else if (baseNode is EndNode endNode)
            {
                var data = new EndNodeData
                {
                    Guid = endNode.GUID,
                    Position = endNode.GetPosition().position
                };
                nodeContainer.baseNodeData.Add(data);
            }
            else
            {
                // För andra typer av noder som ärver från BaseNode men inte är specifika typer.
                nodeContainer.baseNodeData.Add(baseNodeData);
            }
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        AssetDatabase.CreateAsset(nodeContainer, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        _containerCache = Resources.Load<NodeContainer>(fileName);
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK"); //F�rst namn p� flik, sedan vad som ska skrivas ut, sist vad det ska st� p� knappen
            return;
        }
        
        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGUID;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                var outputPort = Nodes[i].outputContainer[j].Q<Port>();
                var inputPort = (Port)targetNode.inputContainer[0];

                // Länka noder
                LinkNodes(outputPort, inputPort);

                // Sätt positionen för targetNode
                var targetNodeData = _containerCache.baseNodeData.First(x => x.Guid == targetNodeGuid);
                targetNode.SetPosition(new Rect(targetNodeData.Position, _targetGraphView.DefultNodeSize));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        if (output == null || input == null)
        {
            Debug.LogError("Output eller input port är null. Kontrollera nodkopplingar.");
            return;
        }

        var tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);

        _targetGraphView.Add(tempEdge);
    }

    private void CreateNodes()
    {
        foreach (var nodeData in _containerCache.baseNodeData)
        {
            BaseNode tempNode = null;          
            if (nodeData is DialogueNodeData dialogueNodeData)
            {
                tempNode = _targetGraphView.CreateDialogueNode(dialogueNodeData.Position, dialogueNodeData.DialogueText);
            }
            else if (nodeData is ChoiceNodeData choiceNodeData)
            {
                tempNode = _targetGraphView.CreateChoiceNode(choiceNodeData.Position);

            }
            else if (nodeData is InfoNodeData infoNodeData)
            {
                tempNode = _targetGraphView.CreateInfoNode(infoNodeData.Position, infoNodeData.emotion, infoNodeData.personName, infoNodeData.soundPath);
            }
            else if (nodeData is EndNodeData endNodeData)
            {
                tempNode = _targetGraphView.CreateEndNode(endNodeData.Position);
            }

            if (tempNode != null)
            {
                tempNode.GUID = nodeData.Guid;
                _targetGraphView.AddElement(tempNode);

                var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGUID == nodeData.Guid).ToList();
                switch (nodeData)
                {
                    case DialogueNodeData:
                        break;
                    case ChoiceNodeData:
                        nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
                        break;
                    case InfoNodeData:
                        break;
                    case EndNodeData:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGUID;

        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);
        }
    }
}