/*
spt_panelListener

Author(s): Hayden Platt

Revision 1

Listener for the panel burning. Once the laser has hit the panel,
this script opens the trap doors and raises the TNT Levers.
*/
using UnityEngine;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_panelListener : spt_baseInteractiveObject
    {
        private bool once = false;

        public GameObject trapDoorA;
        public GameObject trapDoorB;
        public GameObject leverA;
        public GameObject leverB;

        // Use this for initialization
        override protected void Start()
        {

        }

        // Update is called once per frame
        override protected void Update()
        {
            //If the laser has hit the panel, open the trapdoors and raise the TNT Levers
            Debug.Log(GameObject.FindWithTag("Player").name);
            if (!once && spt_laserSwitch.local_laserHitPanel)//GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true)
            {
                trapDoorA.transform.Translate(Vector3.right);
                trapDoorB.transform.Translate(Vector3.right);
                leverA.transform.Translate(Vector3.up);
                leverB.transform.Translate(Vector3.up);
                once = true;
            }
        }
    }
}