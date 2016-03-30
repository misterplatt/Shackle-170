/*
spt_remotePower

Author(s): Hayden Platt

Revision 1

Sets the NPL state TVOn if the extCordPlugged NPL state is true.
*/


using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_remotePower : spt_baseInteractiveObject
    {
        public static bool local_TVpowerState = false;

        [SerializeField]
        private Material m_StateOneMaterial;
        [SerializeField]
        private Material m_StateTwoMaterial;
        [SerializeField]
        private Renderer m_Renderer;

        private bool currentState = false;

        //Function that activates all manipulation object's children's colliders on pickup, and deactivates on put down
        public void childActive(bool state)
        {
            GetComponent<BoxCollider>().enabled = state;
        }

        protected override void Update()
        {
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[3].state == false){
                m_Renderer.material = m_StateTwoMaterial;
            }
        }

        //Handle the Click event, alternates states on every press
        override protected void clickSuccess()
        {
            //If the puzzle state extCordPlugged is true, modify the TVPowered state
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[3].state == true) {
                currentState = !currentState;
                local_TVpowerState = currentState;
                Debug.Log("tvPowerState : " + local_TVpowerState);
                spt_WorldState.worldStateChanged = true;
                if (currentState == true) m_Renderer.material = m_StateOneMaterial;
                else m_Renderer.material = m_StateTwoMaterial;
            }
        }

        override protected void HandleDown(){}
    }
}