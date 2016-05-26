using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class spt_LobbyPlayer : NetworkBehaviour {

    [SyncVar]
    public bool isReady;

	// Use this for initialization and player spawn position
	void Start () {
        isReady = false;
        Debug.Log(GameObject.Find("NetworkManager").GetComponent<NetworkManager>().startPositions.ToString() );
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name != "net_playerlobby") return;
        if (!isLocalPlayer) return;
        
        if (spt_playerControls.startButtonPressed()) {
            isReady = !isReady;
            if (!isServer) Cmd_updateReady(isReady);
        }
	}

    //Game Loop Functions
    public void quitLevel() {
        if (!isServer) return;
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StopHost();
    }

    public void restartLevel() {
        if (!isServer) return;
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene("LoadScreen");
    }

    [Command]
    public void Cmd_updateReady( bool rdy ) {
        isReady = rdy;

    }

    //Game Loop Commands
    [Command]
    public void Cmd_restartLevel() {
        restartLevel();
    }

    [Command]
    public void Cmd_quit() {
        quitLevel();
    }
}
