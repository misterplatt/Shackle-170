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
    public class spt_VRMenuButton: spt_baseInteractiveObject_Single
    {

        private Button button;

        //Find Vignette on start
        protected override void Start()
        {
            button = GetComponent<Button>();
        }


        //Set button color to red while reticle is over the object
        protected override void HandleOver()
        {
            button.image.color = new Color32(151, 0, 0, 255);
        }

        //Return button to grey while reticle leaves the object
        protected override void HandleOut()
        {
            button.image.color = new Color32(55, 55, 55, 255);
        }

        protected override void holdSuccess()
        {
            button.onClick.Invoke(); //Calls whatever is placed in the Button component's OnClick sections
            holding = false;
        }
    }
}
