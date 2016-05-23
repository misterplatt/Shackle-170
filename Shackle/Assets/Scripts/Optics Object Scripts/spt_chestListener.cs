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
        private Vector3 initialRotation;
        private AudioSource aSource;
        public AudioClip DARASFRESHSOUND;

        protected override void Start()
        {
            initialRotation = transform.parent.rotation.eulerAngles;
            aSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        override protected void Update()
        {

            //Check for laser collision while no laser has hit the lock
            if (!local_laserHitLock) {
                //Accumulate list of colliders intersecting the chest lock's collider
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, .1f);
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
                GameObject go = (GameObject)Instantiate(Resources.Load("Smoke"), new Vector3(1.23f, 1.857f, -1.808f), Quaternion.Euler(0, 0, 0));
                go.GetComponent<ParticleSystem>().enableEmission = true;
                //GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState("isChestOpen", true, "mdl_chestLock");
            }

            if (once && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[1].state == false) {
                //Chest will do things, sound will play
                transform.parent.eulerAngles = initialRotation;
                aSource.clip = DARASFRESHSOUND;
                aSource.Play();

                //Uncomment below line when the other code (the stuff to close the chest in-scene) is implemented
                local_isChestOpen = false;
                local_laserHitLock = false;
                spt_WorldState.worldStateChanged = true;
                once = false;
            }
        }

        public override void resetItem()
        {
            local_isChestOpen = false;
            local_laserHitLock = false;
            spt_WorldState.worldStateChanged = true;
        }
    }
}
