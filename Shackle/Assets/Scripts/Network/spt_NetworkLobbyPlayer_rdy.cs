using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_NetworkLobbyPlayer_rdy : NetworkBehaviour
{

	[SyncVar]
    public bool pReady;
    [SyncVar]
    public int pCount;
    NetworkLobbyManager nlm;

    void Start()
    {
        pReady = false;
        pCount = 0;
        nlm = GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>();
    }

    void Update()
    {
        if (isServer)
        {
            pCount = nlm.numPlayers;
            return;
        }

        GameObject[] lPlayers = GameObject.FindGameObjectsWithTag("lobbyPlayer");
        foreach (GameObject lPlayer in lPlayers)
        {
            if (!lPlayer.GetComponent<NetworkIdentity>().isLocalPlayer) pCount = lPlayer.GetComponent<spt_NetworkLobbyPlayer_rdy>().pCount;
        }

        GetComponent<NetworkLobbyPlayer>().readyToBegin = pReady;
    }

    public void toggleReady() {
        pReady = !pReady;

        if (!isServer) Cmd_updateStatus(pReady);
    }
    
    [Command]
    public void Cmd_updateStatus( bool newStatus)
    {
        pReady = newStatus;
    }

}
