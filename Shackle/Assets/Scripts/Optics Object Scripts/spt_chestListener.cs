/*
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

        private bool once = false;

        // Use this for initialization
        override protected void Start()
        {

        }

        // Update is called once per frame
        override protected void Update()
        {
            //LOCAL VERSION: If the laser has hit the lock, open the chest
            if (!once && spt_laserSwitch.local_laserHitLock)//GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[1].state == true)
            {
                gameObject.SetActive(false);
                once = true;
            }
        }
    }
}
