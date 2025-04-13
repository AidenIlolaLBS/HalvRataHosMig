using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChopBlock : MonoBehaviour
{
    public GameObject spawnLocation;
    List<GameObject> choppedPrefabs;
    AudioManager audioManager;
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
                foreach (var item in choppedPrefabs)
                {
                    GameObject temp = Instantiate(item, new Vector3(0, 5, 0), Quaternion.identity);
                    temp.GetComponent<InspectorItemTags>().Start();
                    for (int i = 0; i < temp.GetComponent<InGameItemTags>().Tags.Count; i++)
                    {
                        if (temp.GetComponent<InGameItemTags>().Tags[i].TagName == ingredientTag)
                        {
                            Destroy(temp.GetComponent<InspectorItemTags>());
                            temp.GetComponent<InGameItemTags>().RemoveTag(i);
                            float y = spawnLocation.transform.position.y + spawnLocation.gameObject.GetComponent<Renderer>().bounds.size.y / 2 + temp.GetComponent<Renderer>().bounds.size.y / 2;
                            Vector3 spawnVector = new(spawnLocation.transform.position.x, y, spawnLocation.transform.position.z);
                            temp.transform.position = spawnVector;
                            Destroy(other.gameObject);
                            GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>().StartSFX(SoundType.ChopSound);
                            return;
                        }
                    }
                    Destroy(temp);
                }
            }
        }
    }
}
