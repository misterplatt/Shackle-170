/*
spt_extensionCord

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 4

Plugs the cord into the wall when pressed,
and updates the extCordPlugged NPL state to true.
If unplugged by the monster, state is reset.
Added plug and unplug sounds.
*/

using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_bucketChuck : spt_baseInteractiveObject
    {

        Rigidbody rb;

        override protected void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        override protected void Update()
        {
            //DEBUG KEY: Simulates bucket chuck
            if (Input.GetKeyDown(KeyCode.X)) resetItem();
        }

        public override void resetItem()
        {
            //Sample puzzle state update
            //GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState("bucketChucked", true, "mdl_bucket");

            rb.useGravity = true;
            rb.AddForce(new Vector3(950, 0, 200));
            GetComponent<spt_interactiveMovable>().enabled = false;
        }
    }
}
