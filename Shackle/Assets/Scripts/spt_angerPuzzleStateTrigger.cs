﻿using UnityEngine;
using System;

public class spt_angerPuzzleStateTrigger : MonoBehaviour {

    private spt_Events network;
    private bool indexInitialized = false;
    private int i;
    private bool triggered = false;

    private spt_angerObject angerObject;
    
    // Use this for initialization
	void Start () {
        network = GameObject.FindObjectOfType(typeof(spt_Events)) as spt_Events;
        angerObject = gameObject.GetComponent<spt_angerObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!indexInitialized)
        {
            for (int index = 0; index < network.devtool_PuzzleStates.Count; ++index)
            {
                if (network.devtool_PuzzleStates[index].item.name == gameObject.name)
                {
                    i = index;
                    indexInitialized = true;
                    break;
                }
            }
        }
        else
        {
            //MARKED FOR CHANGE
            /*
            if (network.PuzzleStates[i].state == true)
            {
                triggered = true;
                angerObject.toggleVisibility();
            }
            */
        }
	}
}
