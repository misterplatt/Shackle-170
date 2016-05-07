/*
spt_barrel

Author(s): Hayden Platt, Lauern Cunningham

Revision 2

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
        public Texture emptyTube;

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            //Initially require the flammable liquid to interact,
            //changing the gateItem to the matchbox afterward
            if (!once)
            {
                gateItemName = "mdl_matchbox";
                once = true;
                GameObject.Find("mdl_beaker").GetComponent<GUITexture>().texture = emptyTube;
            }
            //If the liquid has been used and the matchbox is being used,
            //initialize the fire particles and destroy poster after x seconds.
            else {
                transform.FindChild("Fire").gameObject.SetActive(true);
                inventorySpt.removeItm("mdl_matchbox");
                Invoke("DestroyPoster", 1.5f);
                GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("barrelExplosion", true, "mdl_barrel");
            }
            holding = false;
        }

        //Plug HandleClick
        override protected void HandleClick() { }

        //Brief function to be invoked on matchbox interaction
        void DestroyPoster() {
            Destroy(GameObject.Find("Poster"));
        }
    }
}
