using System.Collections.Generic;
using UnityEngine;

public enum LikeLevel { ReallyDislikes, Dislikes, Neutral, Likes, ReallyLikes } // olika nivåerna karaktärerna kan gilla spelarn
public class Tyckeromdigmätare : MonoBehaviour
{
    public LikeLevel likeLevel = LikeLevel.Neutral;
    public LikeLevel prevLikeLevel = LikeLevel.Neutral;
    public int likeLevelChange = 0;
    public int prevLikeLevelChange = 0;
    public List<string> dislikedIngredients = new();
    public List<string> likedIngredients = new();

    public Tyckeromdigmätare(InGameItemTags dislikedTags, InGameItemTags likedTags)
    {
        foreach (var item in dislikedTags.Tags)
        {
            if (item != null)
            {
                dislikedIngredients.Add(item.TagName);
            }
        }
        foreach (var item in likedTags.Tags)
        {
            if (item != null)
            {
                likedIngredients.Add(item.TagName);
            }
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
        foreach (var ingredient in meal.GetComponent<InGameItemTags>().Tags)
        {
            if (likedIngredients.Contains(ingredient.TagName))
            {
                IncreaseLikeMeter();
                return;
            }
        }
    }

    public void IncreaseLikeMeter(bool food = false)  // gillar dig mer
    {
        if (likeLevel < LikeLevel.ReallyLikes)
        {
            prevLikeLevelChange = likeLevelChange;
            prevLikeLevel = likeLevel;
            likeLevel++;
            if (food)
            {
                likeLevelChange = 1;
            }
            else
            {
                likeLevelChange = 0;
            }
        }
    }

    public void DecreaseLikeMeter(bool food = false)  // gillar dig mindre
    {
        prevLikeLevelChange = likeLevelChange;
        prevLikeLevel = likeLevel;
        likeLevel--;
        if (food)
        {
            likeLevelChange = -1;
        }
        else
        {
            likeLevelChange = 0;
        }

        if (likeLevel == LikeLevel.ReallyDislikes && prevLikeLevel == LikeLevel.ReallyDislikes)  // försvinner om karaktären hatar dig
        {
            Debug.Log("Objekt dör");
            Destroy(this);
        }
    }
}