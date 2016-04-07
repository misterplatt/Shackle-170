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
        public static bool local_laserHitLock = false;

        public float laserLength = 50;

        [SerializeField] LineRenderer laser;
        [SerializeField] LayerMask laserLayers;
        private bool currentState = false;
        private Vector3 projectionStart;

        private bool once = false;

        override protected void Start() {
            projectionStart = transform.FindChild("Projection Point").position;
            laser.useWorldSpace = true;
        }

        protected override void Update()
        {
            //If the laser is on and it's been off or a mirror is being rotated, recalculate laser trajectory.
            if (laser.enabled && (spt_mirrorHandle.rotating || spt_mirrorHandle.mirrorRemoved || !once)) {
                Debug.Log("LASERING SHIT");
                //spt_mirrorHandle.mirrorRemoved = false;
                //Declare an infinite ray shooting in the direction from Projection point
                Ray ray = new Ray(projectionStart, transform.forward);
                //Set the line's starting point to the Projection Point's position
                laser.SetPosition(0, projectionStart);
                RaycastHit hit;

                //Raycast in the ray's direction, storing data in hit. If there is a collision
                //set line endpoint to that position.

                //Starting line origin, direction, and vertexCount
                Vector3 projectionOrigin = projectionStart;
                Vector3 projectionDirection = transform.forward;
                int maxVertexCount = 2;
                laser.SetVertexCount(maxVertexCount); //Ensures vertex count is recalculated properly

                //While you haven't hit the maxVertexCount, cast a refelecting line
                for (int i = 1; i < maxVertexCount; i++)
                {
                    if (Physics.Raycast(projectionStart, projectionDirection, out hit, laserLength, laserLayers))
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        //If the reflecting line hits a mirror, up the vertex count so you can reflect again
                        if (hit.collider.gameObject.name == "Mirror Pickup")
                        {
                            maxVertexCount++;
                            laser.SetVertexCount(maxVertexCount);
                        }

                        if (hit.collider.gameObject.name == "Chest Lock") local_laserHitLock = true;

                        //Set latest line point, 
                        Debug.Log("Reflecting");
                        laser.SetPosition(i, hit.point);
                        //Calculate next raycast origin and direction
                        projectionDirection = Vector3.Reflect(projectionDirection, hit.normal);
                        projectionOrigin = hit.point;
                    }
                }
                once = true;
            }
        }

        override protected void clickSuccess()
        {
            currentState = !currentState;

            //Change laser LineRenderer's enabled status on switch click
            if (currentState == true)
            {
                laser.enabled = true;
            }
            else if (currentState == false)
            {
                
                laser.enabled = false;
                once = false;
            }
        }

        //Plug handleDown
        override protected void HandleDown() { }
    }
}