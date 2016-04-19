/* spt_MultiplayerMenuLogic.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This script allows for 
 */

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class spt_MultiplayerMenuLogic : NetworkBehaviour {

    const int NETPORT = 1775;
    spt_NetworkDiscovery serverListener;
    NetworkLobbyManager nlm;
    string selIP = "";
    bool inLobby;

    void Start() {
        serverListener = GameObject.Find("NetworkDiscovery").GetComponent<spt_NetworkDiscovery>();
        nlm = GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>();
        inLobby = true;
    }

    void Update() {
        updatePStatus();
        if (Input.GetKeyDown(KeyCode.F12)) {
            setUIStatus("UI_LobbySearch", true);
            setUIStatus("UI_LobbyHost", false);
        }
    }

    void updatePStatus() {
        if (!inLobby) return;
        Debug.Log(nlm.numPlayers);
    }
    
    //Connect connects to discovered network game.
    public void connect() {
        if (serverListener.getIP() == "") return;
        nlm.networkAddress = "localhost";//serverListener.getIP();
        nlm.networkPort = NETPORT;
        nlm.StartClient();
        inLobby = true;
    }

    public void selectGame(int index) {
        serverListener.selectIP(index);
    }

    //removes all UI options for searching for game and instead
    //reveals UI options for hosting a game.
    public void hostGame() {
        string gameName = GameObject.Find("InputField").GetComponent<InputField>().text;
        if (!nameCheck(gameName)) return;

        setUIStatus("UI_LobbySearch", false);
        setUIStatus("UI_LobbyHost", true);

        serverListener.setGameName(gameName);       
        serverListener.startBroadcast();
        nlm.networkPort = NETPORT;

        nlm.StartHost();
        inLobby = true;
    }

    public void cancelHost() {
        setUIStatus("UI_LobbyHost", false);
        setUIStatus("UI_LobbySearch", true);
        serverListener.stopAllDiscovery();
        inLobby = false;
        nlm.StopHost();
    }

    private bool nameCheck( string name ) {
        if (name == "Game Name" || name == "" || name == "Invalid Game Name") {
            GameObject.Find("InputField").GetComponent<InputField>().text = "Invalid Game Name";
            return false;
        }
        return true;
    }

    private void setUIStatus( string name, bool status ) {
        //get root object,  disable all components
        GameObject lobbySearch_UI_root = GameObject.Find(name);
        Button[] childComponents_Buttons = lobbySearch_UI_root.GetComponentsInChildren<Button>();
        Text[] childComponents_Text = lobbySearch_UI_root.GetComponentsInChildren<Text>();
        Image[] childComponents_Image = lobbySearch_UI_root.GetComponentsInChildren<Image>();
        InputField[] childComponents_Input = lobbySearch_UI_root.GetComponentsInChildren<InputField>();

        foreach (Button childButton in childComponents_Buttons) {
            childButton.enabled = status;
        }

        foreach (Text childText in childComponents_Text) {
            childText.enabled = status;
        }

        foreach (Image childImage in childComponents_Image) {
            childImage.enabled = status;
        }

        foreach (InputField childInput in childComponents_Input) {
            childInput.enabled = status;
        }
    }    

}
