/* spt_mainMenu_btns.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This script reassigns main menu button functions.
 */

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class spt_mainMenu_btns : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        unloadScenes();
        
        GameObject manager = GameObject.Find("NetworkManager");
        spt_NetworkLobbyUI lobbyUI = manager.GetComponent<spt_NetworkLobbyUI>();

        Button local = transform.Find("btn_ConnectLocal").gameObject.GetComponent<Button>();
        Button ip = transform.Find("btn_ConnectIP").gameObject.GetComponent<Button>();
        Button host = transform.Find("btn_Host").gameObject.GetComponent<Button>();

        local.onClick.AddListener(() => { lobbyUI.connectLocal(); });
        ip.onClick.AddListener(() => { lobbyUI.connectLAN(); });
        host.onClick.AddListener(() => { lobbyUI.hostGame(); });
        
    }

    void unloadScenes()
    {
        Debug.Log("Unloading Scenes...");
        SceneManager.UnloadScene("net_SpookyGarage");
        SceneManager.UnloadScene("net_RangerOutpost");
        SceneManager.UnloadScene("net_OpticsLab");
    }
}
