using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class spt_ManagerMenuInterface : NetworkBehaviour {

    public NetworkManager manager;


    void Awake()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Start()
    {

    }

    public void showIp()
    {
        //Get the ip text field and update it. 
        GameObject ipTxt = GameObject.Find("txt_IP");
        Text iptxt = null;
        if (ipTxt != null)
        {
            iptxt = ipTxt.GetComponent<Text>();
            iptxt.text = "Your IP : " + Network.player.ipAddress;
        }
    }

    void Update()
    {
        GameObject monster = GameObject.Find("MonsterStandin");

        if (Input.GetKeyDown(KeyCode.F10)) connectLocal();

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
            
            if (manager.networkAddress == Network.player.ipAddress) manager.networkAddress = "localhost";
            
        }
        catch {
            manager.networkAddress = "localhost";
        }
        //manager.networkPort = 17750;
        manager.StartClient();
    }

    public void hostGame()
    {
        manager.StartHost();
        //manager.StopHost();
        
    }
}
