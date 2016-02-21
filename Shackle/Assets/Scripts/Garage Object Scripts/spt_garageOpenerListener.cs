/*
spt_garageOpenerListener

Author(s): Hayden Platt

Revision 1

Listens for correctChannel NPL state. Once true,
it applies gravity to the garageDoorOpener, dropping
it on the floor.
*/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_garageOpenerListener : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //If the correctChannel network state is true, drop the garageOpener
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == true) GetComponent<Rigidbody>().useGravity = true;
    }
}