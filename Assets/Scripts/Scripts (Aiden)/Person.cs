using Subtegral.DialogueSystem.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Person : MonoBehaviour
{
    public Tyckeromdigmätare tyckeromdigmätare;
    string _personName;

    public List<AudioClip> audioClips = new List<AudioClip>();

    public GameObject likedIngredients;
    public GameObject dislikedIngredients;

    public List<GameObject> personStates = new List<GameObject>();

    public NodeContainer IntroDialogue;
    public List<NodeContainer> likeMealDialogue;
    public List<NodeContainer> reallyLikeMealDialogue;
    public List<NodeContainer> dislikeMealDialogue;
    public List<NodeContainer> reallyDislikeMealDialogue;
    public List<NodeContainer> likesDislikesMealDialogue;
    public List<NodeContainer> dislikesLikesMealDialogue;
    public List<NodeContainer> leavesPositiveMealDialogue;
    public List<NodeContainer> leavesNegativeMealDialogue;

    public NodeContainer[] mealDialogue1 = new NodeContainer[5];
    public NodeContainer[] mealDialogue2 = new NodeContainer[5];
    public NodeContainer[] mealDialogue3 = new NodeContainer[5];

    NodeContainer[][] allMealDialogues = new NodeContainer[3][];

    public int haveTalked = 0;

    bool haveStarted = false;

    public string PersonName
    {
        get { return _personName; }
    }

    public Person()
    {
        
    }

    public void Start()
    {
        if (!haveStarted)
        {
            allMealDialogues[0] = mealDialogue1;
            allMealDialogues[1] = mealDialogue2;
            allMealDialogues[2] = mealDialogue3;

            dislikedIngredients.GetComponent<InspectorItemTags>().Start();
            likedIngredients.GetComponent<InspectorItemTags>().Start();

            tyckeromdigmätare = gameObject.AddComponent<Tyckeromdigmätare>();
            tyckeromdigmätare.Init(dislikedIngredients.GetComponent<InGameItemTags>().Tags, likedIngredients.GetComponent<InGameItemTags>().Tags);
            haveStarted = true;
        }
    }

    private void Update()
    {
        for (int i = 0; i < personStates.Count; i++)
        {
            personStates[i].SetActive(false);
        }
        switch (tyckeromdigmätare.likeLevel)
        {
            case LikeLevel.ReallyDislikes:
                personStates[3].SetActive(true);
                break;
            case LikeLevel.Dislikes:
                personStates[2].SetActive(true);
                break;
            case LikeLevel.Neutral:
                personStates[1].SetActive(true);
                break;
            case LikeLevel.Likes:
                personStates[0].SetActive(true);
                break;
            case LikeLevel.ReallyLikes:
                personStates[0].SetActive(true);
                break;
            default:
                break;
        }   
    }

    public void Talk()
    {
        Debug.Log(tyckeromdigmätare.likeLevel);
        DialogueParser dialogueParser = GameObject.FindGameObjectWithTag("DialogueParser").GetComponent<DialogueParser>();
        AudioManager audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
        if (haveTalked == 1)
        {
            switch (tyckeromdigmätare.likeLevel)
            {
                case LikeLevel.ReallyDislikes:
                    audioManager.StartDialogue(audioClips[3]);
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][4], gameObject);
                    break;
                case LikeLevel.Dislikes:
                    audioManager.StartDialogue(audioClips[2]);
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][3], gameObject);
                    break;
                case LikeLevel.Neutral:
                    audioManager.StartDialogue(audioClips[1]);
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][2], gameObject);
                    break;
                case LikeLevel.Likes:
                    audioManager.StartDialogue(audioClips[0]);
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][1], gameObject);
                    break;
                case LikeLevel.ReallyLikes:
                    audioManager.StartDialogue(audioClips[0]);
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][0], gameObject);
                    break;
                default:
                    break;
            }
            haveTalked++;
            gameObject.tag = "Untagged";
        }
        else if (haveTalked == 0)
        {
            System.Random rnd = new System.Random();
            if (tyckeromdigmätare.prevLikeLevelChange > 0 && tyckeromdigmätare.likeLevelChange > 0) // Really likes
            {
                dialogueParser.ExternalStartNarrative(reallyLikeMealDialogue[rnd.Next(0, reallyLikeMealDialogue.Count)]);
            }
            else if (tyckeromdigmätare.prevLikeLevelChange > 0 && tyckeromdigmätare.likeLevelChange < 0) // Likes dislikes
            {
                dialogueParser.ExternalStartNarrative(likesDislikesMealDialogue[rnd.Next(0, likesDislikesMealDialogue.Count)]);
            }
            else if (tyckeromdigmätare.prevLikeLevelChange == 0 && tyckeromdigmätare.likeLevelChange > 0) // Likes
            {
                dialogueParser.ExternalStartNarrative(likeMealDialogue[rnd.Next(0, likeMealDialogue.Count)]);
            }
            else if (tyckeromdigmätare.prevLikeLevelChange == 0 && tyckeromdigmätare.likeLevelChange == 0) // Neutral / Likes
            {
                dialogueParser.ExternalStartNarrative(likeMealDialogue[rnd.Next(0, likeMealDialogue.Count)]);
            }
            else if (tyckeromdigmätare.prevLikeLevelChange == 0 && tyckeromdigmätare.likeLevelChange < 0) // Dislikes
            {
                dialogueParser.ExternalStartNarrative(dislikeMealDialogue[rnd.Next(0, dislikeMealDialogue.Count)]);
            }
            else if (tyckeromdigmätare.prevLikeLevelChange < 0 && tyckeromdigmätare.likeLevelChange > 0) // Dislikes likes
            {
                dialogueParser.ExternalStartNarrative(dislikesLikesMealDialogue[rnd.Next(0, dislikesLikesMealDialogue.Count)]);
            }
            else if (tyckeromdigmätare.prevLikeLevelChange < 0 && tyckeromdigmätare.likeLevelChange < 0) // Really dislikes
            {
                dialogueParser.ExternalStartNarrative(reallyDislikeMealDialogue[rnd.Next(0, reallyDislikeMealDialogue.Count)]);
            }

            switch (tyckeromdigmätare.likeLevel)
            {
                case LikeLevel.ReallyDislikes:
                    audioManager.StartDialogue(audioClips[3]);
                    break;
                case LikeLevel.Dislikes:
                    audioManager.StartDialogue(audioClips[2]);
                    break;
                case LikeLevel.Neutral:
                    audioManager.StartDialogue(audioClips[1]);
                    break;
                case LikeLevel.Likes:
                    audioManager.StartDialogue(audioClips[0]);
                    break;
                case LikeLevel.ReallyLikes:
                    audioManager.StartDialogue(audioClips[0]);
                    break;
                default:
                    break;
            }
            haveTalked++;
        }        
    }
}
