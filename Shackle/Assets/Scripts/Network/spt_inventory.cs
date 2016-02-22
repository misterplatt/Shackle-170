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
    private int MAX_SLOTS = 4;

    [SyncVar]
    public bool invChanged = true;

    public int activeItem = 0;
    public float lerpSpeed = 5;
    private int activeSlotNumber = 0;
    private Vector3 startPos;
    private Vector3 endPos;

    [SerializeField] private bool once = false;

    void Start() {
        
        if (!isLocalPlayer)
        {
            invChanged = false;
            return;
        }

        activeItem = 0;
        //initialize Inventory with hand as active object, set slot 1 sprite
        if (isServer)
        {            
            inventory.Add("Hand");
            transform.Find("VRCameraUI/InventorySlot0").gameObject.GetComponent<RawImage>().texture = handSprite;
            reticleUpdate();
            //dbg_addInventory();
        }
        else
        {
            CmdinitSpawn(this.name);
            GameObject.Find(this.name).transform.Find("VRCameraUI/InventorySlot0").gameObject.GetComponent<RawImage>().texture = handSprite;
            reticleUpdate();
        }
        


    }
    
    void Update() {
        if (!isLocalPlayer) return;

        if (invChanged) visualList();
        reticleUpdate();
        
        //cycling controls
        
        if ((spt_playerControls.triggers() == -1 || Input.GetKeyDown(KeyCode.A)) && !once) {

            cycleLeft();
            once = true;
        }
        if ((spt_playerControls.triggers() == 1 || Input.GetKeyDown(KeyCode.D)) && !once) {
            cycleRight();
            once = true;
        }
        if (spt_playerControls.triggers() == 0) once = false; //Prevents multiple cycles in one trigger press
        
        //ryans test stuff because his control does wierd stuff
        if (Input.GetKeyDown(KeyCode.A)) cycleLeft();
        if (Input.GetKeyDown(KeyCode.D))
        {
            cycleRight();
        }
        if (Input.GetKeyDown(KeyCode.N) || Input.GetButtonDown("xButton")) sendItem();
        if (Input.GetKeyDown(KeyCode.E)) dbg_printInventory();
        if (Input.GetKeyDown(KeyCode.F)) dbg_serverPrintInventory();
        if (Input.GetKeyDown(KeyCode.Q)) pickUp(GameObject.Find("mdl_screwDriver"));

    }

    //grab correct game object given the index
    public GameObject retrieveObjectFromInventory(int index) {
        if (index >= inventory.Count || index < 0) {
            Debug.Log("Error : RetrieveObjectFromInventory called with index " + index );
            return null;
        }
        return GameObject.Find(inventory[index]);
    }

    public void reticleUpdate() {        
        reticleTex = transform.Find("VRCameraUI/GUIReticle").gameObject;
        reticleTex.GetComponent<RawImage>().texture = retrieveObjectFromInventory(activeItem).GetComponent<GUITexture>().texture;
    }

    public void visualList() {
        for (int index = 1; index < MAX_SLOTS; ++index ) {
            Debug.Log("Updating Slot : " + index);
            GameObject thisSlot = transform.Find("VRCameraUI/InventorySlot" + index).gameObject;
            GameObject invItem = null;

            if ( index >= inventory.Count )
            {
                thisSlot.GetComponent<RawImage>().texture = none;
                break;
            }
            invItem = retrieveObjectFromInventory(index);
            thisSlot.GetComponent<RawImage>().texture = invItem.GetComponent<GUITexture>().texture;
        } 
    }
    
    public void pickUp ( GameObject item ) {
        if (!isLocalPlayer) return;
        if (inventory.Count == 5) return;
        //should never be null
        if (!isServer) CmdPickUp(item.name, this.name);
        else inventory.Insert(inventory.Count, item.name);

        invChanged = true;
    }

    //wrap c# list.remove() to remove inventory item from synclist
    public void removeItm(string item) {
        if (!isLocalPlayer) return;
        if (item == "Hand") return;

        if (!isServer) CmdRemoveItem(item, this.name);
        else inventory.Remove(item);

        invChanged = true;
    }

    //cycle right in inventory
    void cycleRight() {
        if (!isLocalPlayer) return;
        if (inventory.Count < 0) return;

        if (activeItem < inventory.Count-1) {
            ++activeItem;
            ++activeSlotNumber;
        }
        else {
            //case : loop from end to beginning
            activeItem = 0;
            activeSlotNumber = 0;            
        }
        reticleUpdate();

        //Move selection bar below the new active item
        selectionBar.transform.localPosition = new Vector3(
            transform.Find("VRCameraUI/InventorySlot" + activeSlotNumber).localPosition.x, 
            selectionBar.transform.localPosition.y, 
            selectionBar.transform.localPosition.z );
    }

    //cycle left in inventory
    void cycleLeft() {
        if (!isLocalPlayer) return;
        if (inventory.Count < 0) return;

        if(activeItem > 0) {
            --activeItem;
            --activeSlotNumber;
        }
        else {
            activeItem = inventory.Count - 1;
            activeSlotNumber = inventory.Count - 1;
        }
        reticleUpdate();

        //Move selection bar below the new active item
        selectionBar.transform.localPosition = new Vector3(
        transform.Find("VRCameraUI/InventorySlot" + activeSlotNumber).transform.localPosition.x,
            selectionBar.transform.localPosition.y,
            selectionBar.transform.localPosition.z);
    }

    void sendItem() {
        if (!isLocalPlayer) return;
        if (activeItem == 0) return;
        
        CmdSendItem(transform.gameObject.name, inventory[activeItem]);
        cycleLeft();
        
        invChanged = true;
    }

    public void dbg_serverPrintInventory()
    {
        if (!isLocalPlayer) return;
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

    public void dbg_addInventory()
    {
        if (!isLocalPlayer) return;
        inventory.Add("mdl_screwDriver");
        inventory.Add("mdl_garageOpener");
        inventory.Add("mdl_key");
    //    inventory.Add("mdl_screwDriver");
    }

    public void dbg_printInventory()
    {
        if (!isLocalPlayer) return;
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
        GameObject.Find(pName).transform.Find("VRCameraUI/InventorySlot1").gameObject.GetComponent<RawImage>().texture = handSprite;
    }

    [Command]
    void CmdRemoveItem( string itemName, string pName ) {
        GameObject.Find(pName).GetComponent<spt_inventory>().inventory.Remove(itemName);
    }

    [Command]
    void CmdPickUp( string itemName, string pName ) {
        GameObject.Find(pName).GetComponent<spt_inventory>().inventory.Add(itemName);
    }

    [Command]
    void CmdSendItem( string pGiver, string itemName ) {
        if (itemName == "Hand") return;
        //get players from scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject giver = null;
        GameObject reciever = null;

        //find player object giving and recieving
        foreach (GameObject player in players) {
            if (player.name == pGiver) giver = player;
            else reciever = player;
        }


        if ( reciever.GetComponent<spt_inventory>().inventory.Count == 5 )
        {
            Debug.Log("Tried to pass to full inventory.");
            return;
        }

        giver.GetComponent<spt_inventory>().inventory.Remove(itemName);
        reciever.GetComponent<spt_inventory>().inventory.Add(itemName);
        reciever.GetComponent<spt_inventory>().invChanged = true;
        //set remote invChange to true

    }

    //Used by tooltip listener to see when player has picked up an object.
    public int inventorySize() {
        return inventory.Count;
    }

}