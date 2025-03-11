using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class PickUpScript : MonoBehaviour
{
    public GameObject Player;
    public Transform holdPos;
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index

    public TMP_Text text;

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
    }
    void Update()
    {
        text.text = "";
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                    else if (hit.transform.gameObject.tag == "Door")
                    {
                        hit.transform.parent.gameObject.GetComponent<Door>().InteractDoor();
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    Debug.Log("drop");
                    GameObject cauldron = StopClipping();
                    if (cauldron == null)//prevents object from clipping through walls
                    {
                        DropObject();
                    }
                    else
                    {
                        PickUpMeal(cauldron.transform.gameObject.GetComponent<Cauldron>());
                    }
                }
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
            {
                if (hit.transform.gameObject.tag == "canPickUp" && heldObj == null)
                {
                    text.text = "Pick up";
                }
                else if (hit.transform.gameObject.tag == "Door")
                {
                    if (hit.transform.parent.GetComponent<Door>().Open)
                    {
                        text.text = "Close";
                    }
                    else
                    {
                        text.text = "Open";
                    }                  
                }
                else if(heldObj != null)
                {
                    if (heldObj.gameObject.tag == "Plate" && hit.transform.gameObject.tag == "Cauldron")
                    {
                        text.text = "Pick up meal";
                    }
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            if (Input.GetKeyDown(KeyCode.R) && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    private void PickUpMeal(Cauldron cauldron)
    {       
        GameObject gameObject = cauldron.GetNewMeal();
        if (gameObject != null)
        {
            Destroy(heldObj);
            heldObj = gameObject;
            heldObjRb = gameObject.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            gameObject.GetComponent<Collider>().enabled = false;
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);

            gameObject.transform.position = Player.transform.position;

            IPickupable pickupable = heldObj.GetComponent<IPickupable>();
            if (pickupable != null)
            {
                pickupable.OnPickup();
                Debug.Log("OnPickup called on picked-up object.");
            }
        }
        else
        {
            DropObject();
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            pickUpObj.GetComponent<Collider>().enabled = false;
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);

            IPickupable pickupable = heldObj.GetComponent<IPickupable>();
            if (pickupable != null)
            {
                pickupable.OnPickup();
                Debug.Log("OnPickup called on picked-up object.");
            }
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Player.GetComponent<Collider>(), false);
        heldObj.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj.GetComponent<Collider>().enabled = true;
        heldObj = null; //undefine game object
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), Player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj.GetComponent<Collider>().enabled = true;
        heldObj = null;
    }
    GameObject StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        Debug.Log(hits.Length);
        //if the array length is greater than 0, it has hit an obstical
        if (hits.Length > 0)
        {
            //change object position to camera position 
            Debug.Log(hits[0].collider.gameObject.tag);
            if (hits[0].collider.tag == "Cauldron")
            {
               
                if (heldObj.gameObject.TryGetComponent(out InGameItemTags tags))
                {
                    Debug.Log("Component found");
                    foreach (var item in tags.Tags)
                    {
                        Debug.Log("trying");
                        if (item.TagName == "Plate")
                        {
                            Debug.Log("Plate");
                            return hits[0].collider.gameObject;
                        }
                    }
                }
            }
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 

            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
        return null;
    }
}
