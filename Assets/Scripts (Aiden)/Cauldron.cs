using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    private List<string> containingTags = new();
    private List<string> meals = new(){ "ToastSkagish", "Eggsallad" , "Gratin", "Soup", "Spaghetti", "Pie", "IceCream", "Random"};

    public GameObject spawnLocation;

    //public void TakeOutMeal()
    //{
    //    GameObject newGameObject = new();
    //    newGameObject.transform.position = spawnLocation.transform.position;
    //    newGameObject.AddComponent<InspectorItemTags>();
    //}

    public GameObject GetNewMeal()
    {
        if (containingTags.Count > 3)
        {
            GameObject newObject = new();
            newObject.AddComponent<InGameItemTags>();
            
            List<TagInfo> newTags = new();
            string mealName = GetMealName();
            if (mealName != "Random")
            {
                newObject.GetComponent<InGameItemTags>().fullMeal = true;
                newTags.Add(new TagInfo(mealName, true));
            }
            foreach (var item in containingTags)
            {
                newTags.Add(new TagInfo(item, true));
            }
            newObject.GetComponent<InGameItemTags>().AddTags(newTags);

            newObject.AddComponent<Rigidbody>();
            newObject.AddComponent<BoxCollider>();

            return newObject;
        }
        return null;
    }

    private string GetMealName()
    {
        //Lägg till namnen också i variabeln meals
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
        else
        {
            return "Random";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "canPickUp")
        {
            bool dontDestroy = false;
            List<TagInfo> newTags = collision.gameObject.GetComponent<InGameItemTags>().Tags;

            if (!collision.gameObject.GetComponent<InGameItemTags>().fullMeal)
            {
                foreach (var item in newTags)
                {
                    if (item.Active)
                    {
                        if (!containingTags.Contains(item.TagName))
                        {
                            containingTags.Add(item.TagName);
                            dontDestroy = true;
                        }
                        else if (!dontDestroy)
                        {
                            dontDestroy = false;
                        }
                    }
                }

                if (!dontDestroy)
                {
                    Destroy(collision.gameObject);
                }
            }        
        }
    }
}
