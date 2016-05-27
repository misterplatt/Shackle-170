/*
spt_garageDoor

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 5

Open the garage if opener is used on door for holdTime seconds.
Plays garagelocked/garageopened sound depending on when the player tries to open the garage.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_garageDoor : spt_baseInteractiveObject
    {
        public static bool local_puzzleCompletion;
        public static bool local_puzzleCompletionMonster;
        public static bool local_garageOpeningAttempt;

        private static bool garageFail = false;
        private AudioSource garageSound;
        public AudioClip garageOpenSound;
        public AudioClip garageLockedSound;

        public Animation garageDoorAnimations;

        override protected void Start()
        {
            garageSound = GetComponent<AudioSource>();
            garageDoorAnimations = GameObject.Find("GarageDoorMech").GetComponent<Animation>();
        }

        override protected void Update()
        {
            //DEBUG_CMD: Verifying new garage state works locally.
            if (Input.GetKeyDown(KeyCode.M)) {
                spt_garageLock.local_garageDoorUnlocked = true; //Comment out to test failure
                holdSuccess();
            } 
            //If the garage remote is used...
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[10].state == true) {
                //Play a fail sound/animation if garage is locked
                if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[5].state == false)
                {
                    //Play failed opening sound
                    garageSound.clip = garageLockedSound;
                    garageSound.Play();

                    //Play failed opening animation
                    garageDoorAnimations.Play("garageOpen_locked");
                }
                //Play an opening sound/animation if garage is unlocked
                if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[5].state == true)
                {
                    //Play opening sound
                    garageSound.clip = garageOpenSound;
                    garageSound.Play();

                    //Play opening animation
                    garageDoorAnimations.Play("garageOpen_unlocked");
                }
                //Turn local_garageOpeningAttempt back off until another attempt is made
                local_garageOpeningAttempt = false;
                spt_WorldState.worldStateChanged = true;
            }
        }

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            //If the garage door has been unlocked set puzzleCompletion to true
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[5].state == true) local_puzzleCompletionMonster = true;

            //Always turn off holding and set garage attempt to true
            holding = false;
            local_garageOpeningAttempt = true;
            spt_WorldState.worldStateChanged = true;
        }

        //Plug HandleClick
        override protected void HandleClick() { }
    }
}

