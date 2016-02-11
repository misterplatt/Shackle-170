using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_acUnit : MonoBehaviour
    {

        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;

        // private bool m_GazeOver;
        public spt_inventory inventoryScript;
        public string gateItemName;
        public float holdTime;
        private float timer = 0;
        private bool holding = false;

        private void OnEnable()
        {
            m_InteractiveItem.OnDown += HandleDown;
            m_InteractiveItem.OnUp += HandleUp;
            m_InteractiveItem.OnOut += HandleOut;

        }


        private void OnDisable()
        {
            m_InteractiveItem.OnDown -= HandleDown;
            m_InteractiveItem.OnUp -= HandleUp;
            m_InteractiveItem.OnOut -= HandleOut;

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
            if (holding || holdTime == 0)
            {
                timer += Time.deltaTime;
                if (timer >= holdTime || holdTime == 0)
                {
                    GameObject.Find("screwDoor").transform.Translate(new Vector3(0, 0, -2)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
                }
            }
        }

        //Handle the Up event
        private void HandleUp()
        {
            holding = false;
        }

        private void HandleOut()
        {
            // When the user looks away from the rendering of the scene, hide the radial.
            // m_GazeOver = false;
            holding = false;
        }

    }

}