/* spt_carCrashTrigger.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 4/19/2016
 * 
 * This script is a specialized version of an spt_angerPuzzleStateTrigger.
 * When the ranger outpost's car crash occurs, it pushes the monster to its lower anger threshold.
 **/

using UnityEngine;
using System.Collections;

public class spt_carCrashTrigger : MonoBehaviour {

    private spt_NetworkPuzzleLogic network;
    private spt_monsterMotivation monster;
    private bool indexInitialized = false;
    private int i;
    private bool triggered = false;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (network == null || monster == null)
        {
            network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
            monster = GameObject.FindObjectOfType<spt_monsterMotivation>();
        }
        else
        {
            if (network.loaded && network.loaded == true)
            {
                if (!indexInitialized)
                {
                    for (int index = 0; index < network.PuzzleStates.Count; ++index)
                    {
                        if (network.PuzzleStates[index].itemName == gameObject.name)
                        {
                            i = index;
                            indexInitialized = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (network.PuzzleStates[i].state == true && triggered == false)
                    {
                        triggered = true;
                        monster.updateAnger(monster.lowerThreshold - monster.angerLevel, gameObject.transform);
                    }
                }
            }
        }
	}
}