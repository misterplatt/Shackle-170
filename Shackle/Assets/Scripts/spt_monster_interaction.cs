using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spt_monster_interaction : MonoBehaviour {

    private spt_NetworkPuzzleLogic network;

    private GameObject[] interactableObjects;
    private string[] interactableObjectNames;
    private double[] weights;

    private int interactionDowntime = 30;
    private int currentTime = 0;
    private int lastInteractionTime = 0;

	// Use this for initialization
	void Start () {
        network = GameObject.FindObjectOfType(typeof(spt_NetworkPuzzleLogic)) as spt_NetworkPuzzleLogic;
        List<dev_LogicPair>.Enumerator e = network.devtool_PuzzleStates.GetEnumerator();
        int index = 0;
        while (e.MoveNext()){
            interactableObjects[index] = e.Current.item;
            interactableObjectNames[index] = e.Current.eventName;
            weights[index] = 0.5;
            index++;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if ((currentTime - lastInteractionTime) > interactionDowntime){
            for (int i = 0; i < interactableObjects.Length; i++){
                if (Vector3.Distance(interactableObjects[i].transform.position, gameObject.transform.position) < 2){
                    float decision = Random.Range(0, 1);
                    if (decision < weights[i]){
                        interactWithObject(interactableObjectNames[i], interactableObjects[i].name);
                    }
                }
            }
        }
	}

    void interactWithObject(string item, string itemName){
        network.updatePuzzleState(item, false, itemName);
    }

    void updateTime(){
        currentTime = currentTime + 1;
    }
}
