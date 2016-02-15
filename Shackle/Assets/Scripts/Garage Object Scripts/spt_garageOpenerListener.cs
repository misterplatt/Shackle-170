using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_garageOpenerListener : NetworkBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //If the correctChannel network state is true, drop the garageOpener
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == true) GetComponent<Rigidbody>().useGravity = true;
    }
}