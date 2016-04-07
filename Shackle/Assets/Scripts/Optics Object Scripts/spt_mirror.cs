/*
spt_mirror

Author(s): Hayden Platt, Dara Diba

Revision 1

Inherits basePickup, but contains added functionality
which activates its laser child when a laser collides with it.
*/

using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_mirror : spt_interactivePickUp
    {
        protected override void clickSuccess()
        {
            base.clickSuccess();
            transform.parent = GameObject.Find("Objects").transform;
        }
    }
}

