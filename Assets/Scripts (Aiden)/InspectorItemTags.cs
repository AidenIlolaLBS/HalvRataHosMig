using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorItemTags : MonoBehaviour
{
    [HideInInspector]
    public List<TagInfo> tags = new();

    public bool fullMeal = false;

   //Kom ihåg att lägga till dessa tags i tags listan genom att lägga till de i AddTags().
   //Dem behövs även läggas till i allTags
    public bool Bread = false;
    public bool Seaweed = false;
    public bool ChoppedSeaweed = false;
    public bool ToastSkagish = false;
    public bool DragonEgg = false;
    public bool LizardHeart = false;
    public bool Eggsallad = false;
    public bool Fairy = false;
    public bool Flour = false;
    public bool Gratin = false;
    public bool Water = false;
    public bool BatWing = false;
    public bool Soup = false;
    public bool FlyAgaric = false;
    public bool Spaghetti = false;
    public bool Pie = false;
    public bool StinkFruit = false;
    public bool DeadBerry = false;
    public bool IceCream = false;
    public InspectorItemTags()
    {
        AddTags();
    }

    public void Start()
    {
        AddTags();
        this.gameObject.AddComponent<InGameItemTags>();
        this.gameObject.GetComponent<InGameItemTags>().AddTags(tags);
        this.gameObject.GetComponent<InGameItemTags>().fullMeal = fullMeal;
        this.gameObject.GetComponent<InspectorItemTags>().enabled = false;
    }

    public void AddTags()
    {
        tags.Clear();
        //Måste lägga till manuellt alla tags i tags listan
        tags.Add(new TagInfo(nameof(Bread), Bread));
        tags.Add(new TagInfo(nameof(Seaweed), Seaweed));
        tags.Add(new TagInfo(nameof(ChoppedSeaweed), ChoppedSeaweed));
        tags.Add(new TagInfo(nameof(ToastSkagish), ToastSkagish));
        tags.Add(new TagInfo(nameof(DragonEgg), DragonEgg));
        tags.Add(new TagInfo(nameof(LizardHeart), LizardHeart));
        tags.Add(new TagInfo(nameof(Eggsallad), Eggsallad));
        tags.Add(new TagInfo(nameof(Fairy), Fairy));
        tags.Add(new TagInfo(nameof(Flour), Flour));
        tags.Add(new TagInfo(nameof(Gratin), Gratin));
        tags.Add(new TagInfo(nameof(Water), Water));
        tags.Add(new TagInfo(nameof(BatWing), BatWing));
        tags.Add(new TagInfo(nameof(Soup), Soup));
        tags.Add(new TagInfo(nameof(FlyAgaric), FlyAgaric));
        tags.Add(new TagInfo(nameof(Spaghetti), Spaghetti));
        tags.Add(new TagInfo(nameof(Pie), Pie));
        tags.Add(new TagInfo(nameof(StinkFruit), StinkFruit));
        tags.Add(new TagInfo(nameof(DeadBerry), DeadBerry));
        tags.Add(new TagInfo(nameof(IceCream), IceCream));
    }
}
