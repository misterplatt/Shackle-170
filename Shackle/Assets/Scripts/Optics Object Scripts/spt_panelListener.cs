/*
spt_panelListener

Author(s): Hayden Platt, Dara Diba

Revision 2

Listener for the panel burning. Once the laser has hit the panel,
this script opens the trap doors and raises the TNT Levers.
Added sound functionality - Dara
*/
using UnityEngine;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_panelListener : spt_baseInteractiveObject
    {
        public static bool local_laserHitPanel = false;

        private bool once = false;

        public GameObject trapDoorA;
        public GameObject trapDoorB;
        public GameObject leverA;
        public GameObject leverB;
        private AudioSource aSource;
        public AudioClip panelBurning;
        public AudioClip systemMeltDown;
        public AudioClip hatchOpening;

        // Use this for initialization
        override protected void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        override protected void Update()
        {
            //Check for laser collision while no laser has hit the lock
            if (!local_laserHitPanel)
            {
                //Accumulate list of colliders intersecting the chest lock's collider
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, .3f);

                //Check each collider
                foreach (Collider col in hitColliders)
                {
                    if (col.gameObject.tag == "laser")
                    {
                        aSource.clip = panelBurning;
                        aSource.Play();
                        //If a laser has hit the panel, set the corresponding puzzle state to true
                        local_laserHitPanel = true;
                        spt_WorldState.worldStateChanged = true;
                    }
                }
            }

            //If the laser has hit the panel, open the trapdoors and raise the TNT Levers
            if (!once && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true)
            {
                //TEMP FUNCTIONALITY UNTIL MODELS ARE IMPORTED
                trapDoorA.GetComponent<AudioSource>().clip = hatchOpening;
                trapDoorB.GetComponent<AudioSource>().clip = hatchOpening;
                trapDoorA.GetComponent<AudioSource>().Play();
                trapDoorB.GetComponent<AudioSource>().Play();

                trapDoorA.transform.FindChild("mdl_hatchDoorR").Translate(Vector3.left);
                trapDoorA.transform.FindChild("mdl_hatchDoorL").Translate(Vector3.right);
                trapDoorB.transform.FindChild("mdl_hatchDoorR").Translate(Vector3.left);
                trapDoorB.transform.FindChild("mdl_hatchDoorL").Translate(Vector3.right);
                leverA.transform.Translate(Vector3.up);
                leverB.transform.Translate(Vector3.up);
                once = true;
            }
        }
    }
}