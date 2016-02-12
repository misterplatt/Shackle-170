using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_TVScreenListener : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true) GetComponent<SpriteRenderer>().enabled = true;
        else GetComponent<SpriteRenderer>().enabled = false;
    }
}
