/*
spt_extensionCord

Author(s): Hayden Platt

Revision 2

Plugs the cord into the wall when pressed,
and updates the extCordPlugged NPL state to true.
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

        //Handle the Click event
         override protected void clickSuccess()
        {
            spt_WorldState.worldStateChanged = true;
            local_extCordPlugged = true;
            transform.Translate(Vector3.up * 1);
            transform.eulerAngles = new Vector3(17, -180, 0); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
        }

        //Plug HandleDown from base
        protected override void HandleDown() {}
    }
}