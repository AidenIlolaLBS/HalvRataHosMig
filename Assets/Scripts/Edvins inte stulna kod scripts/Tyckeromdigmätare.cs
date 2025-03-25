using System.Collections.Generic;
using UnityEngine;

public class Tyckeromdigmätare : MonoBehaviour
{
    public enum LikeLevel { ReallyDislikes, Dislikes, Neutral, Likes, ReallyLikes } // olika nivåerna karaktärerna kan gilla spelarn

    [System.Serializable]
    public class NPC
    {
        public string name;
        public LikeLevel likeLevel = LikeLevel.Neutral;
        public List<string> dislikedIngredients;
    }

    public List<NPC> allNPCs = new List<NPC>();
    private List<NPC> activeNPCs = new List<NPC>();

    void Start()
    {
        SelectRandomNPCs();
    }

    void SelectRandomNPCs()  // gambling om vilka karaktärer som kommer in i spelet
    {
        List<NPC> tempNPCs = new List<NPC>(allNPCs);
        for (int i = 0; i < 3; i++)
        {
            if (tempNPCs.Count == 0) break;
            int randomIndex = Random.Range(0, tempNPCs.Count);
            activeNPCs.Add(tempNPCs[randomIndex]);
            tempNPCs.RemoveAt(randomIndex);
        }
    }

    public void ServeFood(NPC npc, List<string> ingredients)  // kollar efter vilka ingredienser som är i maten och matchar den med karaktären
    {
        foreach (string ingredient in ingredients)
        {
            if (npc.dislikedIngredients.Contains(ingredient))
            {
                DecreaseLikeMeter(npc);
                return;
            }
        }
        IncreaseLikeMeter(npc);
    }

    void IncreaseLikeMeter(NPC npc)  // gillar dig mer
    {
        if (npc.likeLevel < LikeLevel.ReallyLikes)
        {
            npc.likeLevel++;
        }
    }

    void DecreaseLikeMeter(NPC npc)  // gillar dig mindre
    {
        if (npc.likeLevel > LikeLevel.ReallyDislikes)
        {
            npc.likeLevel--;
        }

        if (npc.likeLevel == LikeLevel.ReallyDislikes)  // försvinner om karaktären hatar dig
        {
            activeNPCs.Remove(npc);
        }
    }
}


