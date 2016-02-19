using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_interactiveItemManipulate : MonoBehaviour
    {

        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        [SerializeField]
        private Renderer m_Renderer;

        public float rotationSpeed = 70;
        public float lerpSpeed = 5;
        public float distanceBeforeLerp = .3f; //How far reticle can move from the object before the object lerps to it
        public float distanceBeforeFreeze = .01f; //How close the reticle must be to the object before stopping the lerp

        bool currentState = false;
        bool outOfView = false;
        private Vector3 startPoint;
        private Quaternion startRotation;
        public Transform endPoint;
        //public GameObject panelObj; //USE IF VIGNETTE IS WANTED

        private void OnEnable()
        {
            m_InteractiveItem.OnClick += HandleClick;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnClick -= HandleClick;
        }

        void Start() {
            //Store object's original position and rotation
            startPoint = transform.position;
            startRotation = transform.rotation;
            BroadcastMessage("childActive", false); //Deactivate all child colliders
        }

        //Called when the player looks at the object so it knows where to lerp to
        public void RetrieveLookPoint(GameObject raycastingPlayer)
        {
            endPoint = raycastingPlayer.transform.Find("VRCameraUI/InspectPoint");
        }

        void Update() {
            //If the object has been clicked (A button) in the world, lerp it in front of the player and enable rotation with right thumbstick
            if (currentState == true)
            {
                //panelObj.SetActive(true); //USE IF VIGNETTE IS WANTED
                //Debug.Log("Distance: " + Vector3.Distance(transform.position, endPoint.position));
                BroadcastMessage("childActive", true);
                //Only Lerp while reticle position is more than distanceBeforeLerp units away. Then, stop once reticle pos is less than than distanceBeforeFreeze
                if (Vector3.Distance(transform.position, endPoint.position) > distanceBeforeLerp) outOfView = true;
                if(outOfView == true) transform.position = Vector3.Lerp(transform.position, endPoint.position, Time.deltaTime * lerpSpeed);
                if (Vector3.Distance(transform.position, endPoint.position) < distanceBeforeFreeze) outOfView = false;

                //Rotate the object in the world space based on right thumbstick input
                transform.Rotate(new Vector3(spt_playerControls.rightThumb("Vertical"), spt_playerControls.rightThumb("Horizontal"), 0) * Time.deltaTime * rotationSpeed, Space.World);
            }

            //If B is pressed, return the object to it's default position and rotation
            else if (currentState == false && transform.position != startPoint) {
                BroadcastMessage("childActive", false);
                //panelObj.SetActive(false); //USE IF VIGNETTE IS WANTED
                transform.position = Vector3.Lerp(transform.position, startPoint, Time.deltaTime * lerpSpeed);
                transform.rotation = startRotation;
            }
            
            //Return object when button b button is pressed
            if (spt_playerControls.bButtonPressed() == true) {
                currentState = false;
                //m_Renderer.material = m_StateTwoMaterial;
            } 
        }

        //Handle the Click event, alternates states on every press
        private void HandleClick()
        {
            Debug.Log("Show click state");
            currentState = true;
            //m_Renderer.material = m_StateOneMaterial;
        }
    }
}
