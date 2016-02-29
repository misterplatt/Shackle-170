/*
spt_garageVictoryListener
Author(s): Hayden Platt
Revision 1
Listens for NPL puzzle completion to show the 
"YOU WIN" UI Text.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_garageVictoryListener : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //If the correctChannel network state is true, drop the garageOpener
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[3].state == true) GetComponent<Text>().enabled = true;
    }
}
