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
        public bool zAxis = false;
        public float moveSpeed = 1;
        public float maxNegativeDistance = 0;
        public float maxPositiveDistance = 2;

        public Vector3 initialPosition; //FOR LIMITER

        void Start() {
            initialPosition = transform.position; //FOR LIMITER
            Debug.LogWarning("Initial: " + initialPosition + " From Transform: " + transform.position);
        }

        void Update()
        {
            //when mouse is held use right thumbstick to move object based on object's axis boolean
            if (mouseHeld == true)
            {
                if (xAxis == true)
                {
                    transform.Translate(new Vector3(spt_playerControls.leftThumb("Horizontal"), 0, 0) * Time.deltaTime * moveSpeed);
                    //Clamps the object's X position to stay between the starting value +maxPosDistance or -maxMinDistance
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, initialPosition.x - maxNegativeDistance, initialPosition.x + maxPositiveDistance), transform.position.y, transform.position.z); //LIMITER: NOT WORKING
                }
                else if (zAxis == true)
                {
                    transform.Translate(new Vector3(0, 0, spt_playerControls.leftThumb("Vertical")) * Time.deltaTime * moveSpeed);
                    //Clamps the object's z position to stay between the starting value +maxPosDistance or -maxMinDistance
                    transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, initialPosition.z - maxNegativeDistance, initialPosition.z + maxPositiveDistance)); //LIMITER: NOT WORKING
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
            if(m_OverMaterial != null) m_Renderer.material = m_OverMaterial;
        }

        //Handle the Down event
        private void HandleDown()
        {
            if (m_DownMaterial != null) m_Renderer.material = m_DownMaterial;
            mouseHeld = true;
        }

        //Handle the Up event
        private void HandleUp()
        {
            if (m_UpMaterial != null) m_Renderer.material = m_UpMaterial;
        }

    }

}