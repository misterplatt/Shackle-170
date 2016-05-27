using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class spt_ManagerMenuInterface : NetworkBehaviour {

    public NetworkManager manager;
    string ip = "";

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
        if (SceneManager.GetActiveScene().name != "VRMainMenu") return;
        /*
        if (currentTime - lastInit > 5.0F && !discovery.isServer)
        {
            discovery.StopBroadcast();
            discovery.StartAsClient();
            listGames();
        }
        */

        GameObject joinButton = GameObject.Find("Painting_Canvas").transform.Find("Play/btn_join").gameObject;
        GameObject ipField = GameObject.Find("InputField");

        if (ip == "")
        {
            GameObject.Find("Painting_Canvas").transform.Find("Play/txt_gameDetected").gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find("Painting_Canvas").transform.Find("Play/txt_gameDetected").gameObject.SetActive(true);
        }
        Debug.Log("kk");

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
        if (ip == "") return;
        manager.networkAddress = ip;
        //manager.networkPort = 17750;
        manager.StartClient();
    }

    public void hostGame()
    {
        manager.StopHost();
        manager.StartHost();
    }
}
