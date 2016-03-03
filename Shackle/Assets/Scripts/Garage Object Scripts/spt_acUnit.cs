/*
spt_acUnit

Author(s): Hayden Platt

Revision 3

Opens the AC unit once the screwdriver is used on all 4 screws
for holdTime.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_acUnit : spt_baseInteractiveObject
    {
        public static int screwsLeft = 4;

        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            //Deactivate the current screw game object, and decrement the screwsLeft count. Once zero, open the door.
            screwsLeft--;
            gameObject.SetActive(false);
            if(screwsLeft <= 0) GameObject.Find("screwDoor").transform.Translate(new Vector3(-1.4f, 0, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED  
        }
    }
}