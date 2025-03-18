using Subtegral.DialogueSystem.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 DefultNodeSize = new Vector2(150, 200);
    private NodeSearchWindow _searchWindow;

    public DialogueGraphView(DialogueGraph editorWindow)
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());

        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(DialogueGraph editorWindow)
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Configure(editorWindow, this);
        nodeCreationRequest = context =>
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        switch (startPort.node.GetType().ToString())
        {
            case "DialogueNode":
                ports.ForEach(port =>
                {
                    if (startPort != port && startPort.node != port.node)
                    {
                        compatiblePorts.Add(port);
                    }
                });
                break;
            case "InfoNode":
                ports.ForEach(port =>
                {
                    if (startPort != port && startPort.node != port.node && port.node.GetType().ToString() != "InfoNode" && port.node.GetType().ToString() != "ChoiceNode" && port.node.GetType().ToString() != "EndNode")
                    {
                        compatiblePorts.Add(port);
                    }
                });
                break;
            case "ChoiceNode":
                ports.ForEach(port =>
                {
                    if (startPort != port && startPort.node != port.node)
                    {
                        compatiblePorts.Add(port);
                    }
                });
                break;
            case "EndNode":
                ports.ForEach(port =>
                {
                    if (startPort != port && startPort.node != port.node)
                    {
                        compatiblePorts.Add(port);
                    }
                });
                break;
            default:
                Debug.LogError("Could not find correct node");
                break;
        }      

        return compatiblePorts;
    }

    private Port GeneratePort(BaseNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }

    public void CreateNode(string nodeName, Vector2 position = new Vector2())
    {
        switch (nodeName)
        {
            case "Dialogue Node":
                AddElement(CreateDialogueNode(position, nodeName));
                break;
            case "Info Node":
                AddElement(CreateInfoNode(position));
                break;
            case "Choice Node":
                AddElement(CreateChoiceNode(position));
                break;
            case "End Node":
                AddElement(CreateEndNode(position));
                break;
            default:
                break;
        }
        
    }

    public DialogueNode CreateDialogueNode(Vector2 position = new Vector2(), string nodeName = "Default Text", string guid = null, string nodeTitle = "Dialogue")
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeTitle,
            DialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };
        if (guid != null)
        {
            dialogueNode.GUID = guid;
        }

        AddInputPort(dialogueNode);

        dialogueNode.AddToClassList("my-node");

        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

        var generatedPort = GeneratePort(dialogueNode, Direction.Output);
        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        oldLabel.text = "Output";

        dialogueNode.outputContainer.Add(generatedPort);       

        var textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
        });
        textField.SetValueWithoutNotify(dialogueNode.DialogueText);
        dialogueNode.mainContainer.Add(textField);
        


        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position, DefultNodeSize));

        return dialogueNode;
    }

    public InfoNode CreateInfoNode(Vector2 position = new Vector2(), string nodeEmotion = "Deafult", string personName = "Person name", string soundPath = null, string nodeTitle = "Info")
    {
        var infoNode = new InfoNode
        {
            title = nodeTitle,
            personName = personName,
            emotion = nodeEmotion,
            GUID = Guid.NewGuid().ToString(),
            soundPath = soundPath
        };
        
        infoNode.AddToClassList("my-node");

        infoNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

        AddInputPort(infoNode);

        LanguagesAvailable languagesAvailable = new LanguagesAvailable();
        foreach (var item in languagesAvailable.languages)
        {
            AddLanguagePort(infoNode, item);
        }
        
        var textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            infoNode.personName = evt.newValue;
        });
        textField.SetValueWithoutNotify(infoNode.personName);
        infoNode.mainContainer.Add(textField);

        var button = new Button(() =>
        {
            VariableContainer(infoNode);         
        });
        //button.text = "New variable";
        //infoNode.titleContainer.Add(button);

        var objectField = new ObjectField("Dialogue Folder")
        {
            objectType = typeof(DefaultAsset),
            allowSceneObjects = false
        };

        objectField.RegisterValueChangedCallback(evt =>
        {
            var selectedObject = evt.newValue as DefaultAsset;
            if (selectedObject != null)
            {
                string path = AssetDatabase.GetAssetPath(selectedObject);
                if (AssetDatabase.IsValidFolder(path))
                {
                    Debug.Log("Folder selected: " + path);
                    infoNode.soundPath = path;
                }
                else
                {
                    objectField.SetValueWithoutNotify(null);
                    infoNode.soundPath = null;
                }
            }
        });
        objectField.SetValueWithoutNotify(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(infoNode.soundPath));
        infoNode.mainContainer.Add(objectField);

        infoNode.RefreshExpandedState();
        infoNode.RefreshPorts();
        infoNode.SetPosition(new Rect(position, DefultNodeSize));

        return infoNode;
    }

    public EndNode CreateEndNode(Vector2 position = new Vector2(), string nodeTitle = "End")
    {
        var endNode = new EndNode
        {
            title = nodeTitle,
            GUID = Guid.NewGuid().ToString()
        };

        AddInputPort(endNode);

        endNode.RefreshExpandedState();
        endNode.RefreshPorts();
        endNode.SetPosition(new Rect(position, DefultNodeSize));

        return endNode; 
    }

    public void VariableContainer(InfoNode infoNode)
    {
        VisualElement variableContainer = new VisualElement();
        InfoVariables values = new InfoVariables();

        infoNode.variables.Add(values);

        var nameField = new TextField
        {
            name = (1 + infoNode.variables.Count).ToString(), 
            value = $"Name of variable"
        };
        values.name = $"Name of variable";
        variableContainer.contentContainer.Add(nameField);       

        nameField.RegisterValueChangedCallback(evt => 
        {
            nameField.value = evt.newValue;
            values.name = evt.newValue;
        });

        var valueField = new TextField
        {
            name = (1 + infoNode.variables.Count).ToString(),
            value = $"Value of variable"
        };
        values.value = $"Value of variable";
        variableContainer.contentContainer.Add(valueField);

        valueField.RegisterValueChangedCallback(evt =>
        {
            valueField.value = evt.newValue;
            values.value = evt.newValue;
        }); 

        var deleteButton = new Button(() =>
        {
            infoNode.contentContainer.Remove(variableContainer);
            infoNode.variables.Remove(values);
        })
        {
            text = "X"
        };
        variableContainer.contentContainer.Add(deleteButton);

        infoNode.contentContainer.Add(variableContainer);
    }

    public ChoiceNode CreateChoiceNode(Vector2 position = new Vector2(), string nodeTitle = "Choice")
    {
        var choiceNode = new ChoiceNode
        {
            title = nodeTitle,
            GUID = Guid.NewGuid().ToString()
        };

        AddInputPort(choiceNode);

        choiceNode.AddToClassList("my-node");

        choiceNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

        var button = new Button(() => { AddChoicePort(choiceNode); });
        button.text = "New Choice";
        choiceNode.titleContainer.Add(button);

        choiceNode.RefreshExpandedState();
        choiceNode.RefreshPorts();
        choiceNode.SetPosition(new Rect(position, DefultNodeSize));

        return choiceNode;
    }

    private void AddInputPort(BaseNode node)
    {
        var inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        node.inputContainer.Add(inputPort);
    }

    public void AddLanguagePort(BaseNode node, string portName)
    {
        var generatedPort = GeneratePort(node, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        oldLabel.text = portName;

        var outputPortCount = node.outputContainer.Query("connector").ToList().Count;
        
        node.outputContainer.Add(generatedPort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    public void AddChoicePort(BaseNode node, string overriddenPortName = "")
    {
        var generatedPort = GeneratePort(node, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        var outputPortCount = node.outputContainer.Query("connector").ToList().Count;

        var choicePortName = string.IsNullOrEmpty(overriddenPortName) ? $"Choice {outputPortCount + 1}" : overriddenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);
        textField.style.minWidth = 60;
        textField.style.maxWidth = 100;
        var deleteButton = new Button(() => RemovePort(node, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);

        generatedPort.portName = choicePortName;
        node.outputContainer.Add(generatedPort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    private void RemovePort(BaseNode dialogueNode, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}