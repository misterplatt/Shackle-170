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
    public class spt_VRLobbyButton : spt_baseInteractiveObject
    {

        private Button button;
        private bool selected = false;

        //Find Vignette on start
        protected override void Start()
        {
            button = GetComponent<Button>();
        }


        //Set button color to red while reticle is over the object
        protected override void HandleOver()
        {
            if(!selected) button.image.color = new Color32(151, 0, 0, 255);
        }

        //Return button to grey while reticle leaves the object
        protected override void HandleOut()
        {
            if(!selected) button.image.color = new Color32(55, 55, 55, 255);
        }

        protected override void holdSuccess()
        {
            button.onClick.Invoke(); //Calls whatever is placed in the Button component's OnClick sections
            holding = false;
        }

        //Code to be called in onClick for level select
        public void selectLevel()
        {
            //Set all level buttons to grey (unselected)
            foreach (Transform child in transform.parent)
            {
                if (child.name.Contains("lvl_")) {
                    child.GetComponent<Button>().image.color = new Color32(151, 0, 0, 255);
                    child.GetComponent<spt_VRLobbyButton>().selected = false;
                }
            }
            //Then set the clicked level button to red (selected)
            GetComponent<Button>().image.color = new Color32(0, 0, 0, 255);
            selected = true;
        }

        //Function to be called in onClick when Ready button is pressed
        public void readyUp()
        {
            //Depending on which ready button was clicked, change the ready state "button" color to green
            if (transform.parent.name == "Host_UI") {
                transform.parent.FindChild("P1_readystate").GetComponent<Button>().image.color = new Color32(147, 196, 125, 255);
            }
            else transform.parent.FindChild("P1_readystate").GetComponent<Button>().image.color = new Color32(147, 196, 125, 255);
        }
    }
}
