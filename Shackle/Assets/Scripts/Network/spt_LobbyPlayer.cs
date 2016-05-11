﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class spt_LobbyPlayer : NetworkBehaviour {

    [SyncVar]
    public bool isReady;

	// Use this for initialization
	void Start () {
        isReady = false;
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

    [Command]
    public void Cmd_updateReady( bool rdy ) {
        isReady = rdy;
    }
}
