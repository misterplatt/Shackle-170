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
        //Activates mirror's laser on enter, deactivates on exit
        void OnCollisionEnter(Collision col) {
            Debug.Log("Colliding with mirror: " + col.gameObject.tag);
			Debug.Log("Colliding with mirror: " + col.gameObject.name);
			Debug.Log(col.gameObject.tag == "mirror");
			if (col.gameObject.tag == "laser" || col.gameObject.name == "Mirror Pickup") transform.FindChild("Laser").gameObject.SetActive(true);
        }

        void OnCollisionExit(Collision col){
			if (col.gameObject.tag == "laser" || col.gameObject.name == "Mirror Pickup") transform.FindChild("Laser").gameObject.SetActive(false);
        }
    }
}

