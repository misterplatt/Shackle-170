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

public class spt_mainMenu_btns_vr : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        unloadScenes();

        GameObject manager = GameObject.Find("NetworkManager");
        GameObject disc = GameObject.Find("NetworkDiscovery");

        spt_ManagerMenuInterface menu = manager.GetComponent<spt_ManagerMenuInterface>();
        spt_NetworkDiscovery discifc = disc.GetComponent<spt_NetworkDiscovery>();

        Button join = transform.Find("Play/btn_join").gameObject.GetComponent<Button>();
        Button host = transform.Find("Play/btn_host").gameObject.GetComponent<Button>();
        
        join.onClick.AddListener(() => { menu.connect(); });
        host.onClick.AddListener(() => {
            menu.hostGame();
        });
        

    }

    void unloadScenes()
    {
        GameObject manager = GameObject.Find("NetworkManager");

        /*
        if ( manager.GetComponent<spt_NetworkManager_setSpawn>().isNetworkActive )
        {    
            manager.GetComponent<spt_NetworkManager_setSpawn>().StopAllCoroutines();
            manager.GetComponent<spt_NetworkManager_setSpawn>().StopHost();
            manager.GetComponent<spt_NetworkManager_setSpawn>().StopClient();
        }
        */

        Debug.Log("Unloading Scenes...");
        SceneManager.UnloadScene("net_SpookyGarage");
        SceneManager.UnloadScene("net_RangerOutpost");
        SceneManager.UnloadScene("net_OpticsLab");
    }
}
