/*
spt_barrel

Author(s): Hayden Platt, Lauern Cunningham, Dara Diba

Revision 3

First, requires flammable liquid, after which it requires the matchbox.
Once the matchbox is used, the child fire particle system is activated,
and the poster is destroyed shortly thereafter.
Added sounds for pouring flammable liquid, lighting match, and fire. - Dara
Added a puzzle state for the empty beaker sprite so it will stay consistent across network. - Dara
*/

using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.Networking;

namespace VRStandardAssets.Examples
{
    public class spt_barrel : spt_baseInteractiveObject
    {
        private bool once = false;
        //public Texture emptyTube;
        private AudioSource aSource;
        public AudioClip matchStrike;
        public AudioClip beakerPour;
        public AudioClip posterFire;
        public static bool local_beakerPoured = false;
        private bool matchLit = false;

        /*
        protected override void Update()
        {
            if (GameObject.Find( GameObject.Find("WorldState").GetComponent<spt_WorldState>().localPlayer).GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[14].state == true) GameObject.Find("mdl_beaker").GetComponent<GUITexture>().texture = emptyTube;
        }
        */

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.M)) holdSuccess();
        }

        //Open the garage if opener is used on door for holdTime seconds
        override protected void holdSuccess()
        {
            //Initially require the flammable liquid to interact,
            //changing the gateItem to the matchbox afterward
            if (!once)
            {
                local_beakerPoured = true;
                spt_WorldState.worldStateChanged = true;
                aSource.clip = beakerPour;
                aSource.Play();
                inventorySpt.removeItm("mdl_beaker");
                transform.FindChild("spt_sludge").gameObject.GetComponent<SpriteRenderer>().enabled = true; //Enable sludge
                gateItemName = "mdl_matchbox";
                once = true;
                //GameObject.Find("mdl_beaker").GetComponent<GUITexture>().texture = emptyTube;
            }
            //If the liquid has been used and the matchbox is being used,
            //initialize the fire particles and destroy poster after x seconds.
            else
            {
                aSource.clip = matchStrike;
                aSource.Play();
                transform.FindChild("Fire").gameObject.SetActive(true);
                inventorySpt.removeItm("mdl_matchbox");
                Invoke("FireSound", 3f);

                Invoke("DestroyPoster", 2f);
                // Replace above line with the burn function on the new poster model

                GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("barrelExplosion", true, "mdl_barrel");

                //Little explosion particle upon lighting barrel
                GameObject go = (GameObject)Instantiate(Resources.Load("Explosion - Copy"), new Vector3(2.734f, 1.511f, 5.369f), Quaternion.Euler(0, 0, 0));
                transform.FindChild("spt_sludge").gameObject.GetComponent<SpriteRenderer>().enabled = true; //Remove sludge
                once = true;
            }

            holding = false;
        }

        //Plug HandleClick
        override protected void HandleClick() { }

        //Brief function to be invoked on matchbox interaction
        void DestroyPoster()
        {
            GameObject.Find("mdl_poster").GetComponent<PosterScript>().Burn();
            transform.FindChild("Fire").gameObject.SetActive(false);
        }

        //Brief function to be invoked for the fire sound
        void FireSound()
        {
            aSource.clip = posterFire;
            aSource.Play();
        }
    }
}
