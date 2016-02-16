/* spt_monsterInteraction.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 2/15/2016
 * 
 * This file is the one that governs all interactions between the monster and items in the environment. **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spt_monsterInteraction : MonoBehaviour {

    // Connection between the monster and the networked puzzle states
    private spt_NetworkPuzzleLogic network;

    // Arrays of the actual interactable objects, their names (in the network), and the weight of each of those
    //  objects (changes how likely the monster is to interact with an object).
    private int[] indecies;
    private double[] weights;

    // Downtime = the amount of time the monster must wait until it can perform another interaction i.e. the time between interactions.
    // Current Time = the amount of time elapsed in this playthrough, in seconds.
    // Last Interaction Time = the elapsed time of the last interaction
    private int interactionDowntime = 30;
    private int currentTime = 0;
    private int lastInteractionTime = 0;

    private bool loadedTheNetwork = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (network == null)
        {
            network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
            if (network != null)
            {
                indecies = new int[network.PuzzleStates.Count];
                weights = new double[network.PuzzleStates.Count];
            }
        }
        else
        {
            //network = GameObject.FindObjectOfType(typeof(spt_NetworkPuzzleLogic)) as spt_NetworkPuzzleLogic;
            //Debug.Log(network.loaded);
            if (loadedTheNetwork == false)
            {
                // Iterates through the puzzle logic communicator, gets all necessary data for item interaction (populates the above arrays)
                int j = 0;
                for (int i = 0; i < network.PuzzleStates.Count; i++)
                {
                    if (network.PuzzleStates[i].isMonsterInteractable)
                    {
                        Debug.Log(network.PuzzleStates[i].itemName);
                        indecies[j] = i;
                        weights[j] = 0.5;
                        j++;
                    }
                }

                InvokeRepeating("updateTime", 1, 1);
                loadedTheNetwork = true;
            }

            else if (loadedTheNetwork == true)
            {
                // If the monster is not in a downtime period...
                if ((currentTime - lastInteractionTime) > interactionDowntime)
                {

                    // Cycle through the possible interactable objects.
                    for (int i = 0; i < indecies.Length; i++)
                    {

                        // If the monster is within interaction range...
                        if (Vector3.Distance(GameObject.Find(network.PuzzleStates[indecies[i]].itemName).transform.position, gameObject.transform.position) < 2)
                        {

                            // Perform interaction some of the time, dependent on a random number.
                            float decision = Random.Range(0, 1);
                            if (decision < weights[i])
                            {
                                interactWithObject(network.PuzzleStates[indecies[i]].name, network.PuzzleStates[indecies[i]].itemName);
                            }
                        }
                    }
                }
            }
        }
	}

    // Function used to make updates to the network puzzle state communicator.
    void interactWithObject(string item, string itemName){
        Debug.Log("interacting with: " + itemName);
        network.updatePuzzleState(item, false, itemName);
    }

    // Function used to update the elapsed playthrough time. Called every second.
    void updateTime(){
        currentTime = currentTime + 1;
    }
}
