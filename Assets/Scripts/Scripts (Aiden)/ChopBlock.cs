using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopBlock : MonoBehaviour
{
    public GameObject spawnLocation;
    GameObject ingredient = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (ingredient == null)
        {
            if (other.gameObject.TryGetComponent(out InGameItemTags inGameItemTags))
            {
                foreach (var item in inGameItemTags.Tags)
                {
                    if (item.TagName == "Choppable")
                    {
                        other.transform.rotation = new Quaternion();
                        float y = spawnLocation.transform.position.y + spawnLocation.gameObject.GetComponent<Renderer>().bounds.size.y/2 + other.gameObject.GetComponent<Renderer>().bounds.size.y / 2;
                        Vector3 spawnVector = new(spawnLocation.transform.position.x, y, spawnLocation.transform.position.z);
                        other.transform.position = spawnVector;
                        other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3();
                        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        other.gameObject.tag = "Chop";
                        ingredient = other.gameObject;
                        return;
                    }
                }
            }
        }
    }
}
