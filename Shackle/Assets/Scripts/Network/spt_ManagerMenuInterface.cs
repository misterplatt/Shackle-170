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
        Text iptxt = GameObject.Find("txt_IP").GetComponent<Text>();
        iptxt.text = "Your IP : " + Network.player.ipAddress;
    }

    void Update()
    {
        GameObject monster = GameObject.Find("MonsterStandin");

        if (Input.GetKeyDown(KeyCode.F10)) connectLocal();

    }

    void OnApplicationQuit()
    {
        manager.StopHost();
    }

    public void connectLocal()
    {
        manager.StopHost();
        NetworkDiscovery ndisc = GameObject.Find("NetworkDiscovery").GetComponent<NetworkDiscovery>();
        if (ndisc.running) ndisc.StopBroadcast();
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
