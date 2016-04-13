/*
spt_fobButton

Author(s): Hayden Platt, Dara Diba

Revision 2

Simple script for the button on the key fob.
On press, triggers SUV crash animation
Audio for crash now implemented
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    //Script that handles player interaction with digit buttons on the remote
    public class spt_fobButton : spt_baseInteractiveObject
    {
        [SerializeField]
        private Material m_StateOneMaterial;
        [SerializeField]
        private Material m_StateTwoMaterial;

        [SerializeField]
        private Renderer m_Renderer;
        bool once = false;
        bool once1 = false;
        private AudioSource aSource;

        public static bool local_keyFobPressed;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            if (!once1 && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true) {
                //Start car crash animation
                aSource.Play();
                once1 = true;
            }
        }

        //Function that activates all manipulation object's children's colliders on pickup, and deactivates on put down
        //Precon: There is a parent object that BroadcastMessage-calls this script
        public void childActive(bool state)
        {
            GetComponent<BoxCollider>().enabled = state;
        }

        //Plugging so that manipulate won't complain
        public void deactivateDigit() { }

        //Handle the Click event, alternates states on every press
        override protected void clickSuccess()
        {
            if (!once)
            {
                //NPL Update
                local_keyFobPressed = true;
                spt_WorldState.worldStateChanged = true;

                //Return fob after pressing
                //GetComponent<spt_interactiveItemManipulate>().currentState = false;

                //Start car crash animation as crash sound occurs
                Invoke("carCrash", 9.4f);
                once = true;
            }
        }

        //Plug HandleDown function from base
        protected override void HandleDown() { }

        void carCrash() {
            GameObject.Find("mdl_jeep").transform.Translate(new Vector3(0, 0, 5.5f));
            GameObject.Find("Destructible_A_Wall").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("Destructible_Side_Wall").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("Fuse Diagram").GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
