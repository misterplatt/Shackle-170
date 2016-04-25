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
    public class spt_outletKill : spt_baseInteractiveObject
    {
        public static bool local_playerDeath = false;

        private bool once = false;
        private AudioSource plugSound;


        override protected void Start()
        {
            plugSound = GetComponent<AudioSource>();
        }

        //If the monster unplugs the extension cord, reset its position and allow it to be plugged in again 
        override protected void Update()
        {

        }

        //Handle the Click event
        override protected void clickSuccess()
        {
            if (!once)
            {
                plugSound.Play();

                //NPL Update
                local_playerDeath = true;
                spt_WorldState.worldStateChanged = true;

                once = true;
            }
        }

        //Plug HandleDown from base
        override protected void HandleDown() { }
    }
}