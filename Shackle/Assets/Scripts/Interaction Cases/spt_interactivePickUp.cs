/*
spt_interactivePickup

Author(s): Hayden Platt,Dara Diba

Revision 2

Child of the base interactiveObject class
Allows for an item to be picked up and added to the player's inventory
*/


using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_interactivePickUp : spt_baseInteractiveObject
    {
        private AudioSource aSource;
        public AudioClip secondarySound;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        //Handle the Click event
        override protected void clickSuccess()
        {
            Debug.Log("Show click state");
            inventorySpt = GetComponent<VRInteractiveItem>().inventoryScript;

            if (inventorySpt.inventory.Count < 5)
            {
                inventorySpt.pickUp(gameObject);
                transform.position = Vector3.down * 1000; //PLACEHOLDER: Sends objects to hell to prevent inventory breaking
            }

            if (secondarySound != null)
            {
                aSource.clip = secondarySound;
                aSource.Play();
            }
            else aSource.Play();
            //gameObject.SetActive(false); //Desired functionality
        }

        //Ignore handle down
        override protected void HandleDown(){}
    }
}
