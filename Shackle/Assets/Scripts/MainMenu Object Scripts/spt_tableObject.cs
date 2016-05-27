/*
spt_tableObject

Author(s): Hayden Platt

Revision 1

Script attched to objects on the table of the menu scene.
Hovering over an object displays help text, and holdSuccess changes
the Painting canvas' current module.
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

namespace VRStandardAssets.Examples
{
    public class spt_tableObject : spt_baseInteractiveObject_Single
    {

        public Text textToActivate;
        public GameObject moduleToActivate;

        private Image vignette;

        //Find Vignette on start
        protected override void Start()
        {
            vignette = GameObject.Find("Vignette").GetComponent<Image>();
        }


        //Show associated text while reticle is over the object
        protected override void HandleOver()
        {
            textToActivate.enabled = true;
        }

        //Hide associated text while reticle leaves the object
        protected override void HandleOut()
        {
            textToActivate.enabled = false;
            textToActivate.color = Color.white;
        }

        protected override void holdSuccess()
        {
            //If they held A on the garbage can, quit the game
            if (gameObject.name == "mdl_garbageCan") Application.Quit();

            foreach (Transform child in GameObject.Find("Painting_Canvas").transform) {
                if(child.name != "Vignette") child.gameObject.SetActive(false);
            }
            textToActivate.color = new Color32(153,0,0, 255);
            vignette.enabled = true;
            moduleToActivate.SetActive(true);
            GameObject.Find("NetworkManager").GetComponent<spt_ManagerMenuInterface>().showIp();
        }
    }
}
