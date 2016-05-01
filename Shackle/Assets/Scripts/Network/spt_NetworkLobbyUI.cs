/* spt_NetworkLobbyUI.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This file provides an interface  with the Unity Network Manager for starting and joining a game of shackle
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class spt_NetworkLobbyUI : MonoBehaviour {

    public NetworkManager manager;
    private string gameSelection;

    void Awake() {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update() {
        //this will need to be altered in the future, for now it's ok
        GameObject monster = GameObject.Find("MonsterStandin");


        if (monster != null)
        {
            if(monster.GetComponent<spt_monsterMovement>().pLoss && Input.anyKeyDown)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().outputMetrics();
                manager.StopHost();
                manager.StopClient();
            }
        }

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
        manager.networkAddress = GameObject.Find("InputField").GetComponent<InputField>().text;
        manager.networkPort = 7777;
        manager.StartClient();
    }

    public void hostGame() {
        manager.StopHost();
        manager.StartHost();
    }
    
    void OnApplicationQuit() {
        manager.StopHost();
    }

    public void changeLevel() {
        manager.onlineScene = GameObject.Find("Label").GetComponent<Text>().text;
    }
}
