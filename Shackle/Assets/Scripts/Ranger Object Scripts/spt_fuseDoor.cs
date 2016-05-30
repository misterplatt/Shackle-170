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
        private bool didItReset = false;

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

            if (didItReset)
            {
                aSource.PlayOneShot(doorSlam);
                didItReset = false;
                GetComponent<Animation>().Play("fuseDoor_slam");
            }
        }


        //Plug HandleClick
        override protected void HandleClick() { }

        //Handle the Down event, modified so that the reticle doesn't need to stay over object to interact
        override protected void holdSuccess()
        {
            aSource.clip = doorOpen;
            aSource.Play();
            GetComponent<Animation>().Play("fuseDoor_open");

            //Set the car keyfob up to be a manipulate object
            //spt_interactiveItemManipulate keyFobManipulate = transform.FindChild("mdl_carKeyfob").gameObject.GetComponent<spt_interactiveItemManipulate>();
            //keyFobManipulate.startPoint = transform.FindChild("mdl_carKeyfob").position;
            //keyFobManipulate.startRotation = transform.FindChild("mdl_carKeyfob").rotation;
            //keyFobManipulate.anchored = true;

            //NPL Update
            local_fuseBoxOpen = true;
            local_fuseDoorSlam = true;
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
                didItReset = true;
                GetComponent<Animation>().Play("fuseDoor_slam");
            }
        }
    }
}
