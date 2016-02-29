/*
spt_victoryListener
Author(s): Hayden Platt
Revision 1
Listens for NPL puzzle completion to show the 
"YOU WIN" UI Text.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_victoryListener : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //If the garage door is opened, set Win Text to visible
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[0].state == true) GetComponent<Text>().enabled = true;
    }
}
