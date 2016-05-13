/* VREyeRaycaster
 * 
 * 
 * Last Revision Date: 4/3/2016
 * 
 * In order to interact with objects in the scene
 * this class casts a ray into the scene and if it finds
 * a VRInteractiveItem it exposes it for other classes to use.
 * This script should be generally be placed on the camera.
 * Added reticle range - Dara
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace VRStandardAssets.Utils
{

    public class VREyeRaycaster : NetworkBehaviour
    {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.
        public bool heldSuccess;

        [SerializeField] private Transform m_Camera;
        [SerializeField] private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField] private Reticle m_Reticle;                     // The reticle, if applicable.
        [SerializeField] private VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_DebugRayLength = 7f;           // Debug ray length.
        [SerializeField] private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
        [SerializeField] private float m_RayLength = 500f;              // How far into the scene the ray is cast.
        [SerializeField] public Vector3 rhit = new Vector3();
        
        private VRInteractiveItem m_CurrentInteractible;                //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item
        public bool racyCastTouch;
        public bool isSingle;

        public string currentInteractibleName; //For Ryan

        //Network Intractable Informtation
        private string lastInteractableName;
        private Vector3 lastPosition;
        private Quaternion lastRot;

        // Utility for other classes to get the current interactive item
        public VRInteractiveItem CurrentInteractible
        {
            get { return m_CurrentInteractible; }
        }

        
        private void OnEnable()
        {
            m_VrInput.OnClick += HandleClick;
            m_VrInput.OnDoubleClick += HandleDoubleClick;
            m_VrInput.OnUp += HandleUp;
            m_VrInput.OnDown += HandleDown;
        }


        private void OnDisable ()
        {
            m_VrInput.OnClick -= HandleClick;
            m_VrInput.OnDoubleClick -= HandleDoubleClick;
            m_VrInput.OnUp -= HandleUp;
            m_VrInput.OnDown -= HandleDown;
        }

        // Checks to see what the active scene is and as a result will change the ray length
        private void Start()
        {
            heldSuccess = false;
            if (isSingle) this.enabled = true;
            if (SceneManager.GetActiveScene().name == "net_SpookyGarage") m_RayLength = 500f;
            else m_RayLength = 750f;

            lastInteractableName = "";
            lastPosition = Vector3.zero;
            lastRot = Quaternion.identity;
        }

        private void Update()
        {
            if (!isLocalPlayer) {
                return;
            }
            EyeRaycast();
            updateInteractable();
        }

        //This function checks if an object is being interacted with and if the position/transform has changed
        //significantly enough to warrant packet transmission and sync.

              
        private void EyeRaycast()
        {
            // Show the debug ray if required
            if (m_ShowDebugRay)
            {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(m_Camera.position, m_Camera.forward);
            RaycastHit hit;
            
            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {
                //set flashlight reference object transform
                GameObject flashlightObj = transform.Find("Camera Player/pFlashLight").gameObject;
                flashlightObj.transform.position = hit.point;

                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                m_CurrentInteractible = interactible;

                //If the object hit by the raycast has a VRInteractiveItem Script, tell that object to get the inventory script from the raycasting player
                if (interactible != null)
                {

                    if (currentInteractibleName == "") {
                        lastPosition = interactible.gameObject.transform.position;
                        lastRot = interactible.gameObject.transform.rotation;
                    }
                    currentInteractibleName = interactible.gameObject.name; //For Ryan
                    

                    //This will most likely need a Cmd written for it. Let me test it first -R
                    hit.transform.SendMessage("RetrieveInventoryScript", gameObject);

                    //If there is a manipulate script attached to the object you're looking at, send it the player's inspect point transform
                    if (hit.transform.gameObject.GetComponent<VRStandardAssets.Examples.spt_interactiveItemManipulate>() != null) hit.transform.SendMessage("RetrieveLookPoint", gameObject);
                }
                else resetCurrentInter();

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != m_LastInteractible)
                    interactible.Over(); 

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                    DeactiveLastInteractible();

                m_LastInteractible = interactible;

                // Something was hit, set at the hit position.
                if (m_Reticle)
                {
                    m_Reticle.SetPosition(hit);
                    racyCastTouch = true;
                }

                if (OnRaycasthit != null)
                {
                    OnRaycasthit(hit);
                }
                    
            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;
                racyCastTouch = false;

                // Position the reticle at default distance.
                if (m_Reticle)
                    m_Reticle.SetPosition();
            }
        }

        private void DeactiveLastInteractible()
        {
            if (m_LastInteractible == null)
                return;

            m_LastInteractible.Out();
            m_LastInteractible = null;
        }


        private void HandleUp()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }


        private void HandleDown()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Down();
        }


        private void HandleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }


        private void HandleDoubleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.DoubleClick();

        }

        private void updateInteractable()
        {
            if (isServer) return;
            if (currentInteractibleName.Contains("mirrorHandle")) {
                if(spt_playerControls.aButtonPressed()) {
                    Debug.Log("Rotating");
                    GameObject mirror = GameObject.Find(currentInteractibleName);
                    Cmd_InteractableMove(currentInteractibleName, mirror.transform.position, mirror.transform.rotation);//spt_playerControls.leftThumb("Horizontal"));
                    Cmd_UpdateMirrors(laserStatus());
                }
            }
            else if (currentInteractibleName.Contains("mirrorStand") && heldSuccess)
            {
                Debug.Log("attaching mirror");
                spt_inventory pInv = GetComponent<spt_inventory>();
                GameObject mirror = pInv.retrieveObjectFromInventory(pInv.activeItem);

                int itemIndex = 0;
                foreach(string item in pInv.inventory)
                {
                    Debug.Log(item);
                    if (pInv.inventory[itemIndex].Contains("mirror")) break;
                    itemIndex++;
                }

                if (pInv.inventory[itemIndex].Contains("mirrorPickup"))
                {
                    Debug.Log("firing command to add mirror");
                    Cmd_AddMirror(pInv.inventory[itemIndex], currentInteractibleName);
                    pInv.removeItm(pInv.inventory[itemIndex]);
                    Cmd_UpdateMirrors(laserStatus());
                }

                //if stand check if mirror on it, if not take mirror from inventory, add to stand on server.
                heldSuccess = false;
            }
            else if (currentInteractibleName.Contains("mirrorPickup") && spt_playerControls.aButtonPressed())
            {
                GameObject mirror = GameObject.Find(currentInteractibleName);
                if (mirror.transform.parent.name.Contains("mirrorHandle")) return;
                Cmd_SendMirrorToHell(currentInteractibleName);
            }
        }        

        private void resetCurrentInter()
        {
            currentInteractibleName = "";
            lastPosition = Vector3.zero;
            lastRot = Quaternion.identity;
        }

        /*
        [Command]
        public void Cmd_InteractableMove(string name, float amount) {
            GameObject mirror = GameObject.Find(name);
            float rotateSpeed = 10;
            Debug.Log("Rotating : " + name + " by : " + amount);

            float maxNegativeRotation = mirror.GetComponent<VRStandardAssets.Examples.spt_mirrorHandle>().maxNegativeRotation;
            float maxPositiveRotation = mirror.GetComponent<VRStandardAssets.Examples.spt_mirrorHandle>().maxPositiveRotation;

            Vector3 initialRotation = mirror.transform.rotation.eulerAngles;
            Vector3 newRotation = mirror.transform.rotation.eulerAngles;
            mirror.transform.root.GetComponent<NetworkTransformChild>().enabled = false;
            Debug.Log("Initial : " + mirror.transform.rotation.eulerAngles);
            mirror.transform.Rotate(Vector3.up * -amount * Time.deltaTime * rotateSpeed);
            Debug.Log("After : " + mirror.transform.rotation.eulerAngles);
            //reactiveNetworkTransform( mirror.transform.root.GetComponent<NetworkTransformChild>() );

            //get room and mark dirty bits, oh my.
            //mirror.transform.root.gameObject.GetComponent<NetworkTransformChild>().SetDirtyBit();
        }
        */
        private IEnumerable reactiveNetworkTransform( NetworkTransformChild obj )
        {
            yield return new WaitForSeconds(5);
            obj.enabled = true;
        }

        //Function which allows us to limit rotation in the negative direction
        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < 90 || angle > 270)
            {
                if (angle > 180) angle -= 360;
                if (max > 180) max -= 360;
                if (min > 180) min -= 360;
            }
            angle = Mathf.Clamp(angle, min, max);
            if (angle < 0) angle += 360;
            Debug.Log("Angle : " + angle);
            return angle;
        }


        //Generate Bool array for mirrors
        public bool[] laserStatus()
        {
            GameObject[] mirrors = GameObject.FindGameObjectsWithTag("mirror");
            bool[] output = new bool[mirrors.Length];

            int index = 0;
            foreach (GameObject mirror in mirrors)
            {
                output[index++] = mirror.GetComponent<spt_mirrorSync>().emitsLaser;
            }

            return output;
        }

        //Clientside function to send mirror updates
        public void Client_MirrorSync()
        {


        }

        //Second option for updating mirrors in net_optics
        [Command]
        public void Cmd_SyncMirrors()
        {
        }

        
        //Command for updating an interactable location on server.
        [Command]
        public void Cmd_InteractableMove( string objectName, Vector3 position, Quaternion rotation)
        {
            Debug.Log("InteractionUpdating");
            GameObject objWithInteraction = GameObject.Find(objectName);

            objWithInteraction.transform.position = position;
            objWithInteraction.transform.rotation = rotation;
        }

        //command to update mirror emit's lasters from client to host.
        [Command]
        public void Cmd_UpdateMirrors( bool[] mirrorLasers )
        {
            GameObject[] mirrors = GameObject.FindGameObjectsWithTag("mirror");

            int index = 0;
            foreach ( GameObject mirror in mirrors )
            {
                mirror.GetComponent<spt_mirrorSync>().emitsLaser = mirrorLasers[index++];
            }

        }

        [Command]
        public void Cmd_AddMirror( string mirrorName, string standName )
        {
            Debug.Log("I'm adding a mirror, I tink.        ");
            Debug.Log("mirrorName : " + mirrorName);
            GameObject mirror = GameObject.Find(mirrorName);
            GameObject stand = GameObject.Find(standName);

            foreach (Transform child in stand.transform)
            {
                if (child.gameObject.tag == "mirrorHandle")
                {
                    Debug.Log("Mirror Found : " + child.gameObject.name);
                    Debug.Log("Attaching " + mirror.name + " to " + child.gameObject.name);
                    mirror.transform.position = new Vector3(stand.transform.position.x, stand.transform.position.y + 1.3f, stand.transform.position.z);
                    mirror.transform.parent = child;
                    break;
                }
            }
            //mirror.transform.parent = stand.transform.FindChild();
        }

        [Command]
        public void Cmd_SendMirrorToHell( string mirrorName ) {
            Debug.Log("Removing : " + mirrorName);
            GameObject mirror = GameObject.Find(mirrorName);
            mirror.transform.position = Vector3.down * 1000;
        }

        
    }
}