/*
spt_keypad

Author(s): Hayden Platt, Lauren Cunningham, Dara Diba

Revision 2

Simple script which moves the lab door on holdsuccess
with the ID card.
Added audio - Dara
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_keypad : spt_baseInteractiveObject
    {
        public static bool local_doorOpen = false;

        //public float openDuration = 3f;
        public AudioClip doorCloseSound;
        public AudioClip doorOpenSound;
        private bool once = false;
        private bool played = false;
        private AudioSource aSource;


        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
            inventorySpt = GetComponent<VRInteractiveItem>().inventoryScript;
        }

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            if (!once) {
                transform.FindChild("lab_doors").gameObject.GetComponent<Animation>().Play("labDoor_open");
                holding = false;
                once = true;
                //Invoke("CloseDoors", openDuration);

                //NPL Update
                local_doorOpen = true;
                spt_WorldState.worldStateChanged = true;
            }
        }

        protected override void HandleDown()
        {
            base.HandleDown();
            if (!played && inventorySpt.retrieveObjectFromInventory(inventorySpt.activeItem).name.Contains(gateItemName))
            {
                aSource.clip = doorOpenSound;
                aSource.Play();
                played = true;
            }
        }

        protected override void HandleUp()
        {
            base.HandleUp();
            aSource.Stop();
            played = false;
        }

        //Plug HandleClick
        override protected void HandleClick() { }

        //Function which monster closes the doors with
        public override void resetItem()
        {
            aSource.clip = doorCloseSound;
            aSource.Play();
            transform.FindChild("lab_doors/left_door").transform.Translate(Vector3.right * -0.8f);
            transform.FindChild("lab_doors/right_door").transform.Translate(Vector3.left * -1f);
            once = false;

            //NPL Update
            local_doorOpen = false;
            spt_WorldState.worldStateChanged = true;
        }
    }
}