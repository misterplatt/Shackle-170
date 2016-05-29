/*
spt_hatch

Author(s): Hayden Platt, Lauren Cunningham

Revision 2

Opens the hatch if pickaxe is used on
it for holdTime.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_hatch : spt_baseInteractiveObject
    {
        private AudioSource aSource;
        public AudioClip failedOpen;
        public AudioClip successfulOpen;

        public Animation hatchAnimations;

        public static bool local_puzzleCompletion;
        public static bool local_puzzleCompletionMonster;

        override protected void Start()
        {
            aSource = GetComponent<AudioSource>();
            hatchAnimations = transform.FindChild("mdl_hatchDoor").gameObject.GetComponent<Animation>();
        }

        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            //If the hatch door has been unlocked, open the garage door and set puzzleCompletion to true
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[1].state == true)
            {
                aSource.clip = successfulOpen;
                aSource.Play();
                //NPL Update
                //local_puzzleCompletion = true;
                //spt_WorldState.worldStateChanged = true;

                transform.Translate(new Vector3(0, .38f, 0));
                transform.eulerAngles = new Vector3(-30, 0, 0);
                Invoke("UnlockedAnimation", 3.5f);
                local_puzzleCompletionMonster = true;
                spt_WorldState.worldStateChanged = true;
                holding = false;
            }
            else
            {
                aSource.clip = failedOpen;
                aSource.Play();
                Invoke("LockedAnimation", .3f);
                holding = false;
            }
        }

        void LockedAnimation()
        {
            hatchAnimations.Play("hatchOpen_locked");
        }

        void UnlockedAnimation()
        {
            hatchAnimations.Play("hatchOpen_unlocked");

        }
    }
}