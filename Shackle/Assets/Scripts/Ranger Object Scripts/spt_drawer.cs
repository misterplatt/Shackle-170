/*
spt_drawer

Author(s): Hayden Platt, Lauren Cunningham

Revision 1

Drawer specific movable script, with custom reset function
and puzzle state flipping.
*/


using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_drawer : spt_baseInteractiveObject
    {
        private bool buttonHeld = false;
        private bool moved = false;
        private Vector3 initialPosition;
        public AudioClip movingSound;
        public AudioClip slamSound;
        private AudioSource aSource;
        private bool once;
        public int HaydenIdea = 1;

        //Speed at which the object should move
        public float moveSpeed = 1;

        //Flags for which axes the object should translate upon
        public bool moveOnLocalX = true;
        public bool moveOnLocalY = false;
        public bool moveOnLocalZ = false;

        //Flags for which axes should be clamped
        public bool clampGlobalX = true;
        public bool clampGlobalY = false;
        public bool clampGlobalZ = false;

        //Clamp values for each axis. If axis is not being clamped, simply leave as zero
        public float x_maxNegativeDistance = 0;
        public float x_maxPositiveDistance = 0;
        public float y_maxNegativeDistance = 0;
        public float y_maxPositiveDistance = 0;
        public float z_maxNegativeDistance = 0;
        public float z_maxPositiveDistance = 0;
        public GameObject optional_movePathImage;

        override protected void Start()
        {
            initialPosition = transform.position;
            aSource = GetComponent<AudioSource>();
            if (movingSound != null) aSource.clip = movingSound;
            once = false;
        }

        override protected void Update()
        {
            //When A is held, use left thumbstick to move object based on object's axis boolean
            if (buttonHeld == true)
            {
                //Garage only, stops showing an object's movepath once it has been moved
                if ((gameObject.name == "mdl_bucket" && transform.position.z > 4.4f) || (gameObject.name == "mdl_box" && transform.position.x < 2.0f)) moved = true;
                //Displays a movable's movePath sprite if specified
                if (optional_movePathImage != null && !moved) optional_movePathImage.GetComponent<SpriteRenderer>().enabled = true;

                Vector3 newPos = transform.position; //Vector which handles and clamps

                if (movingSound != null)
                {
                    if ((moveOnLocalX == true && spt_playerControls.leftThumb("Horizontal") != 0) || ((moveOnLocalY == true || moveOnLocalZ == true) && spt_playerControls.leftThumb("Vertical") != 0))
                    {
                        if (!once)
                        {
                            aSource.Play();
                            once = true;
                        }
                    }
                }

                //Moves object on appropriate axes
                if (moveOnLocalX == true) transform.Translate(new Vector3(spt_playerControls.leftThumb("Horizontal"), 0, 0) * Time.deltaTime * moveSpeed * HaydenIdea);
                if (moveOnLocalY == true) transform.Translate(new Vector3(0, spt_playerControls.leftThumb("Vertical"), 0) * Time.deltaTime * moveSpeed * HaydenIdea);
                if (moveOnLocalZ == true) transform.Translate(new Vector3(0, 0, spt_playerControls.leftThumb("Vertical")) * Time.deltaTime * moveSpeed * HaydenIdea);


                //Clamps the object on the appropriate axes, using the specified min/max's
                if (clampGlobalX == true) newPos.x = Mathf.Clamp(transform.position.x, initialPosition.x - x_maxNegativeDistance, initialPosition.x + x_maxPositiveDistance);
                if (clampGlobalY == true) newPos.y = Mathf.Clamp(transform.position.y, initialPosition.y - y_maxNegativeDistance, initialPosition.y + y_maxPositiveDistance);
                if (clampGlobalZ == true) newPos.z = Mathf.Clamp(transform.position.z, initialPosition.z - z_maxNegativeDistance, initialPosition.z + z_maxPositiveDistance);

                //Sets the objects position according to clamps
                transform.position = newPos;
            }
            //stop moving when button is released
            if (spt_playerControls.aButtonPressed() == false)
            {
                buttonHeld = false;
                once = false;
                if (movingSound != null) aSource.Stop();
                if (optional_movePathImage != null) optional_movePathImage.GetComponent<SpriteRenderer>().enabled = false;

                spt_NetworkPuzzleLogic network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
                //If the drawer is been pulled out, set isMonsterInteractable to true.
                if (transform.position.z > initialPosition.z + 0.2f)
                {
                    for (int i = 0; i < network.PuzzleStates.Count; ++i)
                    {
                        if (network.PuzzleStates[i].itemName == gameObject.name && network.PuzzleStates[i].isMonsterInteractable)
                        {
                            network.updatePuzzleState(network.PuzzleStates[i].name, true, gameObject.name);
                        }
                    }
                }
                else {
                    for (int i = 0; i < network.PuzzleStates.Count; ++i)
                    {
                        if (network.PuzzleStates[i].itemName == gameObject.name && network.PuzzleStates[i].isMonsterInteractable)
                        {
                            network.updatePuzzleState(network.PuzzleStates[i].name, false, gameObject.name);
                        }
                    }
                }
            }
        }

        protected override void holdSuccess()
        {
            buttonHeld = true;

        }

        //Plugging HandleClick
        override protected void HandleClick() { }

        public override void resetItem()
        {
            Debug.Log("Drawer reset called");
            transform.position = initialPosition;
            aSource.clip = slamSound;
            aSource.Play();
            GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState("deskDrawerOpen", false, "mdl_Drawer");
        }
    }
}