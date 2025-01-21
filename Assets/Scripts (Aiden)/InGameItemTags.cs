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
    public List<TagInfo> Tags 
    {
        get { return _tags; }
    }
    
    public void AddTags(List<TagInfo> newTags)
    {
        _tags = newTags;
    }
}

