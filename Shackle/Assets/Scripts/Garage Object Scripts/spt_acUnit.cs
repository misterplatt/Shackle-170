using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_acUnit : spt_baseInteractiveObject
    {
        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            GameObject.Find("screwDoor").transform.Translate(new Vector3(-1.4f, 0, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED  
        }
    }
}