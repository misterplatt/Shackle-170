/*
spt_keyDoorChuck

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 1

Script which allows the monster to interact with the fallen key door, and throw
it across the room.
*/

using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_keyDoorChuck : spt_baseInteractiveObject
    {

        Rigidbody rb;

        override protected void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        override protected void Update()
        {
            //DEBUG KEY: Simulates keyDoor chuck
            if (Input.GetKeyDown(KeyCode.X)) resetItem();
        }

        public override void resetItem()
        {
            //If combo lock has unlocked and activated gravity on the keyDoor, chuck it. Otherwise, do nothing on interaction.
            if (rb.useGravity) {
                //Sample puzzle state update
                //GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState("keyDoorChucked", true, "mdl_cabinetDoor");

                rb.useGravity = true;
                rb.AddForce(new Vector3(12000, 4000, 16000));
            }
        }
    }
}
