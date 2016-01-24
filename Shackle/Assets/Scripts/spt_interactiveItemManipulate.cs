using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_interactiveItemManipulate : MonoBehaviour
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

        public float rotationSpeed = 70;
        public float lerpSpeed = 5;
        bool currentState = false;
        private Vector3 startPoint;
        private Quaternion startRotation;
        public Transform endPoint;

        private void OnEnable()
        {
            m_InteractiveItem.OnClick += HandleClick;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnClick -= HandleClick;
        }

        void Start() {
            //Store object's original position
            startPoint = transform.position;
            startRotation = transform.rotation;
        }

        void Update() {
            if (currentState == true)
            {
                transform.position = Vector3.Lerp(transform.position, endPoint.position, Time.deltaTime * lerpSpeed);
                transform.Rotate(new Vector3(spt_playerControls.rightThumb("Vertical"), spt_playerControls.rightThumb("Horizontal"), 0) * Time.deltaTime * rotationSpeed, Space.World);
            }
            else if (currentState == false) {
                Debug.Log("SENDBACK");
                transform.position = Vector3.Lerp(transform.position, startPoint, Time.deltaTime * lerpSpeed);
                transform.rotation = startRotation;
            }
            Debug.Log(startPoint);
            //stop moving when button b button is pressed
            if (spt_playerControls.bButtonPressed() == true) {
                currentState = false;
                m_Renderer.material = m_StateTwoMaterial;
            } 
        }

        //Handle the Click event, alternates states on every press
        private void HandleClick()
        {
            Debug.Log("Show click state");
            currentState = true;
            m_Renderer.material = m_StateOneMaterial;
        }
    }
}
