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
        private bool pressed = false;

        override protected void clickSuccess()
        {
            Debug.Log("PRESSING" + pressed);
            if (!pressed) {
                transform.Translate(Vector3.down * .2f);
                pressed = true;
                Invoke("raiseLever", 1.5f);
            }
        }

        //Plug handleDown
        override protected void HandleDown() { }

        void raiseLever() {
            transform.Translate(Vector3.up * .2f);
            pressed = false;
        }
    }
}
