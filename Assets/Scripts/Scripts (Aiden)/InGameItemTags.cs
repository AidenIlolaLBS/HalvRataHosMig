using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InGameItemTags : MonoBehaviour
{
    private List<TagInfo> _tags = new();

    public bool fullMeal = false;
    public string fullMealName = "";
    public List<TagInfo> Tags 
    {
        get { return _tags; }
    }
    
    public List<string> names = new List<string>();

    public void AddTags(List<TagInfo> newTags)
    {
        foreach (var item in newTags)
        {
            if (item.Active)
            {
                _tags.Add(item);
                names.Add(item.TagName);
            }
        }
    }

    public void RemoveTag(int  index)
    {
        _tags.RemoveAt(index);
        names.RemoveAt(index);
    }
}

