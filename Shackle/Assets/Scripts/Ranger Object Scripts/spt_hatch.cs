/*
spt_hatch

Author(s): Hayden Platt

Revision 1

Opens the fusebox door if square key is used on
it for holdTime.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_hatch : spt_baseInteractiveObject
    {
        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            transform.Translate(new Vector3(0, 2, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED  
        }
    }
}