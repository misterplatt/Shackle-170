/*
spt_interactiveItemCombinationTWO

Author(s): Hayden Platt,Dara Diba

Revision 1

Child of the base interactiveObject class
Allows for an item to manage and check that if the conditions to the speficic combination are met, then the combination state is changed to true.
Seems to be unused so far.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_interactiveItemCombinationTWO : spt_baseInteractiveObject
    {
        [SerializeField]
        private Material m_StateOneMaterial;
        [SerializeField]
        private Material m_StateTwoMaterial;


        //Access to InteractiveItem script
        [SerializeField]
        private Renderer m_Renderer;


        private bool currentState = false;

        override protected void Update()
        {
            //If the object has been clicked in the world, lerp it in front of the player and enable rotation with right thumbstick
            if (currentState == true)
            {
                //transform.position = Vector3.Lerp(transform.position, endPoint.position, Time.deltaTime * lerpSpeed);
                //transform.Rotate(new Vector3(spt_playerControls.rightThumb("Vertical"), spt_playerControls.rightThumb("Horizontal"), 0) * Time.deltaTime * rotationSpeed, Space.World);
            }
            //If B is pressed, return the object to it's default position and rotation
            else if (currentState == false) //&& transform.position != startPoint)
            {
                Debug.Log("SENDBACK");
                //transform.position = Vector3.Lerp(transform.position, startPoint, Time.deltaTime * lerpSpeed);
                //transform.rotation = startRotation;
            }

            //Return object when button b button is pressed
            if (spt_playerControls.bButtonPressed() == true)
            {
                currentState = false;
                m_Renderer.material = m_StateTwoMaterial;
            }
        }

        //Handle the Click event, alternates states on every press
        override protected void clickSuccess()
        {
            currentState = true;
            m_Renderer.material = m_StateOneMaterial;
        }
    }
}