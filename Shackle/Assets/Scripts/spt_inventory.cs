using UnityEngine;
using System.Collections.Generic;

public class spt_inventory : MonoBehaviour {

    public static LinkedList<GameObject> inventory;

    public LinkedListNode<GameObject> activeItem;

    public GameObject object1;

    // Use this for initialization
    void Start () {
        activeItem = new LinkedListNode<GameObject>(object1);
        inventory = new LinkedList<GameObject>();
        inventory.AddLast(activeItem);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(activeItem.Value);
        }
        if (Input.GetKeyDown(KeyCode.R)){
            pickUp(GameObject.Find("Remote"));
            pickUp(GameObject.Find("Black Tar Heroin"));
        }
        if (Input.GetKeyDown(KeyCode.D)){
            cycleRight();
        }
        if (Input.GetKeyDown(KeyCode.A)){
            cycleLeft();
        }
    }

    public void pickUp(GameObject item) {
        inventory.AddLast(item);
    }

    void cycleRight() {
        if (inventory.Count == 0) return;
        if (activeItem.Next != null) {
           Debug.Log("Moving right");
           activeItem = activeItem.Next;
        } else {
            Debug.Log("Looping");
            activeItem = inventory.First;
        }
    }

    void cycleLeft(){
        if (inventory.Count == 0) return;
        if (activeItem.Previous != null){
            Debug.Log("Moving left");
            activeItem = activeItem.Previous;
        } else {
            Debug.Log("Looping");
            activeItem = inventory.Last;
        }
    }
}
