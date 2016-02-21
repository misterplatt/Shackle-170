﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_NetworkLobbyUI : MonoBehaviour {

    public NetworkManager manager;

    void Awake() {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F12)) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().outputMetrics();
            manager.StopHost();
            manager.StopClient();
        }
    }

    //clearly this shouldn't be used outside of development.
    public void connectLocal() {
        manager.networkAddress = "localhost";
        manager.networkPort = 7777;
        manager.StartClient();
    }

    public void connectLAN() {
        manager.networkAddress = "128.114.52.99";
        manager.networkPort = 7777;
        manager.StartClient();
    }
    public void hostGame() {
        manager.StartHost();
    }
    
    void OnApplicationQuit() {
        manager.StopHost();
    }
}
