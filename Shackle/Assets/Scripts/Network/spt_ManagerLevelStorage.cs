using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class spt_ManagerLevelStorage : MonoBehaviour {

    public string currentLevel;

	// Use this for initialization
	void Start () {
        currentLevel = "";

    }
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name == "LoadScreen") return;
        currentLevel = SceneManager.GetActiveScene().name;
    }

    public void cli_reconnect()
    {

    }
}
