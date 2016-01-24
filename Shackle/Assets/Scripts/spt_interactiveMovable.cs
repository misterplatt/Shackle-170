using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_interactiveMovable : MonoBehaviour
    {

        [SerializeField]
        private Material m_OverMaterial;
        [SerializeField]
        private Material m_DownMaterial;
        [SerializeField]
        private Material m_UpMaterial;

        private bool mouseHeld = false;
        public bool xAxis = true;
        public bool yAxis = false;
        public float moveSpeed = 1;

        void Update()
        {
            //when mouse is held use right thumbstick to move object based on object's axis boolean
            if (mouseHeld == true)
            {
                if (xAxis == true)
                {
                    transform.Translate(new Vector3(spt_playerControls.leftThumb("Horizontal"), 0, 0) * Time.deltaTime * moveSpeed);
                }
                else if (yAxis == true)
                {
                    transform.Translate(new Vector3(0, 0, spt_playerControls.leftThumb("Vertical")) * Time.deltaTime * moveSpeed);
                }
            }
            //stop moving when button is released
            if (spt_playerControls.aButtonPressed() == false) mouseHeld = false;
        }

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
            mouseHeld = true;
        }

        //Handle the Up event
        private void HandleUp()
        {
            Debug.Log("Show up state");
            m_Renderer.material = m_UpMaterial;
        }

    }

}