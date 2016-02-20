using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_interactiveSwitchTWO : spt_baseInteractiveObject
    {

        private bool currentState = false;

        override protected void clickSuccess() {
            currentState = !currentState;
            if (currentState == true) Debug.Log("ON");
            else if (currentState == false) Debug.Log("OFF");
        }

        //Plug handleDown
        override protected void HandleDown() { }
    }
}
