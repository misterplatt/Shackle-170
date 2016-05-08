/*
spt_mirrorHandle

Author(s): Hayden Platt, Dara Diba

Revision 1

Special movable for the mirror stands.
When hand is held and left stick is used horizontally,
mirror is rotated within certain bounds.
*/


using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_mirrorHandle : spt_baseInteractiveObject
    {
        public static bool rotating = false;

        private bool buttonHeld = false;
        private bool moved = false;
        private Vector3 initalRotation;

        //Sound variables
        public AudioClip movingSound;
        private AudioSource aSource;
        private bool once = false;

        //Speed at which the object should move
        public float rotateSpeed = 1;

        public float maxNegativeRotation = 0;
        public float maxPositiveRotation = 0;

        public GameObject optional_movePathImage;

        override protected void Start()
        {
            initalRotation = transform.rotation.eulerAngles;
            aSource = GetComponent<AudioSource>();
            if (movingSound != null) aSource.clip = movingSound;
        }

        override protected void Update()
        {
            //When A is held, use left thumbstick to move object based on object's axis boolean
			if (buttonHeld == true)
            {
                //Displays a movable's movePath sprite if specified
                if (optional_movePathImage != null && !moved) optional_movePathImage.GetComponent<SpriteRenderer>().enabled = true;

                Vector3 newRotation = transform.rotation.eulerAngles; //Vector which handles and clamps

                if (movingSound != null)
                {
                    if (spt_playerControls.leftThumb("Horizontal") != 0)
                    {
                        if (!once)
                        {
                            aSource.Play();
                            once = true;
                        }
                    }
                }
                //Rotates stand before clamp
                transform.Rotate(Vector3.up * -spt_playerControls.leftThumb("Horizontal") * Time.deltaTime * rotateSpeed);
                rotating = true;

                //Clamps the stands rotation on the Y axis, using the specified min/max's
                //newRotation.y = Mathf.Clamp(transform.rotation.eulerAngles.y, initalRotation.y - maxNegativeRotation, initalRotation.y + maxPositiveRotation);
				newRotation.y = ClampAngle(transform.rotation.eulerAngles.y, initalRotation.y - maxNegativeRotation, initalRotation.y + maxPositiveRotation);

                //Sets the objects position according to clamps
				transform.eulerAngles = newRotation;
            }
            //stop moving when button is released
            if (spt_playerControls.aButtonPressed() == false)
            {
                buttonHeld = false;
                once = false;
                rotating = false;
                if (movingSound != null) aSource.Stop();
                if (optional_movePathImage != null) optional_movePathImage.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        protected override void holdSuccess(){
            buttonHeld = true;
        }

        //Plugging HandleClick
        override protected void HandleClick() { }

        //Function which allows us to limit rotation in the negative direction
		static float ClampAngle (float angle, float min, float max){
			if (angle < 90 || angle > 270) {
				if (angle > 180) angle -= 360;
				if (max > 180) max -= 360;
				if (min > 180) min -= 360;
			}
			angle = Mathf.Clamp (angle, min, max);
			if (angle < 0) angle += 360;
			return angle;
		}
    }
}