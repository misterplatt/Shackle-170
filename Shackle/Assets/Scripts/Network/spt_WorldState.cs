/* spt_NetworkLobbyUI.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This file stores simple world statics so puzzle state checking can occur with ease. 
*/


using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class spt_WorldState : NetworkBehaviour {
    public static bool worldStateChanged = false;

    [SyncVar]
    public bool playCrashSound = false;
    public string localPlayer;
    public bool retryInv = false;

    void Start()
    {
        worldStateChanged = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if ( player.GetComponent<NetworkIdentity>().isLocalPlayer )
            {
                localPlayer = player.name;
            }
        }
    }

    void Update()
    {
        if (retryInv)
        {
            retryInvSetup();
        }
    }

    public void retryInvSetup()
    {
        //initialize the player 2 inventory if for some reason it has previously failed.
        GameObject player = GameObject.Find("Player 2");
        if (player == null) return;

        player.GetComponent<spt_inventory>().inventory.Add("Hand");
        player.transform.Find("Camera Player/VRCameraUI/InventorySlot1").gameObject.GetComponent<RawImage>().texture = player.GetComponent<spt_inventory>().handSprite;
        retryInv = false;
    }
}
