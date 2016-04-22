﻿/*
spt_chestListener

Author(s): Hayden Platt

Revision 2

Listener for the chest opening. Once the laser has hit the laser lock,
this script opens the chest lid.
*/
using UnityEngine;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_chestListener : spt_baseInteractiveObject
    {
        public static bool local_laserHitLock = false;
        private bool once = false;

        // Update is called once per frame
        override protected void Update()
        {
            //Check for laser collision while no laser has hit the lock
            if (!local_laserHitLock) {
                //Accumulate list of colliders intersecting the chest lock's collider
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, .25f);

                //Check each collider
                foreach (Collider col in hitColliders)
                {
                    if (col.gameObject.tag == "laser")
                    {
                        //If a laser has hit the lock, set the corresponding puzzle state to true
                        local_laserHitLock = true;
                        spt_WorldState.worldStateChanged = true;
                    }
                }
            }
            
            //LOCAL VERSION: If the laser has hit the lock, open the chest
            if (!once && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[1].state == true)
            {
                //TEMP FUNCTIONALITY UNTIL MODEL IS IMPORTED
                transform.parent.FindChild("Locked Crate Lid").gameObject.SetActive(false);
                once = true;
            }
        }
    }
}
