using UnityEngine;
using System.Collections;

public class spt_keyDoorCollision : MonoBehaviour {

    bool networkInitialized = false;
    spt_NetworkPuzzleLogic network;
    int doorThrownIndex = -1;
    int doorThrownNoiseIndex = -1;
    
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
                    if (network.PuzzleStates[i].name == "keyDoorThrowable")
                        doorThrownIndex = i;
                    if (network.PuzzleStates[i].name == "keyDoorThrowableNoise")
                        doorThrownNoiseIndex = i;
                }
                networkInitialized = true;
            }
        }
    }

    public void OnCollisionEnter(Collision c)
    {
        if (network.PuzzleStates[doorThrownIndex].state == false)
            network.Cmd_UpdatePuzzleLogic("keyDoorThrowableNoise", true, "mdl_cabinetDoor");
    }
}
