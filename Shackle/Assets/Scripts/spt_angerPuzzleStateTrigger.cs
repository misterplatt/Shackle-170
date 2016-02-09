using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spt_angerPuzzleStateTrigger : MonoBehaviour {

    private spt_NetworkPuzzleLogic network;
    private dev_LogicPair theThing;
    bool pastState;
    
    // Use this for initialization
	void Start () {
        network = GameObject.FindObjectOfType(typeof(spt_NetworkPuzzleLogic)) as spt_NetworkPuzzleLogic;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
