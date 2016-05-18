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
        public AudioClip doorOpen;
        public AudioClip doorSlam;

        public static bool local_fuseBoxOpen = false;
        public static bool local_fuseDoorSlam = false;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        override protected void Update()
        {
            //DEBUG KEY: Simulates keyDoor slam
            if (Input.GetKeyDown(KeyCode.X)) resetItem();
        }


        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            aSource.clip = doorOpen;
            aSource.Play();
            //transform.parent.Translate(new Vector3(1.4f, 0, 0)); //PLACEHOLDER FUNCTIONALITY UNTIL MODEL IS IMPORTED  
            transform.Translate(new Vector3(-.2f, 0, 0));
            transform.Rotate(new Vector3(0, -150, 0));

            //NPL Update
            local_fuseBoxOpen = false;
            local_fuseDoorSlam = false;
            spt_WorldState.worldStateChanged = true;
            //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("fuseBoxOpen", true, "mdl_fuseBox_base");
            //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("fuseDoorSlam", true, "mdl_fuseBox_door");
            holding = false;
        }

        public override void resetItem()
        {
            //We shouldn't need to modify any puzzle states here, as the fusebox door swings back out to open after being slammed
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[7].state == true) {
                aSource.PlayOneShot(doorSlam);
                GetComponent<Animation>().Play("fuseDoor_slam");
                //PLAY SLAM AUDIO HERE
            }
        }
    }
}
