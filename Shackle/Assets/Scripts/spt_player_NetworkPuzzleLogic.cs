using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_player_NetworkPuzzleLogic : NetworkBehaviour {

	void Update() {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.K)) {
            Cmd_UpdatePuzzleLogic("EventA", true, "EventA_itm");
        }
    }
    
    [Command]
    public void Cmd_UpdatePuzzleLogic(string name, bool state, string itmName) {
        GameObject pLogic = NetworkServer.FindLocalObject( GameObject.Find("PuzzleLogic").GetComponent<NetworkIdentity>().netId );
        spt_NetworkPuzzleLogic logScript = pLogic.GetComponent<spt_NetworkPuzzleLogic>();
        logScript.updatePuzzleState(name, state, itmName);
    }
}
