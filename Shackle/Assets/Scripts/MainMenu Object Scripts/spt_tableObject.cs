/*
spt_fuseManager

Author(s): Hayden Platt, Dara Diba

Revision 2 

Stores the currentstate of the switches' states
in a bool array. Unlocks electronic lock if all true.
Added sucessful fuse combo sound - Dara
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

        protected override void Start()
        {
            vignette = GameObject.Find("Vignette").GetComponent<Image>();
        }


        protected override void HandleOver()
        {
            textToActivate.enabled = true;
        }

        protected override void HandleOut()
        {
            textToActivate.enabled = false;
            textToActivate.color = Color.white;
        }

        protected override void holdSuccess()
        {
            foreach (Transform child in GameObject.Find("Painting_Canvas").transform) {
                if(child.name != "Vignette") child.gameObject.SetActive(false);
            }
            textToActivate.color = new Color32(153,0,0, 255);
            vignette.enabled = true;
            moduleToActivate.SetActive(true);
        }
    }
}
