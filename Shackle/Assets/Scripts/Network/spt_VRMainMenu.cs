using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_VRMainMenu : NetworkBehaviour {

    NetworkManager manager;
    //NetworkDiscovery discovery;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        //discovery = GameObject.Find("NetworkDiscovery").GetComponent<NetworkDiscovery>();    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
