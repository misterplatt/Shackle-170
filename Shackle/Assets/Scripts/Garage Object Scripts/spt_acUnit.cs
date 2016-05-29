/*
spt_acUnit

Author(s): Hayden Platt

Revision 3

Opens the AC unit once the screwdriver is used on all 4 screws
for holdTime.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_acUnit : spt_baseInteractiveObject
    {
        private AudioSource unscrewingAC;
        public static int screwsLeft;

        private AudioSource aSource;

        override protected void Start()
        {
            unscrewingAC = GetComponent<AudioSource>();
            screwsLeft = 4;
            aSource = transform.parent.GetComponent<AudioSource>();
        }

    //Plug HandleClick
    override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            //Deactivate the current screw game object, and decrement the screwsLeft count. Once zero, open the door.
            screwsLeft--;
            gameObject.SetActive(false);
            unscrewingAC.Stop();
            if (screwsLeft <= 0) {
                transform.parent.GetComponent<Animation>().Play("screwDoor_open");
                aSource.Play();
            } 
            holding = false;
        }

        override protected void HandleDown()
        {
            inventorySpt = GetComponent<VRInteractiveItem>().inventoryScript;
            selectionRadial = GetComponent<VRInteractiveItem>().radial;
            // User must press A to interact with the object, negates the case of user holding A previous to interaction
            if (Input.GetButtonDown("aButton") && inventorySpt.retrieveObjectFromInventory(inventorySpt.activeItem).name == gateItemName)//activeItem.Value.name == gateItemName)
            {
                holding = true;
                selectionRadial.enabled = true;
                unscrewingAC.Play();
            }
            if (holding)
            {
                selectionRadial.fillAmount = timer / holdTime;
                timer += Time.deltaTime;

                if (timer >= holdTime || holdTime == 0)
                {
                    selectionRadial.enabled = false;
                    holdSuccess();
                }
            }

            //START HERE FOR RADIAL FADING
            if (!holding)
            {
                unscrewingAC.Stop();
                selectionRadial.fillAmount = 0;
                selectionRadial.enabled = false;
                timer = 0;
            }
        }
    }
}