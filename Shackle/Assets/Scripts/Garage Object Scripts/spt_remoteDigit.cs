using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    //Script that handles player interaction with digit buttons on the remote
    public class spt_remoteDigit : spt_baseInteractiveObject
    {
        [SerializeField]
        private Material m_StateOneMaterial;
        [SerializeField]
        private Material m_StateTwoMaterial;

        [SerializeField]
        private Renderer m_Renderer;
        bool currentState = false;

        public spt_remoteManager rManager;

        //Function that activates all manipulation object's children's colliders on pickup, and deactivates on put down
        public void childActive(bool state)
        {
            GetComponent<CapsuleCollider>().enabled = state;
        }

        //Handle the Click event, alternates states on every press
        override protected void clickSuccess()
        {
            Debug.Log("Show click state");
            currentState = !currentState;
            //Highlight digit button and send it's number to remoteManager
            if (currentState == true)
            {
                m_Renderer.material = m_StateTwoMaterial;
                rManager.enterChannelNumber(gameObject.name);
            }
            //Un-highlight digit button and remove it's number from remoteManager
            else if (currentState == false)
            {
                m_Renderer.material = m_StateOneMaterial;
                rManager.enterChannelNumber("-");
            }
        }

        //Function that gets broadcast by spt_remoteManager and spt_remoteEnter to 
        //revert digit material, indicating it has been unselected
        void deactivateDigit()
        {
            m_Renderer.material = m_StateOneMaterial;
            currentState = false;
        }

        protected override void HandleDown(){}
    }
}