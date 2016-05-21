/*
spt_chestListener

Author(s): Hayden Platt, Dara Diba, Lauren Cunnigham

Revision 3

Listener for the chest opening. Once the laser has hit the laser lock,
this script opens the chest lid.
Added sound functionality. - Dara
*/
using UnityEngine;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_chestListener : spt_baseInteractiveObject
    {
        
        public static bool local_laserHitLock = false;
        public static bool local_isChestOpen = false;
        private bool once = false;
        private Vector3 spherePos;
        private AudioSource aSource;

        protected override void Start()
        {
            spherePos = transform.FindChild("sphereCastPoint").position;
            aSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        override protected void Update()
        {

            //Check for laser collision while no laser has hit the lock
            if (!local_laserHitLock) {
                //Accumulate list of colliders intersecting the chest lock's collider
                Collider[] hitColliders = Physics.OverlapSphere(spherePos, .25f);
                //Check each collider
                foreach (Collider col in hitColliders)
                {
                    if (col.gameObject.tag == "laser")
                    {
                        //If a laser has hit the lock, set the corresponding puzzle state to true
                        local_laserHitLock = true;
                        local_isChestOpen = true;
                        spt_WorldState.worldStateChanged = true;
                    }
                    
                }
            }
            
            //If the laser has hit the lock, open the chest
            if (!once && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[1].state == true)
            {
                aSource.Play();
                local_laserHitLock = true;
                local_isChestOpen = true;
                transform.parent.eulerAngles = new Vector3(-50,270,0);
                once = true;

                //Smoke Particles
                GameObject go = (GameObject)Instantiate(Resources.Load("Smoke"), gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                //GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState("isChestOpen", true, "mdl_chestLock");
            }
        }

        public override void resetItem()
        {
            //Chest will do things, sound will play

            //Uncomment below line when the other code (the stuff to close the chest in-scene) is implemented
            //GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState("isChestOpen", false, "mdl_chestLock");
        }

    }
}
