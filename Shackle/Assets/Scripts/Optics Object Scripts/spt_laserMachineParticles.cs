/* spt_laserMachineParticles.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 5/11/2016
 * 
 * This script turns on and off the laser machine's particle system as the machine is turned on and off. **/

using UnityEngine;
using System.Collections;

public class spt_laserMachineParticles : MonoBehaviour {

    private int puzzleStateIndex = -1;
    private spt_NetworkPuzzleLogic network;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (network == null)
        {
            network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
            for (int i = 0; i < network.PuzzleStates.Count; i++)
            {
                if (network.PuzzleStates[i].name == "isLaserOn")
                    puzzleStateIndex = i;
            }
        }
        else
        {
            if (network.PuzzleStates[puzzleStateIndex].state == true)
            {
                gameObject.GetComponent<ParticleSystem>().enableEmission = true;
            }
            else
                gameObject.GetComponent<ParticleSystem>().enableEmission = false;
        }

	}
}
