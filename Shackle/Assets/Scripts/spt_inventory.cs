using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class spt_inventory : NetworkBehaviour {

    [SyncVar]
    public static LinkedList<GameObject> inventory;

    public LinkedListNode<GameObject> activeItem;
    public GameObject object1;

    //flag to trigger server sync on inventory change
    bool invChange = false;

    //Network Vars

    // Use this for initialization
    void Start () {
        activeItem = new LinkedListNode<GameObject>(object1);
        inventory = new LinkedList<GameObject>();
        inventory.AddLast(activeItem);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(activeItem.Value);
        }
        if (Input.GetKeyDown(KeyCode.R)){
            pickUp(GameObject.Find("Remote"));
            pickUp(GameObject.Find("Black Tar Heroin"));
        }
        if (Input.GetKeyDown(KeyCode.D)){
            cycleRight();
        }
        if (Input.GetKeyDown(KeyCode.A)){
            cycleLeft();
        }
    }

    public void pickUp(GameObject item) {
        inventory.AddLast(item);
        invChange = true;
    }

    void cycleRight() {
        if (inventory.Count == 0) return;
        if (activeItem.Next != null) {
           Debug.Log("Moving right");
           activeItem = activeItem.Next;
        } else {
            Debug.Log("Looping");
            activeItem = inventory.First;
        }
    }

    void cycleLeft(){
        if (inventory.Count == 0) return;
        if (activeItem.Previous != null){
            Debug.Log("Moving left");
            activeItem = activeItem.Previous;
        } else {
            Debug.Log("Looping");
            activeItem = inventory.Last;
        }
    }

    void sendItem() {
        //activeItem is removed from local list
        //previousItem is set as new active
        //call to the server is made to sending object name
        //to other client to find
        //the object and add it to their inventory
        //do an inventorysync
    }

    //Network Functions

    //CmdProvideInventoryToServer syncs local player inventory to the server copy of 
    //varable. This is temporarily using a list of GameObjects to test performance impact.
    [Command]
    void CmdProvideInventoryToServer( LinkedList<GameObject> pInv ) {
        inventory = pInv;
    }

    [ClientCallback]
    void transmitInventory() {
        if (isLocalPlayer)
        {
            //Additional conditions for actual transmission.
            if (invChange)
            {
                //update serverside inventory then clear inv change flag
                CmdProvideInventoryToServer(inventory);
                invChange = false;
            }
        }
    }
}
