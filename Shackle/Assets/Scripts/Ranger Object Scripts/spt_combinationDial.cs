/*
spt_combinationDial

Author(s): Hayden Platt

Revision 1

Script which resides on each combo dial. Rotates
1/10, and updates the current code combo.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    //Script that handles player interaction with digit buttons on the remote
    public class spt_combinationDial : spt_baseInteractiveObject
    {
        bool currentState = false;

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
            transform.Rotate(new Vector3(0, 18, 0));
            Debug.Log("Show click state");
            currentState = !currentState;
            //Highlight digit button and send it's number to remoteManager
            if (currentState == true)
            {
                //cManager.enterChannelNumber(gameObject.name);
            }
            //Un-highlight digit button and remove it's number from remoteManager
            else if (currentState == false)
            {
                //cManager.enterChannelNumber("-");
            }
        }

        //Plug HandleDown function from base
        protected override void HandleDown() { }
    }
}