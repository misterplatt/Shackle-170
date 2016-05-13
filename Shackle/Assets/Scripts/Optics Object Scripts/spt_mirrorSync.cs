/*
spt_mirrorSync

Author(s): Hayden Platt

Revision 1

Simple NetworkBehaviour class which contains a SyncVar to determine
whether or not a mirror's laser should be active. Bool is changed in spt_mirror.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class spt_mirrorSync : NetworkBehaviour {

    [SyncVar]
    public bool emitsLaser = false;

    bool isFourthMirror = false;

    void Start() {
        //Initalize whether or not this is the 4th mirror
        isFourthMirror = (gameObject.name == "mdl_mirrorPickup (4)");
    }
    /* THIS CODE BREAKS NETWORKED MIRRORS
    void Update()
    {
        //If the labDoor isn't open and this is the 4th mirror, stop emitting the laser
        if (isFourthMirror && GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[8].state == false) emitsLaser = false;
    }*/
}
