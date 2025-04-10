using Subtegral.DialogueSystem.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public Tyckeromdigmätare tyckeromdigmätare;
    string _personName;

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

    public int haveTalked = 0;

    public string PersonName
    {
        get { return _personName; }
    }

    public Person()
    {

    }

    public void Talk()
    {
        if (haveTalked == 1)
        {
            switch (GameObject.FindGameObjectWithTag("GameManaager").GetComponent<GameSceneManager>().CurrentGameLoop)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
        else if (haveTalked == 0)
        {
            System.Random rnd = new System.Random();
            switch (tyckeromdigmätare.likeLevel)
            {
                case LikeLevel.ReallyDislikes:

                    GameObject.FindGameObjectWithTag("DialogueParser").GetComponent<DialogueParser>().ExternalStartNarrative(reallyDislikeMealDialogue[rnd.Next(0, reallyDislikeMealDialogue.Count - 1)]);
                    break;
                case LikeLevel.Dislikes:
                    break;
                case LikeLevel.Neutral:
                    break;
                case LikeLevel.Likes:
                    break;
                case LikeLevel.ReallyLikes:
                    break;
                default:
                    break;
            }
        }        
    }

    private void Start()
    {
        tyckeromdigmätare = new(gameObject.GetComponent<InGameItemTags>());
    }
}
