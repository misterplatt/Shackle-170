/*
spt_bucketChuck

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 1

Script which allows the monster to interact with the bucket, and knock it off
the shelf. Once complete, it disables the bucket's movable script so that
it can no longer be interacted with.
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
