/* spt_angerPuzzleState.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 2/15/2016
 * 
 * This file is used one-time triggered anger events by flipping the associated baseAngerClass object when a puzzle
 * state change occurs.*/

using UnityEngine;
using System;

public class spt_angerPuzzleStateTrigger : MonoBehaviour {

    private spt_NetworkPuzzleLogic network;
    private bool indexInitialized = false;
    private int i;
    private bool triggered = false;

    private spt_angerObject angerObject;
    
    // Use this for initialization
	void Start () {
        angerObject = gameObject.GetComponent<spt_angerObject>();
	}
	
	// Update is called once per frame
	void Update () {
        
        // Tries to get a handle on the network if it hasn't already.
        if (network == null)
        {
            network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
        }
        else
        {
            if (network.loaded && network.loaded == true)
            {
                // If we just got a handle on the network, find the index of the particular event in the networked puzzle states
                if (!indexInitialized)
                {
                    for (int index = 0; index < network.PuzzleStates.Count; ++index)
                    {
                        if (network.PuzzleStates[index].itemName == gameObject.name)
                        {
                            i = index;
                            indexInitialized = true;
                            break;
                        }
                    }
                }
                
                // Check for the puzzle state to change, when it does, flip the baseAngerClass object.
                else
                {
                    if (network.PuzzleStates[i].state == true)
                    {
                        triggered = true;
                        angerObject.toggleVisibility();
                    }
                }
            }
        }
	}
}
