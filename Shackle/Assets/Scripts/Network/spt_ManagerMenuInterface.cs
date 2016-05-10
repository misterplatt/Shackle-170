using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking;
using System.Collections;

public class spt_ManagerMenuInterface : NetworkBehaviour {

    public NetworkManager manager;


    void Awake()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update()
    {
        GameObject monster = GameObject.Find("MonsterStandin");

        if (Input.GetKeyDown(KeyCode.F10)) connectLocal();

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
        manager.StopHost();
        GameObject.Find("NetworkDiscovery").GetComponent<NetworkDiscovery>().StopBroadcast();
        manager.networkAddress = "localhost";
       // manager.networkPort = 17750;
        manager.StartClient();
    }

    public void connect()
    {
        try {
            spt_NetworkDiscovery nDiscover = GameObject.Find("NetworkDiscovery").GetComponent<spt_NetworkDiscovery>();
            manager.networkAddress = nDiscover.getIP();
            if (manager.networkAddress == Network.player.ipAddress) manager.networkAddress = "localhost";
            nDiscover.stopAllDiscovery();
        }
        catch {
            manager.networkAddress = "localhost";
        }
        //manager.networkPort = 17750;
        manager.StartClient();
    }

    public void hostGame()
    {
        manager.StopHost();
        manager.StartHost();
    }
}
