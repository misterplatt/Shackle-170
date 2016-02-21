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
    public class spt_interactiveMovableTWO : spt_baseInteractiveObject
    {
        private bool buttonHeld = false;
        private Vector3 initialPosition;

        public bool xAxis = true;
        public bool zAxis = false;
        public float moveSpeed = 1;
        public float maxNegativeDistance = 0;
        public float maxPositiveDistance = 2;

        override protected void Start()
        {
            initialPosition = transform.position;
        }

        override protected void Update()
        {
            //When A is held, use left thumbstick to move object based on object's axis boolean
            if (buttonHeld == true)
            {
                Debug.Log("CLICKIN DA BUCKT");
                if (transform.Find("movePath") != null) transform.Find("movePath").gameObject.GetComponent<SpriteRenderer>().enabled = true;
                if (xAxis == true){
                    //Translate along local X axis
                    transform.Translate(new Vector3(spt_playerControls.leftThumb("Horizontal"), 0, 0) * Time.deltaTime * moveSpeed);
                    //Clamps the object's X position to stay between the starting value +maxPosDistance or -maxMinDistance
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, initialPosition.x - maxNegativeDistance, initialPosition.x + maxPositiveDistance), transform.position.y, transform.position.z); //LIMITER: NOT WORKING
                }
                else if (zAxis == true){
                    //Translate along local Z axis
                    transform.Translate(new Vector3(0, 0, spt_playerControls.leftThumb("Vertical")) * Time.deltaTime * moveSpeed);
                    //Clamps the object's z position to stay between the starting value +maxPosDistance or -maxMinDistance
                    transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, initialPosition.z - maxNegativeDistance, initialPosition.z + maxPositiveDistance)); //LIMITER: NOT WORKING
                }
            }
            //stop moving when button is released
            if (spt_playerControls.aButtonPressed() == false) buttonHeld = false;
            if (transform.Find("movePath") != null) transform.Find("movePath").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        protected override void holdSuccess()
        {
            buttonHeld = true;
        }

        //Plugging HandleClick
        override protected void HandleClick() { }
    }
}