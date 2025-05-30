using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [SerializeField]
    private List<string> containingTags = new();
    //private List<string> meals = new(){ "ToastSkagish", "Eggsallad" , "Gratin", "Soup", "Spaghetti", "Pie", "IceCream", "Random"};
    private List<GameObject> prefabMeals = new();
    int minIngredients = 2;

    private void Start()
    {
        prefabMeals = Resources.LoadAll<GameObject>("MealPrefabs").ToList();
    }

    public bool CanGetMeal()
    {
        if (containingTags.Count > minIngredients)
        {
            return true;
        }
        return false;
    }

    public GameObject GetNewMeal()
    {
        if (containingTags.Count > minIngredients)
        {
            string mealName = GetMealName();
            
            for (int i = 0; i < prefabMeals.Count; i++)
            {
                GameObject temp = Instantiate(prefabMeals[i], new Vector3(0, 5, 0), Quaternion.identity);
                temp.GetComponent<InspectorItemTags>().Start();
                for (int j = 0; j < temp.GetComponent<InGameItemTags>().Tags.Count; j++)
                {
                    if (mealName == temp.GetComponent<InGameItemTags>().Tags[j].TagName)
                    {
                        Debug.Log(mealName);
                        foreach (var item in containingTags)
                        {
                            if (item != mealName)
                            {
                                temp.GetComponent<InGameItemTags>().Tags.Add(new TagInfo(item,true));
                            }
                        }
                        temp.GetComponent<InGameItemTags>().fullMeal = true;
                        temp.GetComponent<InGameItemTags>().fullMealName = mealName;
                        containingTags.Clear();
                        return temp;
                    }
                }
                Destroy(temp);
            }
        }
        return null;
    }

    private string GetMealName()
    {
        //L�gg till namnen ocks� i variabeln meals
        if (containingTags.Contains("Bread") && containingTags.Contains("ChoppedSeaweed")) //Toast skagish
        {
            return "ToastSkagish";
        }
        else if (containingTags.Contains("DragonEgg") && containingTags.Contains("LizardHeart")) //Eggsallad
        {
            return "Eggsallad";
        }
        else if (containingTags.Contains("Fairy") && containingTags.Contains("Flour") && containingTags.Contains("Seaweed")) //Gratin
        {
            return "Gratin";
        }
        else if (containingTags.Contains("Water") && containingTags.Contains("BatWing") && containingTags.Contains("Bread")) //Soup
        {
            return "Soup";
        }
        else if (containingTags.Contains("Seaweed") && containingTags.Contains("FlyAgaric") && containingTags.Contains("LizardHeart")) //Spaghetti
        {
            return "Spaghetti";
        }
        else if (containingTags.Contains("Flour") && containingTags.Contains("DragonEgg")) //Pie
        {
            return "Pie";
        }
        else if (containingTags.Contains("StinkFruit") && containingTags.Contains("DeadBerry")) //IceCream
        {
            return "IceCream";
        }
        else if (containingTags.Contains("Seaweed") && containingTags.Contains("ChoppedSeaweed")) //Sallad
        {
            return "Sallad";
        }
        else
        {
            return "Random";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "canPickUp")
        {
            bool shouldDestroy = false;
            List<TagInfo> newTags = collision.gameObject.GetComponent<InGameItemTags>().Tags;

            if (!collision.gameObject.GetComponent<InGameItemTags>().fullMeal)
            {
                foreach (var item in newTags)
                {
                    if (item.Active)
                    {
                        if (item.TagName == "Plate" || item.TagName == "Glass")
                        {
                            continue;
                        }
                        if (!containingTags.Contains(item.TagName) && item.TagName != "Choppable")
                        {
                            containingTags.Add(item.TagName);
                            shouldDestroy = true;
                        }
                    }
                }
                if (shouldDestroy)
                {
                    Destroy(collision.gameObject);
                }
            }        
        }
    }
}