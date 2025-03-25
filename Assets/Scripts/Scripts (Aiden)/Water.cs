using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out InGameItemTags inGameItemTags))
        {
            foreach (var item in inGameItemTags.Tags)
            {
                if (item.TagName == "Glass")
                {
                    other.GetComponent<Water_glass>().FillWater();
                    return;
                }
            }
        }
    }
}
