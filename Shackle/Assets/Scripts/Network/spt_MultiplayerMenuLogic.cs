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

public class spt_MultiplayerMenuLogic : MonoBehaviour {

    const int NETPORT = 1775;
    spt_NetworkDiscovery serverListener;
    NetworkLobbyManager nlm;
    Button startButton;
    Button p1B;
    Button p2B;

    string selIP = "";
    bool inLobby;
    bool pOneColorSet = false;
    bool pTwoColorSet = false;

    int pCount;

    void Start() {
        serverListener = GameObject.Find("NetworkDiscovery").GetComponent<spt_NetworkDiscovery>();
        nlm = GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>();
        inLobby = true;
        pCount = 0;

        startButton = GameObject.Find("btn_Start").GetComponent<Button>();
        p1B = GameObject.Find("btn_P1").GetComponent<Button>();
        p2B = GameObject.Find("btn_P2").GetComponent<Button>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            setUIStatus("UI_LobbySearch", true);
            setUIStatus("UI_LobbyHost", false);
        }

        //update player count for client side sync

        updatePStatus();
        if (!isServer())
        {
            startButton.enabled = false;
            startButton.GetComponent<Image>().enabled = false;
            startButton.GetComponentInChildren<Text>().enabled = false;
            return;
        }
        else
        {
            startButton.enabled = true;
            startButton.GetComponent<Image>().enabled = true;
            startButton.GetComponentInChildren<Text>().enabled = true;
        }

            if (playersReady()) startButton.interactable = true;
        else startButton.interactable = false;
    }

    public bool isServer()
    {
        GameObject[] lPlayers = GameObject.FindGameObjectsWithTag("lobbyPlayer");

        foreach (GameObject lPlayer in lPlayers)
        {
            NetworkIdentity nIdent = lPlayer.GetComponent<NetworkIdentity>();
            if (nIdent.isServer && nIdent.isLocalPlayer) return true;
        }
        return false;
    }

    bool playersReady()
    {
        GameObject[] lPlayers = GameObject.FindGameObjectsWithTag("lobbyPlayer");

        if (lPlayers.Length < 2) return false;
        foreach (GameObject lPlayer in lPlayers)
        {
            if (!lPlayer.GetComponent<spt_NetworkLobbyPlayer_rdy>().pReady) return false;
        }
        return true;
    }


    void updatePStatus() {
        if (!inLobby) return;
        Debug.Log("Test");    

        //get players
        GameObject[] lPlayers = GameObject.FindGameObjectsWithTag("lobbyPlayer");
        int dbg_int = 1;

        foreach (GameObject lPlayer in lPlayers)
        {
            Debug.Log(dbg_int++ + " : " + lPlayer.GetComponent<spt_NetworkLobbyPlayer_rdy>().pReady);
        }

        int playerCount = 0;
        if (lPlayers.Length > 0) playerCount = lPlayers[0].GetComponent<spt_NetworkLobbyPlayer_rdy>().pCount;

        if (playerCount == 1)
        {
            Debug.Log("host");
            ColorBlock p1cb = p1B.colors;

            if (lPlayers[0].GetComponent<spt_NetworkLobbyPlayer_rdy>().pReady) p1cb.disabledColor = Color.green;
            else p1cb.disabledColor = Color.yellow;

            p1B.colors = p1cb;

            ColorBlock p2cb = p2B.colors;
            p2cb.disabledColor = Color.grey;
            p2B.colors = p2cb;
        }
        else if (playerCount == 2)
        {
            Debug.Log("cli");
            ColorBlock p1cb = p1B.colors;

            if (lPlayers[0].GetComponent<spt_NetworkLobbyPlayer_rdy>().pReady) p1cb.disabledColor = Color.green;
            else p1cb.disabledColor = Color.yellow;

            p1B.colors = p1cb;

            ColorBlock p2cb = p2B.colors;

            if (lPlayers[1].GetComponent<spt_NetworkLobbyPlayer_rdy>().pReady) p2cb.disabledColor = Color.green;
            else p2cb.disabledColor = Color.yellow;

            p2B.colors = p2cb;
        }
        Debug.Log(nlm.numPlayers);

        
    }

    public void startGame()
    {
        if (!playersReady()) return;

        nlm.CheckReadyToBegin();

    }


    public void ready() {

        //find LobbyPlayer I own.
        GameObject[] lPlayers = GameObject.FindGameObjectsWithTag("lobbyPlayer");
        foreach (GameObject lPlayer in lPlayers)
        {
            if (lPlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                //once found, toggle this bool state
                lPlayer.GetComponent<spt_NetworkLobbyPlayer_rdy>().toggleReady();
                break;
            }
        }
    }

    
    //Connect connects to discovered network game.
    public void connect() {
        if (serverListener.getIP() == "") return;
        inLobby = true;
        nlm.networkAddress = serverListener.getIP();
        nlm.networkPort = NETPORT;
        nlm.StartClient();

        setUIStatus("UI_LobbySearch", false);
        setUIStatus("UI_LobbyHost", true);
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
        Dropdown[] childComponents_Dropdown = lobbySearch_UI_root.GetComponentsInChildren<Dropdown>();

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
        foreach (Dropdown childDropdown in childComponents_Dropdown)
        {
            childDropdown.enabled = status;
        }
    }    

}
