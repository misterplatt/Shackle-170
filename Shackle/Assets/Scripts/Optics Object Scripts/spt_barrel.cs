/*
spt_barrel

Author(s): Hayden Platt

Revision 1

First, requires flammable liquid, after which it requires the matchbox.
Once the matchbox is used, the child fire particle system is activated,
and the poster is destroyed shortly thereafter.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_barrel : spt_baseInteractiveObject
    {
        private bool once = false;

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            //Initially require the flammable liquid to interact,
            //changing the gateItem to the matchbox afterward
            if (!once)
            {
                gateItemName = "Matchbox";
                once = true;
                holding = false;
            }
            //If the liquid has been used and the matchbox is being used,
            //initialize the fire particles and destroy poster after x seconds.
            else {
                transform.FindChild("Fire").gameObject.SetActive(true);
                Invoke("DestroyPoster", 1.5f);
            }

        }

        //Plug HandleClick
        override protected void HandleClick() { }

        //Brief function to be invoked on matchbox interaction
        void DestroyPoster() {
            Destroy(GameObject.Find("Poster"));
        }
    }
}
