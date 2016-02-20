using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_garageLock : spt_baseInteractiveObject
    {
        public static bool garageDoorUnlocked = false;

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            garageDoorUnlocked = true;
            transform.Translate(new Vector3(-.15f, 0, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
        }
    }
}