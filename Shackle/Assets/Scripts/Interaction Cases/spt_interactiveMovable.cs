/*
spt_interactiveItemMovableTWO

Author(s): Hayden Platt,Dara Diba

Revision 1

Child of the base interactiveObject class
Allows for an item to be moved a certain distance in the X or Z axis
*/


using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_interactiveMovable : spt_baseInteractiveObject
    {
        private bool buttonHeld = false;
        private bool moved = false;
        private Vector3 initialPosition;
        public AudioClip movingSound;
        private AudioSource aSource;
        private bool once = false;


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

            //bucketSliding = GetComponent<AudioSource>();
        }

        override protected void Update()
        {
            //When A is held, use left thumbstick to move object based on object's axis boolean
            if (buttonHeld == true)
            {
                // NOT WORKING Correctly yet, will fix at my final tonight 3/8(after 8 pm)
                if (movingSound != null) aSource.Play();
                //Garage only, stops showing an object's movepath once it has been moved
                if (transform.position.z > 4.3f || transform.position.x < 2.2f) moved = true;
           
                    //Displays a movable's movePath sprite if specified
                    if (optional_movePathImage != null && !moved) optional_movePathImage.GetComponent<SpriteRenderer>().enabled = true;

                Vector3 newPos = transform.position; //Vector which handles and clamps

                //Moves object on appropriate axes
                if (moveOnLocalX == true) transform.Translate(new Vector3(spt_playerControls.leftThumb("Horizontal"), 0, 0) * Time.deltaTime * moveSpeed);
                if (moveOnLocalY == true) transform.Translate(new Vector3(0, spt_playerControls.leftThumb("Vertical"), 0) * Time.deltaTime * moveSpeed);
                if (moveOnLocalZ == true) transform.Translate(new Vector3(0, 0, spt_playerControls.leftThumb("Vertical")) * Time.deltaTime * moveSpeed);

                //Clamps the object on the appropriate axes, using the specified min/max's
                if (clampGlobalX == true) newPos.x = Mathf.Clamp(transform.position.x, initialPosition.x - x_maxNegativeDistance, initialPosition.x + x_maxPositiveDistance);
                if (clampGlobalY == true) newPos.y = Mathf.Clamp(transform.position.y, initialPosition.y - y_maxNegativeDistance, initialPosition.y + y_maxPositiveDistance);
                if (clampGlobalZ == true) newPos.z = Mathf.Clamp(transform.position.z, initialPosition.z - z_maxNegativeDistance, initialPosition.z + z_maxPositiveDistance);

                //Sets the objects position according to clamps
                transform.position = newPos;
            }
            //stop moving when button is released
            if (spt_playerControls.aButtonPressed() == false) {
                buttonHeld = false;
                if (movingSound !=null) aSource.Stop();
                if (optional_movePathImage != null) optional_movePathImage.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        protected override void holdSuccess()
        {
            buttonHeld = true;

        }

        //Plugging HandleClick
        override protected void HandleClick() { }
    }
}