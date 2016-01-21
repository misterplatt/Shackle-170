using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class spt_inventory : NetworkBehaviour {

    [SerializeField]
    public LinkedList<GameObject> inventory;
    [SerializeField]
    private bool invChanged;


    public LinkedListNode<GameObject> activeItem;
    public GameObject object1;

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

    //Server command accepts a string representation of
    //Items (**which are assumed to exist in scene**)
    //This could be made faster by using a central hash of items
    //and referencing using integers in this msg, but this 
    //is satisfactory.
    [Command]
    void CmdProvideInventoryToServer ( string invMsg ) {
        inventory = new LinkedList<GameObject>();
        translateNetworkInvMsg(invMsg);
    }

    void translateNetworkInvMsg( string invMsg )
    {
        string[] itemsNames = invMsg.Split( new string[] { "," }, StringSplitOptions.RemoveEmptyEntries );

        for (int index = 0; index < itemsNames.Length; ++index )
        {
            inventory.AddLast( GameObject.Find( itemsNames[index] ) );
        }
    }

    string createInvMsg()
    {
        string msg = "";

        for (LinkedListNode<GameObject> iter = inventory.First; iter != null; iter = iter.Next) {
            msg += iter.Value.name;
            if ( iter.Next != null ){ msg += ","; }
        }

        return msg;
    }

    [ClientCallback]
    void TransmitInventory() {
        if (isLocalPlayer) {
            //If an inventory has been changed, time to perform a serverside sync.
            if ( invChanged ) {
                CmdProvideInventoryToServer( createInvMsg() );
                invChanged = false;
            }
        }
    }
	
}

