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


        //Show associated text while reticle is over the object
        protected override void HandleOver()
        {
            button.image.color = new Color32(153, 0, 0, 255);
        }

        //Hide associated text while reticle leaves the object
        protected override void HandleOut()
        {
            button.image.color = new Color32(229, 229, 229, 255);
        }

        protected override void holdSuccess()
        {
            
        }
    }
}
