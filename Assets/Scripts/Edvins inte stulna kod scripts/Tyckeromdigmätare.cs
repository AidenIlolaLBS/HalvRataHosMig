using System.Collections.Generic;
using UnityEngine;

public class Tyckeromdigmätare : MonoBehaviour
{
    public enum LikeLevel { ReallyDislikes, Dislikes, Neutral, Likes, ReallyLikes } // olika nivåerna karaktärerna kan gilla spelarn
    public LikeLevel likeLevel = LikeLevel.Neutral;
    public List<string> dislikedIngredients;

    public Tyckeromdigmätare(List<string> dislikedIngredients)
    {
        this.dislikedIngredients = dislikedIngredients;
    }

    public void ServeFood(List<TagInfo> ingredients)  // kollar efter vilka ingredienser som är i maten och matchar den med karaktären
    {
        foreach (var ingredient in ingredients)
        {
            if (dislikedIngredients.Contains(ingredient.TagName))
            {
                DecreaseLikeMeter();
                return;
            }
        }
        IncreaseLikeMeter();
    }

    void IncreaseLikeMeter()  // gillar dig mer
    {
        if (likeLevel < LikeLevel.ReallyLikes)
        {
            likeLevel++;
        }
    }

    void DecreaseLikeMeter()  // gillar dig mindre
    {
        likeLevel--;

        if (likeLevel == LikeLevel.ReallyDislikes)  // försvinner om karaktären hatar dig
        {
            Destroy(this);
        }
    }
}


