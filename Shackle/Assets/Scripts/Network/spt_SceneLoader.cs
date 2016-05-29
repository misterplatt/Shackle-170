using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class spt_SceneLoader : NetworkBehaviour {
    void Start() {
        Debug.Log("mkay");
        StartCoroutine(cleanUp());         
    }
    public IEnumerator cleanUp() {        
        Debug.Log("Loading");        
        string level = GameObject.Find("NetworkManager").GetComponent<spt_ManagerLevelStorage>().currentLevel;
        Debug.Log(level);
        yield return new WaitForSeconds(7);
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene( level );
    }
}
