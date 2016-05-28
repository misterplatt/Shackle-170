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
        public AudioClip chestSlam;
        public AudioClip chestUnlock;
        public AudioClip chestOpen;

        private float timer = 0f;
        private float completionTime = 2f;

        private Animation discAnimations;

        protected override void Start()
        {
            initialRotation = transform.parent.rotation.eulerAngles;
            aSource = GetComponent<AudioSource>();
            discAnimations = transform.FindChild("mdl_chestLockDisc").gameObject.GetComponent<Animation>();
        }

        // Update is called once per frame
        override protected void Update()
        {

            //Check for laser collision while no laser has hit the lock
            if (!local_laserHitLock) {
                //Accumulate list of colliders intersecting the chest lock's collider
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, .1f);
                if (hitColliders.Length <= 2) timer = 0;
                //Check each collider
                foreach (Collider col in hitColliders)
                {
                    if (col.gameObject.tag == "laser")
                    {
                        //Play spinning anim and sound, increment timer
                        timer += Time.deltaTime;
                        discAnimations.Play("chestLock_spin");
                        aSource.clip = chestUnlock;
                        if (!aSource.isPlaying) aSource.Play();
                    }
                    
                }
            }

            //If a laser has hit the lock for long enough, set the corresponding puzzle state to true
            if (timer > completionTime) {
                local_laserHitLock = true;
                local_isChestOpen = true;
                spt_WorldState.worldStateChanged = true;
                
                timer = 0;
            }
            
            //If the laser has hit the lock, open the chest
            if (!once && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[1].state == true)
            {
                aSource.clip = chestOpen;
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
                aSource.clip = chestSlam;
                aSource.Play();

                //Closing vibrations
                spt_victoryListener.Both = true;
                spt_victoryListener.vibrationTime = 1.5f;
                spt_victoryListener.vibrationForce = 1f;
                spt_victoryListener.vibrationz = true;

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
