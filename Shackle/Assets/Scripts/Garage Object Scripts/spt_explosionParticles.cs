/* spt_explosionParticles.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 5/11/2016
 * 
 * This script the explosion form the TNT levers. **/

using UnityEngine;
using System.Collections;

public class spt_explosionParticles : MonoBehaviour {

    private int playerATNT = -1;
    private int playerBTNT = -1;
    private spt_NetworkPuzzleLogic network;

    private bool once = false;

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
                if (network.PuzzleStates[i].name == "leverAPressed")
                    playerATNT = i;
                if (network.PuzzleStates[i].name == "leverBPressed")
                    playerBTNT = i;
            }
        }
        else
        {
            if ((network.PuzzleStates[playerATNT].state == true) && (network.PuzzleStates[playerBTNT].state == true))
            {
                if (!once)
                {
                    GameObject go = (GameObject)Instantiate(Resources.Load("Explosion"));
                    go.transform.position = gameObject.transform.position;
                    once = true;
                }
            }
        }

    }
}
