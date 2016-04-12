/*
spt_combinationDial

Author(s): Hayden Platt & Dara Diba

Revision 2

Script which resides on each combo dial. Rotates
1/10, and updates the current code combo.
Audio added for the dial rotation
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    //Script that handles player interaction with digit buttons on the remote
    public class spt_combinationDial : spt_baseInteractiveObject
    {
        public spt_lockManager lManager;

        [SerializeField]
        private int dialNumber;
        [SerializeField]
        private int currentDigit;

        private AudioSource aSource;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        //public spt_comboManager cManager;

        //Function that activates all manipulation object's children's colliders on pickup, and deactivates on put down
        //Precon: There is a parent object that BroadcastMessage-calls this script
        public void childActive(bool state)
        {
            GetComponent<BoxCollider>().enabled = state;
        }

        //Handle the Click event, alternates states on every press
        override protected void clickSuccess()
        {
            aSource.Play();
            transform.Rotate(new Vector3(0, 18, 0));
            if (currentDigit == 9) currentDigit = 0;
            else currentDigit++;
            lManager.updateDialStates(dialNumber, currentDigit);
        }

        //Plug HandleDown function from base
        protected override void HandleDown() { }
    }
}