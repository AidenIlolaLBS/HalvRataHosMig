using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    private ItemTags containingTags = new ItemTags();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "canPickUp")
        {
            //List<TagInfo> collisionObjectTags = (collision.gameObject.GetComponent<ItemTags>()).tags;
            //for (int i = 0; i < containingTags.tags.Count; i++)
            //{
            //    if (collisionObjectTags[i].active)
            //    {
            //        containingTags.tags[i].active = true;
            //    }
            //}

            foreach (var item in containingTags.GetType().GetProperties())
            {
                Debug.Log(item.Name);
            }

            Destroy(collision.gameObject);
        }
    }
}
