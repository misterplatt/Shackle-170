using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_extensionCord : spt_baseInteractiveObject
    {
        public static bool local_extCordPlugged = false;


        //Handle the Click event
         override protected void clickSuccess()
        {
                spt_WorldState.worldStateChanged = true;
                local_extCordPlugged = true;
                transform.eulerAngles = new Vector3(0, 90, -17); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
        }

        protected override void HandleDown() {}
    }
}