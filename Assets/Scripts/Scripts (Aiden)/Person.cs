using Subtegral.DialogueSystem.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Person : MonoBehaviour
{
    public Tyckeromdigmätare tyckeromdigmätare;
    string _personName;

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

    public string PersonName
    {
        get { return _personName; }
    }

    public Person()
    {
        
    }

    public void Start()
    {
        allMealDialogues[0] = mealDialogue1;
        allMealDialogues[1] = mealDialogue2;
        allMealDialogues[2] = mealDialogue3;

        Debug.Log(gameObject.name);

        likedIngredients.GetComponent<InspectorItemTags>().Start();
        dislikedIngredients.GetComponent<InspectorItemTags>().Start();

        DestroyImmediate(likedIngredients.GetComponent<InspectorItemTags>(), true);
        DestroyImmediate(dislikedIngredients.GetComponent<InspectorItemTags>(), true);

        tyckeromdigmätare = new Tyckeromdigmätare(dislikedIngredients.GetComponent<InGameItemTags>(), likedIngredients.GetComponent<InGameItemTags>());
    }

    private void Update()
    {
        int currentFeeling = (int)tyckeromdigmätare.likeLevel - 1;
        if (currentFeeling < 0)
        {
            currentFeeling = 0;
        }
        for (int i = 0; i < personStates.Count; i++)
        {
            personStates[i].SetActive(false);
        }
        personStates[currentFeeling].SetActive(true);
    }

    public void Talk()
    {
        DialogueParser dialogueParser = GameObject.FindGameObjectWithTag("DialogueParser").GetComponent<DialogueParser>();
        if (haveTalked == 1)
        {
            switch (tyckeromdigmätare.likeLevel)
            {
                case LikeLevel.ReallyDislikes:
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][4], gameObject);
                    break;
                case LikeLevel.Dislikes:
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][3], gameObject);
                    break;
                case LikeLevel.Neutral:
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][2], gameObject);
                    break;
                case LikeLevel.Likes:
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][1], gameObject);
                    break;
                case LikeLevel.ReallyLikes:
                    dialogueParser.ExternalStartNarrative(allMealDialogues[GameSceneManager.CurrentGameLoop][0], gameObject);
                    break;
                default:
                    break;
            }
            haveTalked++;
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
            haveTalked++;
        }        
    }
}
