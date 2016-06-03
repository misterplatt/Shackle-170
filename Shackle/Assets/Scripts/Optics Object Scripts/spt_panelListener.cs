/*
spt_panelListener

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham   

Revision 3

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
        public AudioClip criticalError;
        public AudioClip panelBurning;
        public AudioClip systemMeltDown;
        public AudioClip hatchOpening;
        public AudioClip systemStartup;
        public AudioClip systemLooping;
        private bool didSystemMelt = false;
        private bool oncerino = false;
        private bool errorDetected = false;

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
                if (!oncerino)
                {
                    childSource.clip = systemStartup;
                    childSource.Play();
                    oncerino = true;
                }
                if (!childSource.isPlaying)
                {
                    childSource.clip = systemLooping;
                    childSource.loop = true;
                    childSource.Play();
                }
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
                        childSource.clip = criticalError;
                        if (!errorDetected)
                        {
                            childSource.loop = true;
                            childSource.Play();
                            errorDetected = true;
                        }
                    }
                }
                childSource.Stop();
            }

            if (timerino > completionTime)
            {
                //If a laser has hit the panel for completionTime seconds, set the corresponding puzzle state to true
                childSource.loop = false;
                childSource.clip = systemMeltDown;
                childSource.Play();
                Invoke("Melted", systemMeltDown.length);
                local_laserHitPanel = true;
                spt_WorldState.worldStateChanged = true;

                timerino = 0;

                ParticleSystem parentSystem = gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
                ParticleSystem[] childrenSystems = gameObject.transform.GetChild(2).GetComponentsInChildren<ParticleSystem>();
                //parentSystem.enableEmission = true;
                //parentSystem.Stop();
                parentSystem.Play();
                foreach (ParticleSystem system in childrenSystems)
                {
                    //system.enableEmission = false;
                    //system.Stop();
                    system.Play();
                }
            }

            //If the laser has hit the panel, open the trapdoors and raise the TNT Levers
            if (!once && GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true && didSystemMelt)
            {
                //TEMP FUNCTIONALITY UNTIL MODELS ARE IMPORTED
                trapDoorA.GetComponent<AudioSource>().clip = hatchOpening;
                trapDoorB.GetComponent<AudioSource>().clip = hatchOpening;
                trapDoorA.GetComponent<AudioSource>().Play();
                trapDoorB.GetComponent<AudioSource>().Play();

                trapDoorA.transform.GetComponent<Animation>().Play("TNTOpen");
                trapDoorB.transform.GetComponent<Animation>().Play("TNTOpen");
                Invoke("raisePlungers", 1.25f);
                once = true;
            }
        }

        void SystemMelting()
        {
            
        }

        void raisePlungers() {
            leverA.transform.parent.GetComponent<Animation>().Play("TNTRiseA");
            leverB.transform.parent.GetComponent<Animation>().Play("TNTRiseB");
        }

        void Melted()
        {
            didSystemMelt = true;
        }
    }
}