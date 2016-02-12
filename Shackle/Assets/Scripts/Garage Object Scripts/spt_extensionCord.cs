using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_extensionCord : MonoBehaviour
    {
        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        bool once = false;

        private void OnEnable()
        {
            m_InteractiveItem.OnClick += HandleClick;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnClick -= HandleClick;
        }

        //Handle the Click event
        private void HandleClick()
        {
            if (!once)
            {
                transform.eulerAngles = new Vector3(0, 90, -17); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
                once = true;
            }
        }
    }
}