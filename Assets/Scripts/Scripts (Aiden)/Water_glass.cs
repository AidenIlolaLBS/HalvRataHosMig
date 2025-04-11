using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_glass : MonoBehaviour
{
    public GameObject water;
    void Start()
    {
        water.SetActive(false);
    }
    
    public void FillWater()
    {
        water.SetActive(true);
        gameObject.GetComponent<InGameItemTags>().AddTags(new List<TagInfo> { new TagInfo("Water", true) });
    }
}
