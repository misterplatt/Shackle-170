/*
spt_remoteEnter

Author(s): Hayden Platt, Dara Diba

Revision 2

Checks the current channel number when pressed.
If it is 49, then set NPL state correctChannel to true.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_remoteEnter : spt_baseInteractiveObject
    {
        public static bool local_correctChannelEntered = false;

        [SerializeField]
        private Material m_StateOneMaterial;
        [SerializeField]
        private Material m_StateTwoMaterial;


        [SerializeField]
        private Renderer m_Renderer;

        //Function that activates all manipulation object's children's colliders on pickup, and deactivates on put down
        public void childActive(bool state) {
            GetComponent<BoxCollider>().enabled = state;
        }

        //Handle the Click event, alternates states on every press
        override protected void holdSuccess()
        {
            //If the TV is powered on and the input channel number is 49, set correctChannel puzzle state to true
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true)
            {
                if (spt_remoteManager.channelNumber[0] == "4" && spt_remoteManager.channelNumber[1] == "9")
                {
                    spt_WorldState.worldStateChanged = true;
                    local_correctChannelEntered = true;
                }
            }
            //If the player presses enter with the correct player number in remote manager, change TV to green channel
            //Highlight button briefly, deactivate all digits, and clear channel number
            m_Renderer.material = m_StateTwoMaterial;
            BroadcastMessage("deactivateDigit");
            spt_remoteManager.clearChannelNumber();
        }

        //Handle the Up event
        override protected void HandleUp()
        {
            m_Renderer.material = m_StateOneMaterial;
            Debug.Log("Released ENTER");
        }

        //Plugs the HandleClick function from base
        protected override void HandleClick(){}
    }
}