using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_interactiveGatedHolder : MonoBehaviour
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

        // private bool m_GazeOver;
        public spt_inventory inventoryScript;
        public string gateItemName;
        public float holdTime;
        private float timer = 0;
        private bool holding = false;

        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnDown += HandleDown;
            m_InteractiveItem.OnUp += HandleUp;
            m_InteractiveItem.OnOut += HandleOut;

        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnDown -= HandleDown;
            m_InteractiveItem.OnUp -= HandleUp;
            m_InteractiveItem.OnOut -= HandleOut;

        }

        //Handle the Over event
        private void HandleOver()
        {
            Debug.Log("Show over state");
            m_Renderer.material = m_OverMaterial;
            // m_GazeOver = true;
        }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        private void HandleDown()
        {
            // User must press A to interact with the object, negates the case of user holding A previous to interaction
            if (Input.GetButtonDown("aButton") && inventoryScript.activeItem.Value.name == gateItemName)
            {
                holding = true;
                Debug.Log("Show down state");
                //m_Renderer.material = m_DownMaterial;
            }
            if (holding)
            {
                timer += Time.deltaTime;
                if (timer >= holdTime || holdTime == 0) {
                    m_Renderer.material = m_DownMaterial;
                }
            }
        }

        //Handle the Up event
        private void HandleUp()
        {
            Debug.Log("Show up state");
            m_Renderer.material = m_UpMaterial;
            holding = false;
        }

        private void HandleOut()
        {
            // When the user looks away from the rendering of the scene, hide the radial.
            // m_GazeOver = false;
            m_Renderer.material = m_UpMaterial;
            holding = false;
        }

    }

}