using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_remotePower : MonoBehaviour
    {
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
            //PLACEHOLDER UNTIL NETWORK LOGIC*****************************************************************
            if (GameObject.Find("Extension_Cord").transform.rotation.z < 0) {
                currentState = !currentState;
                if (currentState == true)
                {
                    m_Renderer.material = m_StateOneMaterial;
                    TV_static.SetActive(true); //PLACEHOLDER UNTIL NETWORK LOGIC*******************************
                }
                else if (currentState == false)
                {
                    m_Renderer.material = m_StateTwoMaterial;
                    TV_static.SetActive(false); //PLACEHOLDER UNTIL NETWORK LOGIC*******************************
                }
            }
        }
    }
}