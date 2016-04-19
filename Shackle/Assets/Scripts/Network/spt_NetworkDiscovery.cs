/* spt_NetworkDiscovery.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This file Allows for network based discovery of games.
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class spt_NetworkDiscovery : NetworkBehaviour {

    public NetworkDiscovery discovery;
    private string gameName;
    private string ip;
    private string[] ipList;
    private bool inLobby;

    const int BUTTON_CAP = 3;

    void Start() {
        inLobby = false;
        ipList = new string[4];
    }

    void Update() {
        //if (!inLobby)
        listGames();
    }    

    public void startBroadcast() {
        discovery.Initialize();
        discovery.broadcastData = gameName;
        discovery.StartAsServer();
    }

    public void listen() {
        discovery.Initialize();
        discovery.StartAsClient();
        listGames();
    }

    public void stopAllDiscovery() {
        discovery.StopBroadcast();
    }

    public void setGameName(string name) {
        gameName = name;
    }

    void listGames() {
        if (discovery.broadcastsReceived == null) {
            clearList();
            return;
        }

        int gameFound = 1;

        foreach ( KeyValuePair<string, NetworkBroadcastResult> msg in discovery.broadcastsReceived) {
            GameObject.Find("btn_game" + gameFound).GetComponentInChildren<Text>().text = decodeData(msg.Value) + " at " + decodeMsg(msg.Key);
            ipList[gameFound++] = decodeMsg(msg.Key);
            if (gameFound > BUTTON_CAP) break;
        }
        
        for (int index = gameFound; gameFound <= BUTTON_CAP; ++index) {
            GameObject.Find("btn_game" + gameFound++).GetComponentInChildren<Text>().text = "";
            ipList[index] = "";
        }
    }

    private void clearList() {
        for (int index = 1; index <= BUTTON_CAP; ++index) {
            GameObject.Find("btn_game" + index).GetComponentInChildren<Text>().text = "";
            ipList[index] = "";
        }
    }

    private string decodeMsg( string msg ) {
        char[] delims = { ':' };

        string[] tokens = msg.Split(delims);
        return tokens[3];
    }

    //decode data translates bytes recieved in data packet to original string.
    private string decodeData ( NetworkBroadcastResult data) {
        return ( System.Text.Encoding.Unicode.GetString(data.broadcastData));
    }

    public void selectIP( int ipIndex ) {
        ip = ipList[ipIndex];
    }

    public string getIP() {
        return ip;
    }
    
}
