using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChopBlock : MonoBehaviour
{
    public GameObject spawnLocation;
    List<GameObject> choppedPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        choppedPrefabs = Resources.LoadAll<GameObject>("ChoppedPrefabs").ToList();
        foreach (var item in choppedPrefabs)
        {
            Debug.Log(item.name);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        bool canChop = false;
        string ingredientTag = "";
        if (other.gameObject.TryGetComponent(out InGameItemTags inGameItemTags))
        {
            foreach (var item in inGameItemTags.Tags)
            {
                if (item.TagName == "Choppable")
                {
                    canChop = true;
                }
                else
                {
                    ingredientTag = item.TagName;
                }
            }
            if (canChop)
            {
                other.transform.rotation = new Quaternion();
                float y = spawnLocation.transform.position.y + spawnLocation.gameObject.GetComponent<Renderer>().bounds.size.y / 2 + other.gameObject.GetComponent<Renderer>().bounds.size.y / 2;
                Vector3 spawnVector = new(spawnLocation.transform.position.x, y, spawnLocation.transform.position.z);

                foreach (var item in choppedPrefabs)
                {
                    GameObject temp = Instantiate(item, new Vector3(0, 5, 0), Quaternion.identity);
                    temp.GetComponent<InspectorItemTags>().Start();
                    foreach (var tagInfo in temp.GetComponent<InGameItemTags>().Tags)
                    {
                        if (tagInfo.TagName == ingredientTag)
                        {
                            GameObject gameObject = Instantiate(temp, spawnVector, new Quaternion());
                            Destroy(temp);
                            Destroy(other.gameObject);
                            return;
                        }
                    }
                    Destroy(temp);
                }
            }
        }
    }
}
