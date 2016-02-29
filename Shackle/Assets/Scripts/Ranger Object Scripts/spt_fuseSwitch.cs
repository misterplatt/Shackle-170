/*
spt_fuseSwitch

Author(s): Hayden Platt,Dara Diba

Revision 1

Script for each fuse switch. Relays it's current state to
fuseManager's active array.
*/


using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_fuseSwitch : spt_baseInteractiveObject
    {

        public spt_fuseManager fManager;

        [SerializeField]
        private int switchNumber;

        private bool currentState = false;

        override protected void clickSuccess()
        {
            currentState = !currentState;
            fManager.updateFuseStates(switchNumber, currentState);
            if (currentState == true) transform.Translate(new Vector3(0,0,-.3f));
            else if (currentState == false) transform.Translate(new Vector3(0, 0, .3f));
        }

        //Plug handleDown
        override protected void HandleDown() { }
    }
}

