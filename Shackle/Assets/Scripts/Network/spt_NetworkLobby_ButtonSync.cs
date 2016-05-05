using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections;

public class spt_NetworkLobby_ButtonSync : NetworkBehaviour {

    //connection status syncing
    [SyncVar]
    public bool p1Connected;

    [SyncVar]
    public bool p2Connected;

    //PlayerReady Syncing
    [SyncVar]
    public bool p1Ready;

    [SyncVar]
    public bool p2Ready;

    //local scope
    Button p1Rdy;
    Button p2Rdy;
    spt_NetworkLobby_ButtonSync_Client clientLobbyUI;
    NetworkManager manager;
    public string selectedLevel;

    public int pCount;

	// Use this for initialization
	void Start () {
        p1Connected = true;
        p2Connected = false;
        p1Ready = false;
        p2Ready = false;
        pCount = 0;

        p1Rdy = this.transform.Find("P1_readystate").gameObject.GetComponent<Button>();
        p2Rdy = this.transform.Find("P2_readystate").gameObject.GetComponent<Button>();
        clientLobbyUI = GameObject.Find("Client_UI").GetComponent<spt_NetworkLobby_ButtonSync_Client>();
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        selectedLevel = "";
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(this.transform.root.gameObject.name + " : " + p1Connected + " | " + p2Connected +  " | " + p1Ready + " | " + p2Ready);        
        if (SceneManager.GetActiveScene().name != "net_playerlobby") return;
        updateButtons();
        levelTransitionCheck();
        //check number of players connected and update rdy as expected
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != pCount) updatePConnections( players.Length );

        //update readiness      
        foreach (GameObject player in players) {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer) {
                if (player.GetComponent<spt_LobbyPlayer>().isReady)
                {
                    p1Ready = true;
                }
                else
                {
                    p1Ready = false;
                }
            }
            else {
                if (player.GetComponent<spt_LobbyPlayer>().isReady)
                {
                    p2Ready = true;
                }
                else
                {
                    p2Ready = false;
                }
            }
        }
	}
    
    void updatePConnections( int newCount ) {
        if (newCount == 1)
        {
            p1Connected = true;
        }
        if (newCount >= 2)
        {
            p2Connected = true;
        }
        else
        {
            p2Connected = false;
        }

        pCount = newCount;
    }

    void updateButtons() {
        ColorBlock p1cb = p1Rdy.colors;
        ColorBlock p2cb = p2Rdy.colors;

        if (p1Connected) p1cb.disabledColor = Color.yellow;
        else p1cb.disabledColor = Color.grey;
        if (p2Connected) p2cb.disabledColor = Color.yellow;
        else p2cb.disabledColor = Color.grey;

        if (p1Ready && p1Connected) {
            p1cb.disabledColor = Color.green;
        }
        if (p2Ready && p2Connected) p2cb.disabledColor = Color.green;

        p1Rdy.colors = p1cb;
        p2Rdy.colors = p2cb;
    }

    void levelTransitionCheck() {
        Debug.Log(selectedLevel);
        if ((!( p1Ready && p2Ready )) || selectedLevel == "") return;

        manager.ServerChangeScene(selectedLevel);
    }


}
