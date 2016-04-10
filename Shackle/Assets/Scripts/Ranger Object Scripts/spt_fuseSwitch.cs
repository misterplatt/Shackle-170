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
        private Vector3 initalPosition;

        [SerializeField]
        private int switchNumber;

        private bool currentState = false;

        protected override void Start()
        {
            initalPosition = transform.position;
        }

        override protected void clickSuccess()
        {
            currentState = !currentState;
            fManager.updateFuseStates(switchNumber, currentState);
            if (currentState == true) transform.Translate(new Vector3(0,0,-.3f));
            else if (currentState == false) transform.Translate(new Vector3(0, 0, .3f));
        }

        //Plug handleDown
        override protected void HandleDown() { }

        //Function called by fuseManager when the monster attacks. Randomly sets fuses state, and moves it accordingly
        public void randomToggle() {
            currentState = (Random.value > 0.5f);
            fManager.updateFuseStates(switchNumber, currentState);
            transform.position = initalPosition; //Resets the position before performing the translation
            if (currentState == true) transform.Translate(new Vector3(0, 0, -.3f));
            else if (currentState == false) transform.Translate(new Vector3(0, 0, .3f));
        }
    }
}

