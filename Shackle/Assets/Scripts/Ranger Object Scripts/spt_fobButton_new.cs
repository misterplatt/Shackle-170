/*
spt_fobButton_new

Author(s): Hayden Platt, Dara Diba, Lauren

Revision 1

Simple script for the button on the key fob.
On press, triggers SUV crash animation
Audio for crash now implemented
Implemented puzzle state change for car crash.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    //Script that handles player interaction with digit buttons on the remote
    public class spt_fobButton_new : spt_baseInteractiveObject
    {
        bool once = false;
        bool once1 = false;
        private AudioSource aSource;

        public static bool local_keyFobPressed = false;
        public static bool local_carCrash = false;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            if (!once1 && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true)
            {
                Debug.Log("GETTING HYPHY");
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
                transform.parent.GetComponent<spt_interactiveItemManipulate>().currentState = false;

                //Start car crash animation as crash sound occurs
                Invoke("carCrash", 9.4f);
                once = true;
            }
        }

        //Plug HandleDown function from base
        protected override void HandleDown() { }

        void carCrash()
        {
            GameObject car = GameObject.Find("carAnimation");
            car.GetComponent<Animator>().enabled = true;
            car.transform.FindChild("polySurface48/Tail Light").GetComponent<Light>().enabled = true;
            GameObject.Find("tex_fuseDiagram").GetComponent<MeshRenderer>().enabled = true;

            //NPL Update
            local_carCrash = true;
            spt_WorldState.worldStateChanged = true;
            //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState("carCrash", true, "mdl_jeep");
        }
    }
}