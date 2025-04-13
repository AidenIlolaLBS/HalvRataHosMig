using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Unity.Burst.CompilerServices;

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
        if (text != null)
        {
            text.text = "";
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    switch (hit.transform.gameObject.tag)
                    {
                        case "canPickUp":
                            PickUpObject(hit.transform.gameObject);
                            GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>().StartSFX(SoundType.PickUpSound);
                            break;
                        case "Door":
                            hit.transform.parent.gameObject.GetComponent<Door>().InteractDoor();
                            break;
                        case "Person":
                            hit.transform.gameObject.GetComponent<Person>().Talk();
                            break;
                        case "Sink":
                            hit.transform.gameObject.GetComponent<Sink>().Interact();
                            break;
                        case "CookBook":
                            hit.transform.gameObject.GetComponent<CookBook>().Interact();
                            break;
                        case "FinishMeal":
                            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSceneManager>().NextScene();
                            break;
                        default:
                            break;
                    }                
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if (hit.transform.tag == "DoorDiningRoom")
                    {
                        if (heldObj.TryGetComponent(out InGameItemTags inGameItemTags))
                        {
                            if (inGameItemTags.fullMeal)
                            {
                                Debug.Log(heldObj.name);
                                heldObj.transform.parent = null;
                                GameObject.FindGameObjectWithTag("GameManager").GetComponent<MealManager>().currentMeal = heldObj;
                                hit.transform.parent.gameObject.GetComponent<Door>().InteractDoor();
                            }
                        }                        
                    }                        
                }
                if (canDrop == true)
                {
                    GameObject cauldron = StopClipping();
                    if (cauldron == null)//prevents object from clipping through walls
                    {
                        DropObject();
                        GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>().StartSFX(SoundType.DropSound);
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
                if (text != null)
                {
                    switch (hit.transform.gameObject.tag)
                    {
                        case "canPickUp":
                            string additionalText = "";
                            if (heldObj == null)
                            {
                                if (hit.transform.TryGetComponent(out InGameItemTags inGameItemTags))
                                {
                                    if (inGameItemTags.fullMeal)
                                    {
                                        additionalText = inGameItemTags.fullMealName;
                                    }
                                    else
                                    {
                                        if (inGameItemTags.Tags.Count > 0)
                                        {
                                            additionalText = inGameItemTags.Tags[0].TagName;
                                        }
                                    }
                                    additionalText = ConvertToSwedish(additionalText);
                                }
                                text.text = "Plocka upp " + additionalText;
                            }
                            break;
                        case "Door":
                            if (hit.transform.parent.GetComponent<Door>().Open)
                            {
                                text.text = "Stäng";
                            }
                            else
                            {
                                text.text = "Öppna";
                            }
                            break;
                        case "Person":
                            text.text = "Prata";
                            break;
                        case "Cauldron":
                            if (heldObj != null)
                            {
                                if (heldObj.gameObject.TryGetComponent(out InGameItemTags test))
                                {
                                    foreach (var item in test.Tags)
                                    {
                                        if (item.TagName == "Plate" && hit.transform.gameObject.tag == "Cauldron")
                                        {
                                            if (hit.transform.gameObject.GetComponent<Cauldron>().CanGetMeal())
                                            {
                                                text.text = "Plocka upp maträtt";
                                            }
                                            else
                                            {
                                                text.text = "Mer ingredienser behövs";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (hit.transform.gameObject.GetComponent<Cauldron>().CanGetMeal())
                                {
                                    text.text = "Talrik behövs";
                                }
                                else
                                {
                                    text.text = "Mer ingredienser behövs";
                                }
                            }
                            break;
                        case "Sink":
                            if (hit.transform.GetComponent<Sink>().GetWaterStatus())
                            {
                                text.text = "Stäng av vatten";
                            }
                            else
                            {
                                text.text = "Sätt på vatten";
                            }
                            break;
                        case "CookBook":
                            text.text = "Öppna kokbok";
                            break;
                        case "DoorDiningRoom":
                            if (heldObj != null)
                            {
                                if (heldObj.TryGetComponent(out InGameItemTags inGameItemTags))
                                {
                                    if (inGameItemTags.fullMeal)
                                    {
                                        text.text = "Gå till matsal";
                                        break;
                                    }
                                }                                                                
                            }
                            text.text = "Maträtt behövs";
                            break;
                        case "FinishMeal":
                            text.text = "Avsluta måltid";
                            break;
                        default:
                            break;
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
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>().StartSFX(SoundType.ThrowSound);
            }
        }
    }

    private string ConvertToSwedish(string additionalText)
    {
        InspectorItemTags tags = new InspectorItemTags();
        string newText = "";
        switch (additionalText)
        {
            case nameof(tags.Bread):
                newText = "bröd";
                break;
            case nameof(tags.Seaweed):
                newText = "sjögräs";
                break;
            case nameof(tags.ChoppedSeaweed):
                newText = "hackad sjögräs";
                break;
            case nameof(tags.ToastSkagish):
                newText = "toast skagish";
                break;
            case nameof(tags.DragonEgg):
                newText = "drakägg";
                break;
            case nameof(tags.LizardHeart):
                newText = "ödlehjärta";
                break;
            case nameof(tags.Eggsallad):
                newText = "äggsallad";
                break;
            case nameof(tags.Fairy):
                newText = "älva";
                break;
            case nameof(tags.Flour):
                newText = "mjöl";
                break;
            case nameof(tags.Gratin):
                newText = "gratäng";
                break;
            case nameof(tags.Water):
                newText = "vatten";
                break;
            case nameof(tags.BatWing):
                newText = "fladdermusvinge";
                break;
            case nameof(tags.Soup):
                newText = "soppa";
                break;
            case nameof(tags.FlyAgaric):
                newText = "flugsvamp";
                break;
            case nameof(tags.Spaghetti):
                newText = "spaghetti";
                break;
            case nameof(tags.Pie):
                newText = "paj";
                break;
            case nameof(tags.StinkFruit):
                newText = "stinkfrukt";
                break;
            case nameof(tags.DeadBerry):
                newText = "dödabär";
                break;
            case nameof(tags.IceCream):
                newText = "glass";
                break;
            case nameof(tags.Random):
                newText = "maträtt";
                break;
            case nameof(tags.Lime):
                newText = "lime";
                break;
            case nameof(tags.Plate):
                newText = "tallrik";
                break;
            case nameof(tags.Glass):
                newText = "glas";
                break;
            case nameof(tags.HalfLemon):
                newText = "halv citron";
                break;
            case nameof(tags.Goblin):
                newText = "troll";
                break;
            case nameof(tags.Sallad):
                newText = "sallad";
                break;
            case nameof(tags.ChoppedBatwing):
                newText = "hackad fladdermusvinge";
                break;
            default:
                newText = additionalText.ToLower();
                break;                
        }
        return newText;
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
        //if the array length is greater than 0, it has hit an obstical
        if (hits.Length > 0 && hits[0].transform.tag != "Water")
        {
            //change object position to camera position 
            if (hits[0].collider.tag == "Cauldron")
            {               
                if (heldObj.gameObject.TryGetComponent(out InGameItemTags tags))
                {
                    foreach (var item in tags.Tags)
                    {
                        if (item.TagName == "Plate")
                        {
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
