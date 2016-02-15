using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using VRStandardAssets.Utils;

public class spt_inventory : NetworkBehaviour {
    [SerializeField] SyncListString inventory = new SyncListString();

    //inventory stuff
    public GameObject selectionBar;
    public GameObject handObj;
    public GameObject reticleTex;
    public Texture handSprite;
    public Texture none;

    [SyncVar]
    public bool invChanged = true;

    public int activeItem = -1;
    public float lerpSpeed = 5;
    private int activeSlotNumber = 1;
    private Vector3 startPos;
    private Vector3 endPos;

    [SerializeField] private bool once = false;

    void Start() {
        
        if (!isLocalPlayer)
        {
            invChanged = false;
            return;
        }


        //initialize Inventory with hand as active object, set slot 1 sprite
        if (isServer)
        {
            activeItem = 0;
            inventory.Add("Hand");
            transform.Find("VRCameraUI/InventorySlot1").gameObject.GetComponent<RawImage>().texture = handSprite;
            reticleUpdate();
        }
        else
        {
            CmdinitSpawn(this.name);
            transform.Find("VRCameraUI/InventorySlot1").gameObject.GetComponent<RawImage>().texture = handSprite;
            reticleUpdate();
        }


        if (isServer) pickUp(GameObject.Find("mdl_screwDriver"));
    }
    
    void Update() {
        if (!isLocalPlayer) return;
        if (invChanged) Debug.Log("Inv Change");
        if (invChanged) visualList();
        //cycling controls
        /*
        if ((spt_playerControls.triggers() == -1 || Input.GetKeyDown(KeyCode.A)) && !once) {
            Debug.Log("Test");
            cycleLeft();
            once = true;
        }
        if ((spt_playerControls.triggers() == 1 || Input.GetKeyDown(KeyCode.D)) && !once) {
            cycleRight();
            once = true;
        }
        if (spt_playerControls.triggers() == 0) once = false; //Prevents multiple cycles in one trigger press
        */
        //ryans test stuff because his control does wierd stuff
        if (Input.GetKeyDown(KeyCode.A)) cycleLeft();
        if (Input.GetKeyDown(KeyCode.D)) cycleRight();
        if (Input.GetKeyDown(KeyCode.N)) sendItem();
        if (Input.GetKeyDown(KeyCode.E)) dbg_printInventory();
        if (Input.GetKeyDown(KeyCode.F)) dbg_serverPrintInventory();
        if (Input.GetKeyDown(KeyCode.Q)) pickUp(GameObject.Find("mdl_screwDriver"));

    }

    //grab correct game object given the index
    public GameObject retrieveObjectFromInventory(int index) {
        return GameObject.Find(inventory[index]);
    }

    public void reticleUpdate() {
        reticleTex = GameObject.Find("GUIReticle");
        Debug.Log(retrieveObjectFromInventory(activeItem));
        reticleTex.GetComponent<RawImage>().texture = retrieveObjectFromInventory(activeItem).GetComponent<GUITexture>().texture;
    }

    public void visualList() {
        int slotNumber = 0;
        //Debug.Log("Visualizing List...");
        //run over newly modified list, setting inventory textures according to new list positions. remove trailing tex
        for (int index = 0; index <= inventory.Count; ++index) {
            ++slotNumber;
            if (index == inventory.Count)
            {
                Debug.Log("Clear Slot Hit");
                transform.Find("VRCameraUI/InventorySlot" + slotNumber).gameObject.GetComponent<RawImage>().texture = none;
                break;
            }
            
            GameObject thisObject = retrieveObjectFromInventory(index);

            //skip hand
            if (slotNumber == 1) continue;

            //Otherwise, set the UI Slot's texture to i's Value's texture (the Texture on the objects's GUI Texture opponent)
            transform.Find("VRCameraUI/InventorySlot" + slotNumber).gameObject.GetComponent<RawImage>().texture = thisObject.GetComponent<GUITexture>().texture;
        }
        invChanged = false;
    }

    public void pickUp ( GameObject item ) {
        //should never be null
        inventory.Insert( inventory.Count, item.name );
        invChanged = true;
    }

    //wrap c# list.remove() to remove inventory item from synclist
    public void removeItm(string item) {
        if (item == "Hand") return;
        inventory.Remove(item);
        invChanged = true;
    }

    //cycleright in inventory
    void cycleRight() {
        if (inventory.Count == 0) return;

        if(activeItem < inventory.Count-1) {
            ++activeItem;
            ++activeSlotNumber;
        }
        else {
            //case : loop from end to beginning
            activeItem = 0;
            activeSlotNumber = 1;            
        }
        reticleUpdate();

        //Move selection bar below the new active item
        selectionBar.transform.localPosition = new Vector3(
            GameObject.Find("InventorySlot" + activeSlotNumber).transform.localPosition.x, 
            selectionBar.transform.localPosition.y, 
            selectionBar.transform.localPosition.z );
    }

    void cycleLeft() {
        if (inventory.Count == 0) return;

        if(activeItem > 0) {
            --activeItem;
            --activeSlotNumber;
        }
        else {
            activeItem = inventory.Count - 1;
            activeSlotNumber = inventory.Count;
        }
        reticleUpdate();

        //Move selection bar below the new active item
        selectionBar.transform.localPosition = new Vector3(
        GameObject.Find("InventorySlot" + activeSlotNumber).transform.localPosition.x,
            selectionBar.transform.localPosition.y,
            selectionBar.transform.localPosition.z);
    }

    void sendItem() {
        CmdSendItem(transform.gameObject.name, inventory[activeItem]);
        cycleLeft();
        invChanged = true;
    }

    public void dbg_serverPrintInventory()
    {
        if (!isServer) return;
        int slot = 1;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach ( GameObject player in players )
        {
            Debug.Log( "Player : " + player.name );
            spt_inventory thisInv = player.GetComponent<spt_inventory>();
            if (thisInv.inventory.Count == 0) Debug.Log("Empty");
            for (int index = 0; index < thisInv.inventory.Count; ++index)
            {
                Debug.Log( player.name + " : Slot " + slot + " : " + thisInv.inventory[index]);

            }
            slot = 1;
        }
    }

    public void dbg_printInventory()
    {
        int slot = 1;

        foreach (string item in inventory)
        {
            Debug.Log("Slot " + slot++ + " : " + item);
        }
    }

    [Command]
    void CmdinitSpawn(string pName)
    {
        Debug.Log(pName + " has connected.");

        GameObject.Find(pName).GetComponent<spt_inventory>().inventory.Add("Hand");
    }

    [Command]
    void CmdSendItem( string pGiver, string itemName ) {
        if (itemName == "Hand") return;
        Debug.Log("Sending : " + itemName);
        //get players from scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject giver = null;
        GameObject reciever = null;

        //find player object giving and recieving
        foreach (GameObject player in players) {
            if (player.name == pGiver) giver = player;
            else reciever = player;
        }

        //remove object from giver
        giver.GetComponent<spt_inventory>().removeItm(itemName);

        //give object to reciever
        spt_inventory recInventory = reciever.GetComponent<spt_inventory>();
        recInventory.pickUp(GameObject.Find(itemName));
        

        //set remote invChange to true

    }

}