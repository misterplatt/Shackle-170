using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_remotePower : NetworkBehaviour
    {
        public static bool TVPowered = false;

        [SerializeField]
        private Material m_StateOneMaterial;
        [SerializeField]
        private Material m_StateTwoMaterial;


        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        [SerializeField]
        private Renderer m_Renderer;

        private bool currentState = false;
        public GameObject TV_static;

        private void OnEnable()
        {
            m_InteractiveItem.OnClick += HandleClick;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnClick -= HandleClick;
        }

        //Handle the Click event, alternates states on every press
        private void HandleClick()
        {
            //If the puzzle state extCordPlugged is true, modify the TVPowered state
            if (GameObject.Find("PuzzleStates").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[0].state == true) {
                currentState = !currentState;
                spt_WorldState.worldStateChanged = true;
                TVPowered = currentState;
                if (currentState == true) m_Renderer.material = m_StateOneMaterial;
                else m_Renderer.material = m_StateTwoMaterial;

                TV_static.SetActive(true); //PLACEHOLDER UNTIL NETWORK LOGIC*******************************
            }
        }
    }
}