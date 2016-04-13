/*
spt_forestMap

Author(s): Hayden Platt

Revision 3

Changes to complete forest map sprite once the
mapCorner object is used on it.
*/

using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_forestMap : spt_baseInteractiveObject
    {
        public static bool local_extCordPlugged = false;

        private bool once = false;
        public AudioClip plugInsound;
        private AudioSource plugSound;

        public Sprite completeMap;

        override protected void Start()
        {
            plugSound = GetComponent<AudioSource>();
        }

        //Handle the Click event
        override protected void holdSuccess()
        {
            if (!once)
            {
                //Play sound when attaching map corner
                plugSound.clip = plugInsound;
                plugSound.Play();

                //Remove mapCorner from inventory
                inventorySpt.removeItm("mdl_mapCorner");

                //Change the map's sprite to the complete version
                GetComponent<SpriteRenderer>().sprite = completeMap;
                once = true;
            }
        }

        //Plug HandleClick from base
        override protected void HandleClick() { }
    }
}
