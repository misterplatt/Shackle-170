//Created by: Lauren Cunningham

/** This file is the one that handle the dynamic difficulty adjustment. **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spt_DDA : MonoBehaviour {

    private spt_monsterMotivation motivationScript;
    private spt_NetworkPuzzleLogic networkScript;
    
    // List of the level's puzzle states with their associated checkpoint times
    private List<puzzleStateWithCheckpointTime> checkpoints;
    
    // Elapsed time of the current playthrough (in seconds)
    private int elapsedTime = 0;

    // Index in the checkpoint/puzzle state lists of the current task to be completed
    private int currentPuzzleStateIndex = 0;

    private bool loadedTheNetwork = false;

	// Use this for initialization
	void Start () {
        
        // Gets the motivation script (for altering the threshold) and the network script (for getting puzzle states)
        motivationScript = GameObject.FindObjectOfType(typeof(spt_monsterMotivation)) as spt_monsterMotivation;
	}
	
	// Update is called once per frame
	void Update () {

        if (networkScript == null)
        {
            networkScript = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
        }
        else
        {
            if (networkScript.loaded && networkScript.loaded == true && loadedTheNetwork == false)
            {
                //Populates the checkpoint list with puzzle states and their times.
                //  Currently, the players have 30 seconds to complete each task.
                for (int i = 0; i < networkScript.PuzzleStates.Count; i++)
                {
                    if (networkScript.PuzzleStates[i].name == null || checkpoints == null) continue;
                     checkpoints.Add(new puzzleStateWithCheckpointTime(networkScript.PuzzleStates[i].name, ((i + 1) * 30)));
                }

                InvokeRepeating("checkForDifficultyChange", 1, 1);
                loadedTheNetwork = true;
            }
        }
	}

    void raiseTheDifficulty(){
        motivationScript.lowerTheThreshold();
    }

    void lowerTheDifficulty(){
        motivationScript.raiseTheThreshold();
    }

    // Function used to check if a difficulty change needs to occur; called every second
    void checkforDifficultyChange()
    {
        elapsedTime = elapsedTime + 1;

        // If the players are 15 seconds ahead of the ideal checkpoint time, raise the difficulty.
        if (elapsedTime <= checkpoints[currentPuzzleStateIndex].time - 15){
            raiseTheDifficulty();
        }
        
        // If the players are 15 seconds behind the ideal checkpoint time, lower the difficulty.
        else if (elapsedTime >= checkpoints[currentPuzzleStateIndex].time + 15){
            lowerTheDifficulty();
        }

        // If the players have completed the current puzzle state, update the index of the current puzzle state to the next item.
        if (networkScript.PuzzleStates[currentPuzzleStateIndex].state == true){
            currentPuzzleStateIndex = currentPuzzleStateIndex + 1;
        }
    }
}

// Basic class used to store puzzle state/checkpoint time pairs.
public class puzzleStateWithCheckpointTime{

    public string name;
    public int time;

    private puzzleStateWithCheckpointTime() { }

    public puzzleStateWithCheckpointTime(string s, int i)
    {
        name = s;
        time = i;
    }
}
