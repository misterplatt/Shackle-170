using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_ManagerMenuInterface : MonoBehaviour {

    public NetworkManager manager;


    void Awake()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update()
    {
        GameObject monster = GameObject.Find("MonsterStandin");

        if (monster != null)
        {
            if (monster.GetComponent<spt_monsterMovement>().pLoss && Input.anyKeyDown)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().outputMetrics();
                manager.StopHost();
                manager.StopClient();
            }
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().outputMetrics();
            manager.StopHost();
            manager.StopClient();
        }
    }

    void OnApplicationQuit()
    {
        manager.StopHost();
    }

    public void connectLocal()
    {
        manager.networkAddress = "localhost";
        manager.networkPort = 7777;
        manager.StartClient();
    }

    public void connect()
    {
        Debug.Log("Connecting...");
        manager.networkAddress = GameObject.Find("NetworkDiscovery").GetComponent<spt_NetworkDiscovery>().getIP();
        manager.networkPort = 7777;
        manager.StartClient();
    }

    public void hostGame()
    {
        manager.StopHost();
        manager.StartHost();
    }
}
