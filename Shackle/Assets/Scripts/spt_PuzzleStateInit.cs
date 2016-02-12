using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// spt_PuzzleStateInit will be used to set the initial state of puzzle logic.
// Designers will added the event names to the list using the unity gui and any event added
// is assumed to have an initial false state. 
public class spt_PuzzleStateInit : MonoBehaviour {

    public List<string> initEvents;

	// Use this for initialization
	void Start () {
        debug_reportEvents();
	}

    void debug_reportEvents()
    {
        for (int index = 0; index < initEvents.Count; ++index) { Debug.Log(initEvents[index] ); }
    }

}
