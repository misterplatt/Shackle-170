/*
spt_VRMenuButton

Author(s): Hayden Platt

Revision 1

Base class for buttons in the VRMainMenu. Handles hover, holdsuccess, etc.
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

namespace VRStandardAssets.Examples
{
    public class spt_VRMenuInput : spt_baseInteractiveObject_Single
    {

        private InputField input;
        public bool interactable;
        //Find Vignette on start
        protected override void Start()
        {
            interactable = true;
            input = GetComponent<InputField>();
        }


        //Set button color to red while reticle is over the object
        protected override void HandleOver()
        {
            input.image.color = new Color32(151, 0, 0, 255);
        }

        //Return button to grey while reticle leaves the object
        protected override void HandleOut()
        {
            input.image.color = new Color32(55, 55, 55, 255);
        }

        protected override void holdSuccess()
        {
            if (!interactable) return;
            input.Select();
            
            holding = false;
        }
    }
}
