/*
spt_fuseDoor

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 4

Opens the fusebox door if square key is used on
it for holdTime.
Added door opening sound - Dara
Commented out line 34 and added line 35-36 for new model
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_fuseDoor : spt_baseInteractiveObject
    {
        private AudioSource aSource;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }
        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            aSource.Play();
            //transform.parent.Translate(new Vector3(1.4f, 0, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED  
            transform.Translate(new Vector3(-.2f, 0, 0));
            transform.Rotate(new Vector3(0, -150, 0));
            GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("fuseBoxOpen", true, "grp_fuseBox");
            holding = false;
        }
    }
}
