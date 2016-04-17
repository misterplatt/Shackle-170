using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class spt_NetworkDiscovery : MonoBehaviour {

    public NetworkDiscovery discovery;
    const int BUTTON_CAP = 3;

    void Start() {
    }

    void Update() {
        listGames();
    }

    public void startBroadcast() {

        discovery.Initialize();
        discovery.StartAsServer();
    }

    public void listen() {
        discovery.Initialize();
        discovery.StartAsClient();
        listGames();
    }

    void listGames() {
        if (discovery.broadcastsReceived == null) return;
        int gameFound = 1;

        foreach ( KeyValuePair<string, NetworkBroadcastResult> msg in discovery.broadcastsReceived) {
            Debug.Log(msg.Key);
            GameObject.Find("btn_game" + gameFound++).GetComponentInChildren<Text>().text = msg.Key;
            if (gameFound > BUTTON_CAP) break;
        }
        
        for (int index = gameFound; gameFound <= BUTTON_CAP; ++index) {
            GameObject.Find("btn_game" + gameFound++).GetComponentInChildren<Text>().text = "";
        }
    }

}
