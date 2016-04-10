/*
spt_fuseManager

Author(s): Hayden Platt

Revision 1

Stores the currentstate of the switches' states
in a bool array. Unlocks electronic lock if all true.
*/

using UnityEngine;
using System.Collections;
using System.Linq;
using System;

namespace VRStandardAssets.Examples
{
    public class spt_fuseManager : spt_baseInteractiveObject
    {

        [SerializeField]
        public static bool[] fuseStates;
        public static bool[] correctStates;

        public static bool local_correctFuseCombo;

        // Use this for initialization
        override protected void Start()
        {
            fuseStates = new bool[6] { false, false, false, false, false, false };
            correctStates = new bool[6] { true, true, false, false, true, true };
        }

        //Function to handle input of channel
        //Precon: Button objects named 1-9 exist in the scene
        //Postcon: channelNumber String array is altered
        public void updateFuseStates(int index, bool state)
        {
            fuseStates[index] = state;
            if (fuseStates.SequenceEqual(correctStates))
            {
                //NPL Update
                local_correctFuseCombo = true;
                spt_WorldState.worldStateChanged = true;
                GameObject.Find("Electronic Lock").transform.Translate(new Vector3(.3f, 0, 0));
                Debug.Log("CORRECT SWITCHES ON!$@##@#$");
            }
        }

        //When the monster interacts with the box, each fuses is reassigned a random boolean value
        public override void resetItem()
        {
            spt_fuseSwitch[] fuses = GetComponentsInChildren<spt_fuseSwitch>();
            foreach (spt_fuseSwitch fuse in fuses) {
                fuse.randomToggle();
            }
        }
    }
}

