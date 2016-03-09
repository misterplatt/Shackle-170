/*
spt_garageLock

Author(s): Hayden Platt

Revision 2

When the key is used on the lock for holdTime seconds,
the lock's slide is moved out from within the garage.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_garageLock : spt_baseInteractiveObject
    {
        public static bool local_garageDoorUnlocked = false;

        private bool once = false;

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            if (!once)
            {
                local_garageDoorUnlocked = true;
                spt_WorldState.worldStateChanged = true;
                transform.Translate(new Vector3(-.03f, 0, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
                once = true;
            }
        }
    }
}