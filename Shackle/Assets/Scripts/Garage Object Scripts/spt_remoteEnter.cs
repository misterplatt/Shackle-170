using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_remoteEnter : MonoBehaviour
    {
        public static bool correctChannel = false;

        [SerializeField]
        private Material m_StateOneMaterial;
        [SerializeField]
        private Material m_StateTwoMaterial;


        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        [SerializeField]
        private Renderer m_Renderer;
        bool currentState = false;
        private bool m_GazeOver;

        private void OnEnable()
        {
            m_InteractiveItem.OnDown += HandleDown;
            m_InteractiveItem.OnUp += HandleUp;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnDown -= HandleDown;
            m_InteractiveItem.OnUp -= HandleUp;
        }

        //Function that activates all manipulation object's children's colliders on pickup, and deactivates on put down
        public void childActive(bool state) {
            GetComponent<CapsuleCollider>().enabled = state;
        }

        //Handle the Click event, alternates states on every press
        private void HandleDown()
        {
            //If the TV is powered on and the input channel number is 49, set correctChannel puzzle state to true
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[1].state == true)
            {
                if (spt_remoteManager.channelNumber[0] == "4" && spt_remoteManager.channelNumber[1] == "9")
                {
                    spt_WorldState.worldStateChanged = true;
                    correctChannel = true;
                    Debug.Log("TV IS ON!$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                    GameObject.Find("mdl_garageOpener").GetComponent<Rigidbody>().useGravity = true; //MOVE TO A SCRIPT ON GARAGE OPENER
                }
            }
            //If the player presses enter with the correct player number in remote manager, change TV to green channel
            //Highlight button briefly, deactivate all digits, and clear channel number
            m_Renderer.material = m_StateTwoMaterial;
            BroadcastMessage("deactivateDigit");
            spt_remoteManager.clearChannelNumber();
        }

        //Handle the Up event
        private void HandleUp()
        {
            m_Renderer.material = m_StateOneMaterial;
            Debug.Log("Released ENTER");
        }
    }
}