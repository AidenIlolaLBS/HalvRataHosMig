using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Subtegral.DialogueSystem.Runtime
{
    public class DialogueParser : MonoBehaviour
    {
        [SerializeField] private NodeContainer nodeContainer;
        private TextMeshProUGUI dialogueText;
        private TextMeshProUGUI dialogueTitle;
        private List<GameObject> dialogueBox = new List<GameObject>();
        [SerializeField] private GameObject oneChoicePrefab;
        [SerializeField] private GameObject twoChoicePrefab;
        public GameObject player;
        string title = "";
        string text = "";
        string currentDisplayText = "";
        IEnumerable<NodeLinkData> choices;
        float timer = 0;
        private int charindex = 0;
        private float maxTime = 3;
        private float timePerChar = 0.15f;
        bool allTextLaoded = false;

        public GameObject playerCam;
        public MoveCamera moveCamera;

        string audioPath; 
        private AudioManager audioManager;

        GameObject person;

        private void Start()
        {
            audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();

            //ExternalStartNarrative();
        }

        private void Update()
        {           
            if (!allTextLaoded)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Debug.Log(timer);
                    timer += timePerChar;
                    if (charindex < text.Count())
                    {
                        currentDisplayText += text[charindex];
                        dialogueText.text = currentDisplayText;
                        charindex++;
                    }
                    else
                    {
                        allTextLaoded = true;
                        charindex = 0;
                        currentDisplayText = "";
                        timer = 0;
                    }
                }
            }
        }

        public void ExternalStartNarrative()
        {
            person = null;
            var narrativeData = nodeContainer.NodeLinks.First(); //Entrypoint node
            //player.GetComponent<PlayerMovement>().enabled = false;
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        public void ExternalStartNarrative(NodeContainer nodeContainer)
        {
            person = null;
            var narrativeData = (this.nodeContainer = nodeContainer).NodeLinks.First(); //Entrypoint node
            //player.GetComponent<PlayerMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            moveCamera.enabled = false;
            playerCam.GetComponent<Interact>().enabled = false;
            playerCam.GetComponent<PickUpScript>().enabled = false;
            playerCam.GetComponent<PlayerCam>().enabled = false;
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        public void ExternalStartNarrative(NodeContainer nodeContainer, GameObject gameObject)
        {
            person = gameObject;
            var narrativeData = (this.nodeContainer = nodeContainer).NodeLinks.First(); //Entrypoint node
            //player.GetComponent<PlayerMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            moveCamera.enabled = false;
            playerCam.GetComponent<Interact>().enabled = false;
            playerCam.GetComponent<PickUpScript>().enabled = false;
            playerCam.GetComponent<PlayerCam>().enabled = false;
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        private void ProceedToNarrative(string startNarrativeDataGUID)
        {
            allTextLaoded = false;
            if (dialogueBox.ToArray().Length > 0)
            {
                RemoveDialogueBox();
            }
            choices = null;
            string tempNarrativeDataguid = startNarrativeDataGUID;

            BaseNodeData nodeType;

            nodeType = nodeContainer.baseNodeData.Find(x => x.Guid == tempNarrativeDataguid) as EndNodeData;
            if (nodeType != null)
            {
                if (person != null)
                {
                    person.GetComponent<Person>().haveTalked++;
                    if (int.Parse(((EndNodeData)nodeType).endNum) == 1)
                    {
                        person.GetComponent<Person>().tyckeromdigmätare.IncreaseLikeMeter();
                    }
                    else if (int.Parse(((EndNodeData)nodeType).endNum) == -1)
                    {
                        person.GetComponent<Person>().tyckeromdigmätare.DecreaseLikeMeter();
                    }
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                moveCamera.enabled = true;
                playerCam.GetComponent<Interact>().enabled = true;
                playerCam.GetComponent<PickUpScript>().enabled = true;
                playerCam.GetComponent<PlayerCam>().enabled = true;
                return;
            }

            nodeType = nodeContainer.baseNodeData.Find(x => x.Guid == tempNarrativeDataguid) as InfoNodeData;
            if (nodeType != null)
            {
                InfoNodeData infoNodeData = (InfoNodeData)nodeContainer.baseNodeData.Find(x => x.Guid == tempNarrativeDataguid);
                title = infoNodeData.personName;

                audioPath = infoNodeData.soundPath;

                var tempNodeLinks = nodeContainer.NodeLinks.Where(x => x.BaseNodeGUID == tempNarrativeDataguid);
                if (tempNodeLinks != null && tempNodeLinks.ToArray().Length != 0)
                {
                    tempNarrativeDataguid = nodeContainer.NodeLinks.Find(x => x.BaseNodeGUID == tempNarrativeDataguid).TargetNodeGUID;
                }
            }

            nodeType = nodeContainer.baseNodeData.Find(x => x.Guid == tempNarrativeDataguid) as DialogueNodeData;
            if (nodeType != null)
            {
                text = ((DialogueNodeData)nodeContainer.baseNodeData.Find(x => x.Guid == tempNarrativeDataguid)).DialogueText;
                //timePerChar = maxTime / text.Count();
                var tempNodeLinks = nodeContainer.NodeLinks.Where(x => x.BaseNodeGUID == tempNarrativeDataguid);
                if (tempNodeLinks != null && tempNodeLinks.ToArray().Length != 0)
                {
                    tempNarrativeDataguid = nodeContainer.NodeLinks.Find(x => x.BaseNodeGUID == tempNarrativeDataguid).TargetNodeGUID;
                }          
            }

            nodeType = nodeContainer.baseNodeData.Find(x => x.Guid == tempNarrativeDataguid) as ChoiceNodeData;
            if (nodeType != null)
            {
                choices = nodeContainer.NodeLinks.Where(x => x.BaseNodeGUID == tempNarrativeDataguid);
            }

            //audioManager.StartDialogue(audioPath);
            if (choices != null)
            {
                DisplayDialogue(text, title, choices);
            }
            else
            {
                DisplayDialogue(text, title, tempNarrativeDataguid);
            }       
        }

        void DisplayDialogue(string text, string title, string narrativeDataguid)
        {
            dialogueBox.Add(Instantiate(oneChoicePrefab, gameObject.transform));

            dialogueText = dialogueBox[0].transform.Find("dialogueText").GetComponent<TextMeshProUGUI>();
            dialogueText.text = currentDisplayText;

            dialogueTitle = dialogueBox[0].transform.Find("dialogueTitle").GetComponent<TextMeshProUGUI>();
            dialogueTitle.text = title;

            var button1 = dialogueBox[0].transform.Find("firstChoiceButton").GetComponent<Button>();
            var button1text = button1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            button1text.text = "Nästa";

            button1.onClick.AddListener(() => ButtonClick(narrativeDataguid));
        }

        void DisplayDialogue(string text, string title, IEnumerable<NodeLinkData> choices)
        {
            dialogueBox.Add(Instantiate(twoChoicePrefab, gameObject.transform));

            dialogueText = dialogueBox[0].transform.Find("dialogueText").GetComponent<TextMeshProUGUI>();
            dialogueText.text = currentDisplayText;

            dialogueTitle = dialogueBox[0].transform.Find("dialogueTitle").GetComponent<TextMeshProUGUI>();
            dialogueTitle.text = title;

            var button1 = dialogueBox[0].transform.Find("firstChoiceButton").GetComponent<Button>();
            var button1text = button1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            button1text.text = choices.ToArray()[0].PortName;

            var button2 = dialogueBox[0].transform.Find("secondChoiceButton").GetComponent<Button>();
            var button2text = button2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            button2text.text = choices.ToArray()[1].PortName;

            button1.onClick.AddListener(() => ButtonClick(choices.ToArray()[0].TargetNodeGUID));
            button2.onClick.AddListener(() => ButtonClick(choices.ToArray()[1].TargetNodeGUID));
        }

        void ButtonClick(string startGuid)
        {
            //audioManager.StartSFX("Button");
            audioManager.StopDialogue();
            ProceedToNarrative(startGuid);
        }
        void RemoveDialogueBox()
        {
            Destroy(dialogueBox[0]);
            dialogueBox.RemoveAt(0);
            //player.GetComponent<PlayerMovement>().enabled = true;
        }
    }
}
