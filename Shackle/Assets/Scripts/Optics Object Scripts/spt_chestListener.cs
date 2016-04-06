/*
spt_chestLid

Author(s): Hayden Platt

Revision 1

Listener for the chest opening. Once the laser has hit the laser lock,
this script opens the chest lid.
*/
using UnityEngine;
using System.Collections;

public class spt_chestListener : MonoBehaviour {

    private bool once = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //LOCAL VERSION: If the laser has hit the lock, open the chest
        if (!once && VRStandardAssets.Examples.spt_laserSwitch.local_laserHitLock == true) {
            gameObject.SetActive(false);
            once = true;
        }
    }
}
