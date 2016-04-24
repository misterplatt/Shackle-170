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
}
