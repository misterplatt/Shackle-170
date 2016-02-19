using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_player_NetworkPuzzleLogic : NetworkBehaviour {

	void Update() {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.K)) {
        }
    }
    
}
