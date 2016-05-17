/*
spt_bucketChuck

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 2

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
    public class spt_bucketChuck : spt_interactiveMovable
    {
        Rigidbody rb;

        bool networkInitialized = false;
        spt_NetworkPuzzleLogic network;

        //Puzzle state indices of the bucket being on the shelf, and the noise of the bucket hitting the floor
        int bucketFlingIndex = -1;
        int bucketCollisionIndex = -1;

        override protected void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        override protected void Update()
        {
            //DEBUG KEY: Simulates bucket chuck
            if (Input.GetKeyDown(KeyCode.X)) resetItem();

            // Gets all the needed network components once only
            if (!networkInitialized)
            {
                network = GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
                if (network != null)
                {
                    for (int i = 0; i < network.PuzzleStates.Count; i++)
                    {
                        if (network.PuzzleStates[i].name == "isBucketOnShelf")
                            bucketFlingIndex = i;
                        if (network.PuzzleStates[i].name == "playBucketCollisionNoise")
                            bucketCollisionIndex = i;
                    }
                    networkInitialized = true;
                    network.updatePuzzleState("isBucketOnShelf", true, "mdl_bucket");
                }
            }

            //Check to see if audio needs to be played
            else
            {
                if (network.PuzzleStates[bucketCollisionIndex].state == true)
                {
                    //play some audio (make sure it's a Oneshot)
                    Debug.LogWarning("Bucket Throw Sound");
                    network.Cmd_UpdatePuzzleLogic("playBucketCollisionNoise", false, "mdl_bucket");
                }
            }
        }

        public override void resetItem()
        {
            Debug.Log("Reset Called");

            if (networkInitialized)
            {
                network.updatePuzzleState("isBucketOnShelf", false, "mdl_bucket");

                rb.useGravity = true;
                rb.AddForce(new Vector3(950, 0, 200));
                GetComponent<spt_interactiveMovable>().enabled = false;
            }
        }
    }
}
