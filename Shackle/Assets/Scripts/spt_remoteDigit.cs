using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_remoteDigit : MonoBehaviour
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
        bool currentState = false;

        public spt_remoteManager rManager;

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
            Debug.Log("Show click state");
            currentState = !currentState;
            if (currentState == true)
            {
                m_Renderer.material = m_StateTwoMaterial;
                rManager.enterChannelNumber(gameObject.name);
            }
            else if (currentState == false)
            {
                m_Renderer.material = m_StateOneMaterial;
            }
        }

        void deactivateButton()
        {
            m_Renderer.material = m_StateOneMaterial;
            currentState = false;
        }
    }
}