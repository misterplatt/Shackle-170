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
using System.Collections;

public class spt_WorldState : NetworkBehaviour {
    public static bool worldStateChanged = false;
}
