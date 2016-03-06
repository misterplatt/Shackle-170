/*
spt_TVScreenListener

Author(s): Hayden Platt

Revision 1

Listens for event cues to show and alter TV screen sprite.
*/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_TVScreenListener : NetworkBehaviour {
	
	// Update is called once per frame
	void Update () {
        //If the tvOn network state is true, turn on the static
        //Debug.Log("TV Power " + GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state);
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true &&
            GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == false)
            GetComponent<SpriteRenderer>().enabled = true;

        //If tvOn and correctChannel are true, turn on the success screen
        else if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true &&
            GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == true) GetComponent<SpriteRenderer>().color = Color.green;

        //Otherwise, turn the screen off
        else if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == false || 
            GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[3].state == false) GetComponent<SpriteRenderer>().enabled = false;
    }
}
