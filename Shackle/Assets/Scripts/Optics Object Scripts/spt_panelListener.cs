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

        private float timerino = 0f;
        public float completionTime;

        public GameObject trapDoorA;
        public GameObject trapDoorB;
        public GameObject leverA;
        public GameObject leverB;
        private AudioSource aSource;
        private AudioSource childSource;
        public AudioClip criticalHeat;
        public AudioClip panelBurning;
        public AudioClip systemMeltDown;
        public AudioClip hatchOpening;
        private bool didSystemMelt = false;

        // Use this for initialization
        override protected void Start()
        {
            completionTime = 8f;
            aSource = GetComponent<AudioSource>();
            childSource = GameObject.Find("SecurityMeltdownObject").GetComponent<AudioSource>();
        }

        // Update is called once per frame
        override protected void Update()
        {
            //Check for laser collision while no laser has hit the lock
            if (!local_laserHitPanel)
            {
                //Accumulate list of colliders intersecting the chest lock's collider
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, .3f);
                if (hitColliders.Length <= 2) timerino = 0;
                //Check each collider
                foreach (Collider col in hitColliders)
                {
                    if (col.gameObject.tag == "laser")
                    {
                        //Play burning and critical heat sound, increment timerino
                        timerino += Time.deltaTime;
                        aSource.clip = panelBurning;
                        if(!aSource.isPlaying) aSource.Play();
                        childSource.clip = criticalHeat;
                        if (!childSource.isPlaying)
                        {
                            childSource.loop = true;
                            childSource.Play();
                        }
                    }
                }
            }

            Debug.Log("HAYDEN CAN'T CODE FOR SHIT" + timerino);

            if (timerino > completionTime)
            {
                //If a laser has hit the panel for completionTime seconds, set the corresponding puzzle state to true
                Debug.Log("COMPLETION TIME" + completionTime);
                Debug.Log("PENIS " + timerino);
                childSource.loop = false;
                childSource.clip = systemMeltDown;
                childSource.Play();
                Invoke("Melted", systemMeltDown.length);
                local_laserHitPanel = true;
                spt_WorldState.worldStateChanged = true;

                timerino = 0;
            }

            //If the laser has hit the panel, open the trapdoors and raise the TNT Levers
            if (!once && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true && didSystemMelt)
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

        void SystemMelting()
        {
            
        }

        void Melted()
        {
            didSystemMelt = true;
        }
    }
}