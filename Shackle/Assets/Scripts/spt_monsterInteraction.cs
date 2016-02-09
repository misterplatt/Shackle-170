//Created by: Lauren Cunningham

/** This file is the one that governs all interactions between the monster and items in the environment. **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spt_monsterInteraction : MonoBehaviour {

    // Connection between the monster and the networked puzzle states
    private spt_NetworkPuzzleLogic network;

    // Arrays of the actual interactable objects, their names (in the network), and the weight of each of those
    //  objects (changes how likely the monster is to interact with an object).
    private GameObject[] interactableObjects;
    private string[] interactableObjectNames;
    private double[] weights;

    // Downtime = the amount of time the monster must wait until it can perform another interaction i.e. the time between interactions.
    // Current Time = the amount of time elapsed in this playthrough, in seconds.
    // Last Interaction Time = the elapsed time of the last interaction
    private int interactionDowntime = 30;
    private int currentTime = 0;
    private int lastInteractionTime = 0;

	// Use this for initialization
	void Start () {
        network = GameObject.FindObjectOfType(typeof(spt_NetworkPuzzleLogic)) as spt_NetworkPuzzleLogic;
        
        // Iterates through the puzzle logic communicator, gets all necessary data for item interaction (populates the above arrays)
        List<dev_LogicPair>.Enumerator e = network.devtool_PuzzleStates.GetEnumerator();
        int index = 0;
        while (e.MoveNext()){
            interactableObjects[index] = e.Current.item;
            interactableObjectNames[index] = e.Current.eventName;
            weights[index] = 0.5;
            index++;
        }

        InvokeRepeating("updateTime", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
        
        // If the monster is not in a downtime period...
        if ((currentTime - lastInteractionTime) > interactionDowntime){
            
            // Cycle through the possible interactable objects.
            for (int i = 0; i < interactableObjects.Length; i++){
                
                // If the monster is within interaction range...
                if (Vector3.Distance(interactableObjects[i].transform.position, gameObject.transform.position) < 2){
                    
                    // Perform interaction some of the time, dependent on a random number.
                    float decision = Random.Range(0, 1);
                    if (decision < weights[i]){
                        interactWithObject(interactableObjectNames[i], interactableObjects[i].name);
                    }
                }
            }
        }
	}

    // Function used to make updates to the network puzzle state communicator.
    void interactWithObject(string item, string itemName){
        network.updatePuzzleState(item, false, itemName);
    }

    // Function used to update the elapsed playthrough time. Called every second.
    void updateTime(){
        currentTime = currentTime + 1;
    }
}
