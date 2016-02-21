/*
spt_interactiveItemMovableTWO

Author(s): Hayden Platt,Dara Diba

Revision 1

Child of the base interactiveObject class
Allows for an item to be switched from a false state to a true state and vice-versa.
*/


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
