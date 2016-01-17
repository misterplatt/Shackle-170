using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_interactiveButton : MonoBehaviour
    {

        [SerializeField]
        private Material m_OverMaterial;
        [SerializeField]
        private Material m_DownMaterial;
        [SerializeField]
        private Material m_UpMaterial;

        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        [SerializeField]
        private Renderer m_Renderer;

        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnDown += HandleDown;
            m_InteractiveItem.OnUp += HandleUp;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnDown -= HandleDown;
            m_InteractiveItem.OnUp -= HandleUp;
        }

        //Handle the Over event
        private void HandleOver()
        {
            Debug.Log("Show over state");
            m_Renderer.material = m_OverMaterial;
        }

        //Handle the Down event
        private void HandleDown()
        {
            Debug.Log("Show down state");
            m_Renderer.material = m_DownMaterial;
        }

        //Handle the Up event
        private void HandleUp()
        {
            Debug.Log("Show up state");
            m_Renderer.material = m_UpMaterial;
        }

    }

}
    