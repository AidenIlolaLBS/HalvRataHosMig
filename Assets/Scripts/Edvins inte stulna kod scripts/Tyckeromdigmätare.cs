using System.Collections.Generic;
using UnityEngine;

public enum LikeLevel { ReallyDislikes, Dislikes, Neutral, Likes, ReallyLikes } // olika nivåerna karaktärerna kan gilla spelarn
public class Tyckeromdigmätare : MonoBehaviour
{
    public LikeLevel likeLevel = LikeLevel.Neutral;
    public LikeLevel prevlikeLevel = LikeLevel.Neutral;
    public List<string> dislikedIngredients;

    public Tyckeromdigmätare(InGameItemTags tags)
    {
        foreach (var item in tags.Tags)
        {
            dislikedIngredients.Add(item.TagName);
        }
    }

    public void ServeFood(GameObject meal)  // kollar efter vilka ingredienser som är i maten och matchar den med karaktären
    {
        foreach (var ingredient in meal.GetComponent<InGameItemTags>().Tags)
        {
            if (dislikedIngredients.Contains(ingredient.TagName))
            {
                DecreaseLikeMeter();
                return;
            }
        }
        IncreaseLikeMeter();
    }

    public void IncreaseLikeMeter()  // gillar dig mer
    {
        if (likeLevel < LikeLevel.ReallyLikes)
        {
            prevlikeLevel = likeLevel;
            likeLevel++;
        }
    }

    public void DecreaseLikeMeter()  // gillar dig mindre
    {
        prevlikeLevel = likeLevel;
        likeLevel--;

        if (likeLevel == LikeLevel.ReallyDislikes && prevlikeLevel == LikeLevel.ReallyDislikes)  // försvinner om karaktären hatar dig
        {
            Destroy(this);
        }
    }
}