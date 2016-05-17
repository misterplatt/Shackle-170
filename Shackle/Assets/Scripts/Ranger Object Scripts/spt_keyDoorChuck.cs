/*
spt_keyDoorChuck

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 2

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

        bool networkInitialized = false;
        spt_NetworkPuzzleLogic network;

        //Puzzle state indices dictating ehther the door has been thrown, and whether it needs to make noise or not.
        int doorThrownIndex = -1;
        int doorThrownNoiseIndex = -1;

        override protected void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        override protected void Update()
        {
            //DEBUG KEY: Simulates keyDoor chuck
            if (Input.GetKeyDown(KeyCode.X)) resetItem();

            // Gets all the needed network components once only
            if (!networkInitialized)
            {
                network = GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
                if (network != null)
                {
                    for (int i = 0; i < network.PuzzleStates.Count; i++)
                    {
                        if (network.PuzzleStates[i].name == "keyDoorThrowable")
                            doorThrownIndex = i;
                        if (network.PuzzleStates[i].name == "keyDoorThrowableNoise")
                            doorThrownNoiseIndex = i;
                    }
                    networkInitialized = true;
                }
            }

            //Check to see if audio needs to be played
            else
            {
                if (network.PuzzleStates[doorThrownNoiseIndex].state == true)
                {
                    //play some audio (make sure it's a Oneshot)
                    Debug.LogWarning("Door Throw Sound");
                    network.Cmd_UpdatePuzzleLogic("keyDoorThrowableNoise", false, "mdl_cabinetDoor");
                }
            }
        }

        public override void resetItem()
        {
            //If combo lock has unlocked and activated gravity on the keyDoor, chuck it. Otherwise, do nothing on interaction.
            if (rb.useGravity && networkInitialized) {

                network.Cmd_UpdatePuzzleLogic("keyDoorThrowable", false, "mdl_cabinetDoor");

                rb.useGravity = true;
                rb.AddForce(new Vector3(12000, 4000, 16000));
            }
        }
    }
}
