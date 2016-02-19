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
        if (network == null)
        {
            network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
        }
        else
        {
            if (network.loaded && network.loaded == true)
            {
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
