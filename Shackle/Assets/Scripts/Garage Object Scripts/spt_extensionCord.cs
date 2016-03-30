/*
spt_extensionCord

Author(s): Hayden Platt, Dara Diba

Revision 3

Plugs the cord into the wall when pressed,
and updates the extCordPlugged NPL state to true.
If unplugged by the monster, state is reset.
Added plug and unplug sounds.
*/

using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_extensionCord : spt_baseInteractiveObject
    {
        public static bool local_extCordPlugged = false;

        private bool once = false;
        private Vector3 initalPosition;
        private Quaternion initialRotation;
        public AudioClip plugInsound;
        public AudioClip unplugSound;
        private AudioSource plugSound;


        override protected void Start()
        {
            initalPosition = transform.position;
            initialRotation = transform.rotation;
            plugSound = GetComponent<AudioSource>();
        }

        //If the monster unplugs the extension cord, reset its position and allow it to be plugged in again 
        override protected void Update()
        {
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[3].state == false && once) {
                transform.position = initalPosition;
                transform.rotation = initialRotation;
                spt_remotePower.local_TVpowerState = false;
                spt_WorldState.worldStateChanged = true;
                once = false;
                plugSound.clip = unplugSound;
                plugSound.Play();
            }
        }

        //Handle the Click event
        override protected void clickSuccess()
        {
            if (!once){
                plugSound.clip = plugInsound;
                plugSound.Play();
                //NPL Update
                local_extCordPlugged = true;
                spt_WorldState.worldStateChanged = true;

                transform.Translate(Vector3.up * 1);
                transform.eulerAngles = new Vector3(17, -180, 0); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
                once = true;
            }
        }

        //Plug HandleDown from base
        override protected void HandleDown() {}

        public override void resetItem() {
            local_extCordPlugged = false;
        }
    }
}