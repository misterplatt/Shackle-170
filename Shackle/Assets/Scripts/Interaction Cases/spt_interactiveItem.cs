using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    // This script is a simple example of how an interactive item can
    // be used to change things on gameobjects by handling events.
    public class spt_interactiveItem : MonoBehaviour
    {
        [SerializeField]
        private Material m_NormalMaterial;
        [SerializeField]
        private Material m_OverMaterial;
        [SerializeField]
        private Material m_ClickedMaterial;
        [SerializeField]
        private Material m_DoubleClickedMaterial;
        [SerializeField]
        private Material m_UpMaterial;
        [SerializeField]
        private Material m_DownMaterial;
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        [SerializeField]
        private Renderer m_Renderer;


        private void Awake()
        {
            m_Renderer.material = m_NormalMaterial;
        }


        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_InteractiveItem.OnClick += HandleClick;
            m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
            m_InteractiveItem.OnUp += HandleUp;
            m_InteractiveItem.OnDown += HandleDown;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_InteractiveItem.OnClick -= HandleClick;
            m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
            m_InteractiveItem.OnUp -= HandleUp;
            m_InteractiveItem.OnDown -= HandleDown;
        }


        //Handle the Over event
        private void HandleOver()
        {
            Debug.Log("Show over state");
            m_Renderer.material = m_OverMaterial;
        }


        //Handle the Out event
        private void HandleOut()
        {
            Debug.Log("Show out state");
            m_Renderer.material = m_NormalMaterial;
        }


        //Handle the Click event
        private void HandleClick()
        {
            Debug.Log("Show click state");
            m_Renderer.material = m_ClickedMaterial;
        }


        //Handle the DoubleClick event
        private void HandleDoubleClick()
        {
            Debug.Log("Show double click");
            m_Renderer.material = m_DoubleClickedMaterial;
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
