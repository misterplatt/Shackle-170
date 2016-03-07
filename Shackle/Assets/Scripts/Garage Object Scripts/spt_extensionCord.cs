/*
spt_extensionCord

Author(s): Hayden Platt

Revision 2

Plugs the cord into the wall when pressed,
and updates the extCordPlugged NPL state to true.
If unplugged by the monster, state is reset
*/

using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_extensionCord : spt_baseInteractiveObject
    {
        public static bool local_extCordPlugged = false;

        private bool once = false;
        private Transform inital;

        protected override void Start()
        {
            inital = transform;
        }

        //If the monster unplugs the extension cord, reset its position and allow it to be plugged in again 
        protected override void Update()
        {
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[3].state == false && once) {
                transform.position = inital.position;
                transform.rotation = inital.rotation;
                once = false;
            }
        }

        //Handle the Click event
        override protected void clickSuccess()
        {
            if (!once){
                //NPL Update
                spt_WorldState.worldStateChanged = true;
                local_extCordPlugged = true;

                transform.Translate(Vector3.up * 1);
                transform.eulerAngles = new Vector3(17, -180, 0); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
                once = true;
            }
        }

        //Plug HandleDown from base
        protected override void HandleDown() {}
    }
}