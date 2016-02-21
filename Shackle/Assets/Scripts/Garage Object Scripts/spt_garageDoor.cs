/*
spt_garageDoor

Author(s): Hayden Platt

Revision 2

Open the garage if opener is used on door for holdTime seconds.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_garageDoor : spt_baseInteractiveObject
    {
        public static bool local_garageDoorOpen;

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            transform.Translate(new Vector3(0, 1, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
            spt_WorldState.worldStateChanged = true;
            local_garageDoorOpen = true;
            holding = false;
         }

        //Plug HandleClick
        override protected void HandleClick() { }
    }
}

