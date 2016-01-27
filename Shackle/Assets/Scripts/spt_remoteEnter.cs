﻿using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_remoteEnter : MonoBehaviour
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

        //Handle the Click event, alternates states on every press
        private void HandleDown()
        {
            //Debug.Log("Current Channel: " + spt_remoteManager.channelNumber[0] + " " + spt_remoteManager.channelNumber[1]);
            if (spt_remoteManager.channelNumber[0] == "1" && spt_remoteManager.channelNumber[1] == "2") {
                Debug.Log("TV IS ON!$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            }
            m_Renderer.material = m_StateTwoMaterial;
            BroadcastMessage("deactivateButton");
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