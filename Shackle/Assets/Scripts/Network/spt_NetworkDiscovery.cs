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
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class spt_NetworkDiscovery : NetworkBehaviour {

    public NetworkDiscovery discovery;
    private string gameName;
    private string ip;
    private string[] ipList;
    private bool inLobby;
    private bool notChecked;

    public static spt_NetworkDiscovery instance;

    float lastUpdate = 0.0F;
    float lastInit = 0.0F;

    const int BUTTON_CAP = 3;

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
        }
    }

    void Start() {
        ip = "";
        inLobby = false;
        notChecked = false;
        ipList = new string[4];
        discovery.Initialize();
        listen();
    }

    void Update() {
        if ( SceneManager.GetActiveScene().name != "VRMainMenu" ) return;
        float currentTime = Time.time;
        if (currentTime - lastUpdate > 1.0F || notChecked)
        {
            lastUpdate = currentTime;
            listGames();
        }

        if (currentTime - lastInit > 5.0F && !discovery.isServer)
        {
            discovery.StopBroadcast();
            discovery.StartAsClient();
            listGames();
        }

        GameObject joinButton = GameObject.Find("Painting_Canvas").transform.Find("Play/btn_join").gameObject;
        if (ip == "")
        {
            joinButton.SetActive(false);
        }
        else joinButton.SetActive(true);
    }    

    public void startBroadcast() {
        discovery.StopBroadcast();
        discovery.Initialize();
        discovery.broadcastData = "ShackleGame";
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

    void listGames()
    {
        if (notChecked) notChecked = true;
        Debug.Log("Listing Games...");
        if (discovery.broadcastsReceived == null)
        {
            return;
        }

        if (discovery.broadcastsReceived.Count == 0)
        {
            ip = "";
            Debug.Log("No Count");
            return;
        }

        List<string> keyList = new List<string>(discovery.broadcastsReceived.Keys);
        ip = decodeMsg(keyList[0]);
    }

    private void clearList() {
        for (int index = 1; index <= BUTTON_CAP; ++index) {
            GameObject.Find("btn_game" + index).GetComponentInChildren<Text>().text = "";
            ipList[index] = "";
        }
    }

    private string decodeMsg( string msg ) {
        char[] delims = { ':' };
        Debug.Log("Decoding : " + msg);
        string[] tokens = msg.Split(delims);
        foreach (string token in tokens) {
            if (token.Contains(".")) return token;
        }
        return "";
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
