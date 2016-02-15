using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_extensionCord : NetworkBehaviour
    {
        public static bool local_extCordPlugged = false;

        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        bool once = false;

        private void OnEnable()
        {
            m_InteractiveItem.OnClick += HandleClick;
        }

        void Update() {
            if (local_extCordPlugged) Debug.Log("Triggered");
        }

        private void OnDisable()
        {
            m_InteractiveItem.OnClick -= HandleClick;
        }

        //Handle the Click event
        private void HandleClick()
        {
            if (!once){
                spt_WorldState.worldStateChanged = true;
                local_extCordPlugged = true;
                transform.eulerAngles = new Vector3(0, 90, -17); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
                once = true;
            }
        }
    }
}