/*
spt_keypad

Author(s): Hayden Platt

Revision 1

Simple script which moves the lab door on holdsuccess
with the ID card.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_keypad : spt_baseInteractiveObject
    {
        private bool once = false;

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            if (!once) {
                GameObject.Find("Door").transform.Translate(Vector3.left * 1.6f);
                holding = false;
            }

        }

        //Plug HandleClick
        override protected void HandleClick() { }

        //Brief function to be invoked on matchbox interaction
        void DestroyPoster()
        {
            Destroy(GameObject.Find("Poster"));
        }
    }
}