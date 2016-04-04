/*
spt_laserSwitch

Author(s): Hayden Platt, Dara Diba

Revision 1

Child of the base interactiveObject class
Allows for an item to be switched from a false state to a true state and vice-versa.
*/


using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_laserSwitch : spt_baseInteractiveObject
    {
        public float laserLength = 50;

        [SerializeField] LineRenderer laser;
        [SerializeField] LayerMask laserLayers;
        private bool currentState = false;
        private Vector3 projectionStart;

        override protected void Start() {
            projectionStart = GameObject.Find("Projection Point").transform.position;
        }

        override protected void clickSuccess()
        {
            currentState = !currentState;
            if (currentState == true)
            {
                laser.enabled = true; //Make sure the laser is active
                //Declare an infinite ray shooting in the direction from Projection point
                Ray ray = new Ray(projectionStart, Vector3.forward);
                //Set the line's starting point to the Projection Point's position
                laser.SetPosition(0, projectionStart);
                RaycastHit hit;

                //Raycast in the ray's direction, storing data in hit. If there is a collision
                //set line endpoint to that position.
                if (Physics.Raycast(ray.origin, ray.direction, out hit, laserLength, laserLayers))
                {
                    Debug.Log("LASER COLLISION: " + hit.collider.gameObject.name);
                    laser.SetPosition(1, hit.point);
                }
                else
                {
                    Debug.Log("reached the end!");
                    laser.SetPosition(1, ray.GetPoint(laserLength));
                }
            } //transform.FindChild("Laser_Proj").gameObject.SetActive(true);
            else if (currentState == false) laser.enabled = false;
        }

        //Plug handleDown
        override protected void HandleDown() { }
    }
}