/* spt_inventory.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 5/5/2016
 * 
 * This file provides the network based implementation of the player inventory.
 * this inventory is synched between client and hosts and supports two distinct inventories, as well as 
 * inventory interaction in the way of passing, grabbing, and removing items.
 * Added beginnings of reticle range - Dara
 * Added fisting for ranger outpost drawer - Dara
*/


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using VRStandardAssets.Utils;

public class spt_inventory : NetworkBehaviour {
    //The inventory is stored as a synchronized list of string references to scene objects
    [SerializeField] public SyncListString inventory = new SyncListString();

    //UI components
    public GameObject selectionBar;
    public GameObject handObj;
    public GameObject reticleTex;
    public Texture handSprite;
    public Texture fistSprite;
    public Texture unreachableSprite;
    public Texture none;
    private int MAX_SLOTS = 4;
    private VRInteractiveItem lookingObject;

    [SerializeField] private VREyeRaycaster m_EyeRaycaster;
    //A sync variable which dictates if the inventory UI should update
    [SyncVar]
    public bool invChanged = true;

    //LERP and information for inventory icon positions.
    public int activeItem = 0;
    public float lerpSpeed = 5;
    private int activeSlotNumber = 0;
    private Vector3 startPos;
    private Vector3 endPos;

    //flag to prevent duplicated input on button down
    [SerializeField] private bool once = false;

    void Start() {
        
        //if not the local player, don't do anything it won't matter.
        if (!isLocalPlayer)
        {
            invChanged = false;
            return;
        }

        activeItem = 0;
        //initialize Inventory with hand as active object, set slot 1 sprite
        if (isServer)
        {            
            //if we're the server, simply assign textures and add hand item
            inventory.Add("Hand");
            transform.Find("Camera Player/VRCameraUI/InventorySlot0").gameObject.GetComponent<RawImage>().texture = handSprite;
            reticleUpdate();
            //dbg_addInventory();
        }
        else
        {
            //if we're the client, tell the server to initialize our object.
            CmdinitSpawn(this.name);
            GameObject.Find(this.name).transform.Find("Camera Player/VRCameraUI/InventorySlot0").gameObject.GetComponent<RawImage>().texture = handSprite;
            reticleUpdate();
        }
        


    }
    
    void Update() {

        //if not the local player, don't do anything as it's meaningless
        if (!isLocalPlayer) return;

        lookingObject = m_EyeRaycaster.CurrentInteractible;
        

        //if the inventory has changed in someway, redraw the inventory UI.
        if (invChanged) visualList();
        //update the reticle to indicate the currently selected item
        if (activeItem == 0 && spt_playerControls.aButtonPressed())
        {
            Fisting();
        }
        else reticleUpdate();
        
        //use the triggers as cycle controls through the inventory, but only allow them to register once.
        if ((spt_playerControls.triggers() == -1 || Input.GetKey(KeyCode.A)) && !once) {
            cycleLeft();
            once = true;
        }
        if ((spt_playerControls.triggers() == 1 || Input.GetKey(KeyCode.D)) && !once) {
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
        if (Input.GetKeyDown(KeyCode.Q)) Fisting();

        /*
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("yButton")) inspecting = !inspecting;
        if (inspecting) inspectItem();
        else stopInspectItem();
        */
        
    }

    //grab correct game object from the scene by inventory string reference given the index.
    public GameObject retrieveObjectFromInventory(int index) {
        if (index >= inventory.Count || index < 0) {
            Debug.Log("Error : RetrieveObjectFromInventory called with index " + index );
            return null;
        }
        return GameObject.Find(inventory[index]);
    }

    //change the reticle texture based on the current active inventory item.
    public void reticleUpdate() {

        if (m_EyeRaycaster.racyCastTouch)
        {
            reticleTex = transform.Find("Camera Player/VRCameraUI/GUIReticle").gameObject;
            reticleTex.GetComponent<RawImage>().texture = retrieveObjectFromInventory(activeItem).GetComponent<GUITexture>().texture;
        }
        else
        {
            reticleTex.GetComponent<RawImage>().texture = unreachableSprite;
        }

    }

    //iterate through the synced inventory string list and update the UI icons with gameobject icons, remove the icons from items which no longer exist
    public void visualList() {
        //for any inventory item beyond the hand.
        for (int index = 1; index < MAX_SLOTS; ++index ) {
            //get the gameobject storying the UI icon
            GameObject thisSlot = transform.Find("Camera Player/VRCameraUI/InventorySlot" + index).gameObject;
            GameObject invItem = null;

            //once we've gottent o UI icons beyond the inventory we actually have, just remove their texture and return. We've passed an item most likely.
            if ( index >= inventory.Count )
            {
                thisSlot.GetComponent<RawImage>().texture = none;
                break;
            }
            //otherwise get the current gameobject whose name is stored at this inventory index
            //then populate the UI texture appropriately.
            invItem = retrieveObjectFromInventory(index);
            thisSlot.GetComponent<RawImage>().texture = invItem.GetComponent<GUITexture>().texture;
        } 
    }
    
    //pickup adds a new object to the inventory list.
    public void pickUp ( GameObject item ) {
        //if we're not the local player, or our inventory is full, don't do anything. 
        if (!isLocalPlayer) return;
        if (inventory.Count == 5) return;

        //if we're not the server, ask the server to pick up the item for us and update our list.
        //if we our the server, like shia said, JUST DO IT.
        if (!isServer) CmdPickUp(item.name, this.name);
        else inventory.Insert(inventory.Count, item.name);

        //note the inventory has changed so the UI will update.
        invChanged = true;
    }

    //wrap c# list.remove() to remove inventory item from synclist
    public void removeItm(string item) {
        if (!isLocalPlayer) return;
        if (item == "Hand") return;

        if (!isServer) CmdRemoveItem(item, this.name);
        else inventory.Remove(item);

		cycleLeft ();
        invChanged = true;
    }

    //cycle right in inventory
    void cycleRight() {
        if (!isLocalPlayer) return;
        if (inventory.Count < 0) return;

        //if we're not at the end of the list iterate
        if (activeItem < inventory.Count-1) {
            ++activeItem;
            ++activeSlotNumber;
        }
        else {
            //case : loop from end to beginning if we're at the end of the list.
            activeItem = 0;
            activeSlotNumber = 0;            
        }
        reticleUpdate();

        //Move selection bar below the new active item
        selectionBar.transform.localPosition = new Vector3(
            transform.Find("Camera Player/VRCameraUI/InventorySlot" + activeSlotNumber).localPosition.x, 
            selectionBar.transform.localPosition.y, 
            selectionBar.transform.localPosition.z );
    }

    //cycle left in inventory
    //logically equivilant to above algorithm, see notes above.
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
        transform.Find("Camera Player/VRCameraUI/InventorySlot" + activeSlotNumber).transform.localPosition.x,
            selectionBar.transform.localPosition.y,
            selectionBar.transform.localPosition.z);
    }

    /*void inspectItem() {
        GameObject invItem = retrieveObjectFromInventory(activeItem);
        if (invItem.name == "Hand") {
            inspecting = false;
            return;
        }
        Transform endPoint = transform.Find("Camera Player/VRCameraUI/InspectPoint");
        bool outOfView = false;

        if (Vector3.Distance(invItem.transform.position, endPoint.position) > .6f) outOfView = true;
        if (outOfView == true) invItem.transform.position = Vector3.Lerp(invItem.transform.position, endPoint.position, Time.deltaTime * lerpSpeed);
        if (Vector3.Distance(invItem.transform.position, endPoint.position) < .02f) outOfView = false;

        invItem.transform.Rotate(new Vector3(spt_playerControls.rightThumb("Vertical"), spt_playerControls.rightThumb("Horizontal"), 0) * Time.deltaTime * 90, Space.World);
    }

    void stopInspectItem() {
        GameObject invItem = retrieveObjectFromInventory(activeItem);
        if (invItem.name == "Hand") return;
        invItem.transform.position = Vector3.down * 10;
    }*/

    void sendItem() {
        if (!isLocalPlayer) return;
        if (activeItem == 0) return;
        
        //ask server to give my selected object to the other player instance then cycle left.
        CmdSendItem(transform.gameObject.name, inventory[activeItem]);
        cycleLeft();
        
        invChanged = true;
    }

    //Debug command which prints all player inventories to console.
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

    //debug command which adds all inventory objects from garage scene to player inventory
    public void dbg_addInventory()
    {
        if (!isLocalPlayer) return;
        inventory.Add("mdl_screwDriver");
        inventory.Add("mdl_garageOpener");
        inventory.Add("mdl_key");
    }

    //debug command printing local inventory
    public void dbg_printInventory()
    {
        if (!isLocalPlayer) return;
        int slot = 1;

        foreach (string item in inventory)
        {
            Debug.Log("Slot " + slot++ + " : " + item);
        }
    }

    //Server Commands....

    //initSpawn sets up inventory initial conditions, adds hand object and sets UI texture properly
    [Command]
    void CmdinitSpawn(string pName)
    {
        Debug.Log(pName + " has connected.");

        GameObject.Find(pName).GetComponent<spt_inventory>().inventory.Add("Hand");
        GameObject.Find(pName).transform.Find("Camera Player/VRCameraUI/InventorySlot1").gameObject.GetComponent<RawImage>().texture = handSprite;
    }

    //remove item removes the given item from the inventory on the server by string reference
    [Command]
    void CmdRemoveItem( string itemName, string pName ) {
        GameObject.Find(pName).GetComponent<spt_inventory>().inventory.Remove(itemName);
    }

    //adds an item to player inventory by string name on server
    [Command]
    void CmdPickUp( string itemName, string pName ) {
        GameObject.Find(pName).GetComponent<spt_inventory>().inventory.Add(itemName);
    }

    //Command to remove item from giver inventory and add to reciever inventory. Only works if reciever is not full.
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


    public void Fisting()
    {
        if (lookingObject != null && (lookingObject.GetComponent<VRStandardAssets.Examples.spt_interactiveMovable>() != null || lookingObject.GetComponent<VRStandardAssets.Examples.spt_mirrorHandle>() != null || lookingObject.GetComponent<VRStandardAssets.Examples.spt_drawer>() != null))
        {
                reticleTex.GetComponent<RawImage>().texture = fistSprite;
        }
    }
}