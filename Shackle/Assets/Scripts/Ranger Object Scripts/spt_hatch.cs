/*
spt_hatch

Author(s): Hayden Platt

Revision 1

Opens the hatch if pickaxe is used on
it for holdTime.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_hatch : spt_baseInteractiveObject
    {
        private AudioSource aSource;
        public static bool local_puzzleCompletion;

        override protected void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            aSource.Play();
            //NPL Update
            local_puzzleCompletion = true;
            spt_WorldState.worldStateChanged = true;

            transform.Translate(new Vector3(0, .38f, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED
            transform.eulerAngles = new Vector3(-30, 0, 0);
        }
    }
}