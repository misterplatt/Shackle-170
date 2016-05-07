/*
spt_keypad

Author(s): Hayden Platt, Lauren Cunningham

Revision 2

Simple script which moves the lab door on holdsuccess
with the ID card.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_keypad : spt_baseInteractiveObject
    {
        public float openDuration = 3f;
        private bool once = false;

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            if (!once) {
                transform.FindChild("lab_doors/left_door").transform.Translate(Vector3.right * 0.8f);
                transform.FindChild("lab_doors/right_door").transform.Translate(Vector3.left * 1f);
                GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("doorOpen", true, "KeyPad");
                holding = false;
                once = true;
                Invoke("CloseDoors", openDuration);
            }
        }

        //Function which closes the doors after openDuration seconds
        void CloseDoors() {
            transform.FindChild("lab_doors/left_door").transform.Translate(Vector3.right * -0.8f);
            transform.FindChild("lab_doors/right_door").transform.Translate(Vector3.left * -1f);
            GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("doorOpen", false, "KeyPad");
            once = false;
        }

        //Plug HandleClick
        override protected void HandleClick() { }
    }
}