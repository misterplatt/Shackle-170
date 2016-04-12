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
        bool currentState = false;
        private AudioSource aSource;

        public static bool local_keyFobPressed;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        //Function that activates all manipulation object's children's colliders on pickup, and deactivates on put down
        //Precon: There is a parent object that BroadcastMessage-calls this script
        public void childActive(bool state)
        {
            GetComponent<BoxCollider>().enabled = state;
        }

        //Handle the Click event, alternates states on every press
        override protected void clickSuccess()
        {
            currentState = !currentState;
            //Highlight digit button and send it's number to remoteManager
            if (currentState == true)
            {
                //NPL Update
                local_keyFobPressed = true;
                spt_WorldState.worldStateChanged = true;
                aSource.Play();
                m_Renderer.material = m_StateTwoMaterial;
                GameObject.Find("mdl_jeep").transform.Translate(new Vector3(-4,0,0));
                GameObject.Find("Fuse Diagram").GetComponent<SpriteRenderer>().enabled = true;
            }
            //Un-highlight digit button and remove it's number from remoteManager
            else if (currentState == false)
            {
                m_Renderer.material = m_StateOneMaterial;
            }
        }

        //Plug HandleDown function from base
        protected override void HandleDown() { }
    }
}
