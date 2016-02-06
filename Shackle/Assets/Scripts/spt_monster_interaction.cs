using UnityEngine;
using System.Collections;

public class spt_monster_interaction : MonoBehaviour {

    private spt_NetworkPuzzleLogic network;

    public Transform[] interactableObjects;
    private double[] weights;

    private int interactionDowntime = 30;
    private int currentTime = 0;
    private int lastInteractionTime = 0;

	// Use this for initialization
	void Start () {
        network = GameObject.FindObjectOfType(typeof(spt_NetworkPuzzleLogic)) as spt_NetworkPuzzleLogic;
        InvokeRepeating("updateTime", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
        if ((currentTime - lastInteractionTime) > interactionDowntime){
            for (int i = 0; i < interactableObjects.Length; i++){
                if (Vector3.Distance(interactableObjects[i].position, gameObject.transform.position) < 2){

                }
            }
        }
	}

    void interactWithObject(string evtName, string item){
        network.updatePuzzleState(evtName, false, item);
    }

    void updateTime(){
        currentTime = currentTime + 1;
    }
}
