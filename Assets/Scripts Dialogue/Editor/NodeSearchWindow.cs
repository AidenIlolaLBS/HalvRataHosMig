using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Subtegral.DialogueSystem.Editor
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow _window;
        private DialogueGraphView _graphView;

        private Texture2D _indentationIcon;

        public void Configure(EditorWindow window, DialogueGraphView graphView)
        {
            _window = window;
            _graphView = graphView;

            //Transparent 1px indentation icon as a hack
            _indentationIcon = new Texture2D(1, 1);

            _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
                new SearchTreeEntry(new GUIContent("Dialogue Node", _indentationIcon))
                {
                    level = 1, userData = new DialogueNode()
                },
                new SearchTreeEntry(new GUIContent("Info Node", _indentationIcon))
                {
                    level = 1, userData = new InfoNode()
                },
                new SearchTreeEntry(new GUIContent("Choice Node", _indentationIcon))
                {
                    level= 1, userData = new ChoiceNode()
                },
                new SearchTreeEntry(new GUIContent("End Node", _indentationIcon))
                {
                    level=1, userData = new EndNode()
                }
            };

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            //Editor window-based mouse position
            var mousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,
                context.screenMousePosition - _window.position.position);
            var graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePosition);
            switch (SearchTreeEntry.userData)
            {
                case DialogueNode dialogueNode:
                    _graphView.CreateNode("Dialogue Node", graphMousePosition);
                    return true;
                case InfoNode infoNode:
                    _graphView.CreateNode("Info Node", graphMousePosition);
                    return true;
                case ChoiceNode choiceNode:
                    _graphView.CreateNode("Choice Node", graphMousePosition);
                    return true;
                case EndNode endNode:
                    _graphView.CreateNode("End Node", graphMousePosition);
                    return true;
            }
            return false;
        }
    }
}