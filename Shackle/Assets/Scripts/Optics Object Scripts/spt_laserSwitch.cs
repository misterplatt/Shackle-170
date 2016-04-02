/*
spt_laserSwitch

Author(s): Hayden Platt, Dara Diba

Revision 1

Child of the base interactiveObject class
Allows for an item to be switched from a false state to a true state and vice-versa.
*/


using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_laserSwitch : spt_baseInteractiveObject
    {
        private bool currentState = false;

        override protected void clickSuccess()
        {
            currentState = !currentState;
            Debug.Log("CLICK");
            if (currentState == true) transform.FindChild("Laser_Proj").gameObject.SetActive(true);
            else if (currentState == false) transform.FindChild("Laser_Proj").gameObject.SetActive(false);
        }

        //Plug handleDown
        override protected void HandleDown() { }
    }
}