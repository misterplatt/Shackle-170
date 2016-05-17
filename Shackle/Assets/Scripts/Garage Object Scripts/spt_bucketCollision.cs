/*
spt_bucketCollision

Author(s): Lauren Cunningham

Revision 1

Script to detect when the bucket collides with an object after it has been thrown off the shelf by the monster.
*/

using UnityEngine;
using System.Collections;

public class spt_bucketCollision : MonoBehaviour {

    bool networkInitialized = false;
    spt_NetworkPuzzleLogic network;
    int bucketFlingIndex = -1;
    int bucketCollisionIndex = -1;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
    }

    // Called when the bucket collides with an object
    public void OnCollisionEnter(Collision c)
    {
        if (network.PuzzleStates[bucketFlingIndex].state == false)
            network.Cmd_UpdatePuzzleLogic("playBucketCollisionNoise", true, "mdl_bucket");
    }
}
