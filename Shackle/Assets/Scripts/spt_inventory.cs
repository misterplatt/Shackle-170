﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using VRStandardAssets.Utils;

public class spt_inventory : NetworkBehaviour {

    [SerializeField]
    public LinkedList<GameObject> inventory;
    [SerializeField]
    private bool invChanged;

    public GameObject handObj;
    public Texture handSprite;

    public LinkedListNode<GameObject> activeItem;
    
    private GameObject itemSprite;
    private GameObject itemSelected;
    private GameObject selector;
    
    public float lerpSpeed = 5;
    private Vector3 startPos;
    private Vector3 endPos;

    // Use this for initialization
    void Start () {
        //activeItem = null;//new LinkedListNode<GameObject>(object1);

        //Initialize inventory with hand as active object, set slot 1 sprite as well
        inventory = new LinkedList<GameObject>();
        activeItem = new LinkedListNode<GameObject>(handObj);
        inventory.AddLast(activeItem);
        GameObject.Find("InventorySlot1").GetComponent<RawImage>().texture = handSprite;

        //inventorySpriteOn(item.name);
        //inventorySelectionOn(item.name);
        //selector = GameObject.Find(item.name + "Sel");
    }
	
	// Update is called once per frame
	void Update () {

        if(invChanged)
        {
            TransmitInventory();
        }
        //Debug.Log(activeItem.Value);
        //control section
        //if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(activeItem.Value);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
           pickUp(GameObject.Find("Remote"));
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            removeItm("Remote");
        }
        if (spt_playerControls.triggers() == -1 || Input.GetKeyDown(KeyCode.A))
        {
            cycleLeft();
        }
        if (spt_playerControls.triggers() == 1 || Input.GetKeyDown(KeyCode.D))
        {
            cycleRight();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            invChanged = true;
            TransmitInventory();
        }

        //for debug inv printing
        if (Input.GetKeyDown(KeyCode.M) )
        {
            DebugServerPntInv();
        }
        if (Input.GetKeyDown(KeyCode.N) )
        {
            //DebugServerPntPlayers();
            sendItem();
        }
        if (Input.GetKeyDown(KeyCode.B) )
        {
            Debug.Log("Inventory Report : " + createInvMsg());
        }
    }

    public void pickUp(GameObject item) {
        if (activeItem == null)
        {
            activeItem = new LinkedListNode<GameObject>(item);
            inventory.AddLast(activeItem);
            inventorySpriteOn(item.name);
            inventorySelectionOn(item.name);
            selector = GameObject.Find(item.name + "Sel");
            Debug.Log(item);
        }
        else
        {
            inventory.AddLast(item);
            //inventorySpriteOn(item.name);
            invChanged = true;
            visualizeList();
        }
        
    }

    //Function to update the visual UI representation of the invetory list. Called on pickup and remove.
    public void visualizeList() {
        int slotNumber = 0; //Which slot to change
        //Run over the newly modified list, setting inventory Textures according to new list positions, remove any trailing textures
        for (LinkedListNode<GameObject> i = inventory.First; i != null; i = i.Next) {
            Debug.Log("Visualizing...");
            slotNumber++; //Increment slot number
            if (slotNumber == 1) continue; //If it's the hand, skip it
            if (i.Next == null) GameObject.Find("InventorySlot" + (slotNumber + 1)).GetComponent<RawImage>().texture = null;
            //Otherwise, set the UI Slot's texture to i's Value's texture
            Debug.Log("Changing slot " + slotNumber + " texture");
            GameObject.Find("InventorySlot" + slotNumber).GetComponent<RawImage>().texture = i.Value.GetComponent<GUITexture>().texture;
        }
    }

    //remove item from inv linked list. 
    //support function for sending items.
    public void removeItm(string item)
    {
        for (LinkedListNode<GameObject> iter = inventory.First; iter != null; iter = iter.Next)
        {
            if (iter.Value.name == item) inventory.Remove(iter.Value);
        }
        visualizeList();
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

    //in order to do this we need to differentiate between player instances
    // perhaps if we post a tag on connection to note which is player and which is not.
    void sendItem() {
        CmdSendItem(transform.gameObject.name, "Black Tar Heroin");
        //inventory.Remove(activeItem.Value);
        inventory.Remove(GameObject.Find("Black Tar Heroin"));
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

    [Command]
    void CmdSendItem( string pGiver, string itemName )
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("pc");
        GameObject giver = null;
        GameObject reciever = null;

        foreach (GameObject player in players) {
            if (player.name == pGiver) giver = player;
            else reciever = player;
        }

        //Delete itemName from giver
        giver.GetComponent<spt_inventory>().removeItm(itemName);
        reciever.GetComponent<spt_inventory>().pickUp(GameObject.Find(itemName));

        //give itemName to reciever

        //force sync

    }

    void translateNetworkInvMsg( string invMsg )
    {
        string[] itemsNames = invMsg.Split( new string[] { "," }, StringSplitOptions.RemoveEmptyEntries );

        for (int index = 0; index < itemsNames.Length; ++index )
        {
            inventory.AddLast(GameObject.Find( itemsNames[index]));
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

    [Client]
    void TransmitInventory() {
        if (isLocalPlayer) {
            //If an inventory has been changed, time to perform a serverside sync.
            if ( invChanged ) {
                CmdProvideInventoryToServer( createInvMsg() );
                invChanged = false;
            }
        }
    }

    void DebugServerPntInv() {
        if (!isServer) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("pc");

        for ( int index = 0; index < players.Length; ++index )
        {
            Debug.Log("Player " + index + ": " + (isLocalPlayer ? "Local" : "NonLocal") + "\n" );
            spt_inventory pInv = players[index].GetComponent<spt_inventory>();
            Debug.Log("Inv : " + pInv.createInvMsg() + "\n");
        }

    }

    void DebugServerPntPlayers() {
        if (!isServer) return;

        foreach (KeyValuePair<NetworkInstanceId, NetworkIdentity> pair in NetworkServer.objects) {
            Debug.Log(pair.Key.ToString() );
            Debug.Log(pair.Value.ToString());

        }
        
        /*
        foreach (NetworkConnection player in NetworkServer.connections) {
            if (player == null) continue;
            Debug.Log("Player : ID : " + player.connectionId + ", IP :" + player.address + " Connected? " + player.isReady);
            
            foreach ( NetworkInstanceId id in player.clientOwnedObjects ) {
                Debug.Log( "ID : " + id.ToString() );
                Debug.Log("Value : " + id.Value );
            }
            
        }
        */
    }


    //Takes in the gameObject's name and searches for the sprite that corresponds to it and enables the rawImage so the inventory sprite will show
    public void inventorySpriteOn(string itemName)
    {
        itemSprite = GameObject.Find(itemName + "Spr");
        itemSprite.GetComponent<RawImage>().enabled = true;
    }

    public void inventorySelectionOn(string itemName)
    {
        itemSelected = GameObject.Find(itemName);
        selector = GameObject.Find("Selector");
        selector.transform.position = Vector3.Lerp(selector.transform.position, itemSelected.transform.position, Time.deltaTime * lerpSpeed);
        selector.GetComponent<RawImage>().enabled = true;
    }


    //Takes in the gameObject's name and searches for the sprite that corresponds to it and disables the rawImage so the inventory sprite will dissappear
    public void inventorySpriteOff(string itemName)
    {
        itemSprite = GameObject.Find(itemName + "Spr");
        itemSprite.GetComponent<RawImage>().enabled = false;
    }

}

