using System;
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

    public void ServeFood(GameObject meal)  // kollar efter vilka ingredienser som är i maten och matchar den med karaktären
    {
        if (meal == null)
        {
            return;
        }

        foreach (var ingredient in meal.GetComponent<InGameItemTags>().Tags)
        {
            for (int i = 0; i < dislikedIngredients.Count; i++)
            {
                if (dislikedIngredients[i] == ingredient.TagName)
                {
                    DecreaseLikeMeter();
                    return;
                }
            }            
        }
        foreach (var ingredient in meal.GetComponent<InGameItemTags>().Tags)
        {
            for (int i = 0; i < likedIngredients.Count; i++)
            {
                if (likedIngredients[i] == ingredient.TagName)
                {
                    IncreaseLikeMeter();
                    return;
                }
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
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<PersonManager>().RemovePerson(gameObject.GetComponent<Person>());
            Destroy(this);
        }
    }

    public void Init(List<TagInfo> dislikedTags, List<TagInfo> likedTags)
    {
        foreach (var item in dislikedTags)
        {
            if (item.TagName != "")
            {
                dislikedIngredients.Add(item.TagName);
            }
        }
        foreach (var item in likedTags)
        {
            if (item.TagName != "")
            {
                likedIngredients.Add(item.TagName);
            }
        }
    }
}