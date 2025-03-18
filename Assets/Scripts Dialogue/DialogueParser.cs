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
        AudioSource audioSource;
        AudioClip currentClip;
        float timer = 0;
        private int charindex = 0;
        private float maxTime = 3;
        private float timePerChar = 0.15f;
        bool allTextLaoded = false;

        private void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            ExternalStartNarrative();
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
            var narrativeData = nodeContainer.NodeLinks.First(); //Entrypoint node
            //player.GetComponent<PlayerMovement>().enabled = false;
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        private void ProceedToNarrative(string startNarrativeDataGUID)
        {
            allTextLaoded = false;
            Debug.Log(audioSource);
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
                return;
            }

            nodeType = nodeContainer.baseNodeData.Find(x => x.Guid == tempNarrativeDataguid) as InfoNodeData;
            if (nodeType != null)
            {
                InfoNodeData infoNodeData = (InfoNodeData)nodeContainer.baseNodeData.Find(x => x.Guid == tempNarrativeDataguid);
                title = infoNodeData.personName;

                if (infoNodeData.soundPath != null)
                {
                    string[] guids = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, infoNodeData.soundPath));
                    if (guids.Length != 0)
                    {
                        System.Random random = new System.Random();
                        Debug.Log(guids.Length);
                        currentClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guids[random.Next(0, guids.Length)]));
                        Debug.Log(currentClip.name);
                    }
                }

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
            

            if (choices != null)
            {
                DisplayDialogue(text, title, choices);
            }
            else
            {
                DisplayDialogue(text, title, tempNarrativeDataguid);
            }

            //while loop to find all info
                //check if node is info
                    //sets the name to the info.text

                //check if node is dialogue
                    //set text to the dialogue.text
                    //check if next node is choice
                        //sets choices
            //displays the dialogue          
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

            PlaySound();

            button1.onClick.AddListener(() => ProceedToNarrative(narrativeDataguid));
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

            PlaySound();

            button1.onClick.AddListener(() => ProceedToNarrative(choices.ToArray()[0].TargetNodeGUID));
            button2.onClick.AddListener(() => ProceedToNarrative(choices.ToArray()[1].TargetNodeGUID));
        }

        void PlaySound()
        {
            if (currentClip != null)
            {
                audioSource.clip = currentClip;
                audioSource.Play();
            }
        }

        void RemoveDialogueBox()
        {
            Destroy(dialogueBox[0]);
            dialogueBox.RemoveAt(0);
            //player.GetComponent<PlayerMovement>().enabled = true;
        }
    }
}
