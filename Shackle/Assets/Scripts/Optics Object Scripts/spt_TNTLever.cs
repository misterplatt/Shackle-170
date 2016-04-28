/*
spt_TNTLever

Author(s): Hayden Platt

Revision 1

When the lever is pressed, temporarily lowers the lever. If both levers are pressed
within a time frame, the level is beaten and the ending cutscene triggers.
*/


using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_TNTLever : spt_baseInteractiveObject
    {
        public static bool local_leverAPressed = false;
        public static bool local_leverBPressed = false;

        public static bool local_puzzleCompletion = false;

        private bool pressed = false;

        protected override void Update()
        {
            base.Update();
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[10].state == true && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[11].state == true) {
                local_puzzleCompletion = true;
                spt_WorldState.worldStateChanged = true;
            }
        }

        override protected void clickSuccess()
        {
            Debug.Log("PRESSING" + pressed);
            if (!pressed) {
                transform.Translate(Vector3.down * .2f);
                if (transform.parent.gameObject.name == "TNT_Switch A") local_leverAPressed = true;
                if (transform.parent.gameObject.name == "TNT_Switch B") local_leverBPressed = true;
                spt_WorldState.worldStateChanged = true;
                pressed = true;
                Invoke("raiseLever", 1.5f);
            }
        }

        //Plug handleDown
        override protected void HandleDown() { }

        //Function which raises the lever 
        void raiseLever() {
            transform.Translate(Vector3.up * .2f);
            if (transform.parent.gameObject.name == "TNT_Switch A") local_leverAPressed = false;
            if (transform.parent.gameObject.name == "TNT_Switch B") local_leverBPressed = false;
            spt_WorldState.worldStateChanged = true;
            pressed = false;
        }
    }
}
