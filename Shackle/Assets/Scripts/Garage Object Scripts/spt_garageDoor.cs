using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_garageDoor : spt_baseInteractiveObject
    {
        public static bool local_garageDoorOpen;

        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            transform.Translate(new Vector3(0, 1, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
            spt_WorldState.worldStateChanged = true;
            local_garageDoorOpen = true;
            holding = false;
         }
    }
}

