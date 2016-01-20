using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class spt_Network_Inventory : NetworkBehaviour {
    [SerializeField]
    List<string> playerInventory;

    //use this to initialize server side inventory variables
    void FixedUpdate()
    {

    }

    [Command]
    void CmdProvidePlayerInventoryToServer ( List<string> pInv )
    {

    }
}
