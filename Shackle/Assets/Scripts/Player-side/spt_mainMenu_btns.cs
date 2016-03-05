﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class spt_mainMenu_btns : NetworkBehaviour {

	// Use this for initialization
	void Start () {

        GameObject manager = GameObject.Find("NetworkManager");
        spt_NetworkLobbyUI lobbyUI = manager.GetComponent<spt_NetworkLobbyUI>();

        Button local = transform.Find("btn_ConnectLocal").gameObject.GetComponent<Button>();
        Button ip = transform.Find("btn_ConnectIP").gameObject.GetComponent<Button>();
        Button host = transform.Find("btn_Host").gameObject.GetComponent<Button>();

        local.onClick.AddListener(() => { lobbyUI.connectLocal(); });
        ip.onClick.AddListener(() => { lobbyUI.connectLAN(); });
        host.onClick.AddListener(() => { lobbyUI.hostGame(); });
    }
}
