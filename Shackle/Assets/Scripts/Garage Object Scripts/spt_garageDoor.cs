/*
spt_garageDoor

Author(s): Hayden Platt, Dara Diba

Revision 3

Open the garage if opener is used on door for holdTime seconds.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_garageDoor : spt_baseInteractiveObject
    {
        public static bool local_puzzleCompletion;

        private static bool garageFail = false;
        private AudioSource garageLockedSound;

        override protected void Start()
        {
            garageLockedSound = GetComponent<AudioSource>();
        }

        override protected void Update()
        {
            if (garageFail){
                garageLockedSound.Play();
                garageFail = false;
            }

        }

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            //If the garage door has been unlocked, open the garage door and set puzzleCompletion to true
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[5].state == true)
            {
                transform.Translate(new Vector3(0, 1, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
                local_puzzleCompletion = true;
                spt_WorldState.worldStateChanged = true;
                holding = false;
            }
            else garageFail = true;
         }

        //Plug HandleClick
        override protected void HandleClick() { }
    }
}

