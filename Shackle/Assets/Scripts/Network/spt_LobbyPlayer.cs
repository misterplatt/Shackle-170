using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class spt_LobbyPlayer : NetworkBehaviour {

    [SyncVar]
    public bool isReady;
    bool once = false;

	// Use this for initialization
	void Start () {
        isReady = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name != "net_playerlobby") return;
        if (!isLocalPlayer) return;

        if (spt_playerControls.startButtonPressed() && !once) {
            once = !once;
            if (isServer) isReady = !isReady;
            else Cmd_updateReady();
        }
        else {
            once = !once;
        }
	}

    [Command]
    public void Cmd_updateReady() {
        isReady = !isReady;
    }
}
