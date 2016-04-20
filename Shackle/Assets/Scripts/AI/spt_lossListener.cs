/* spt_lossListener.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 2/26/2016
 * 
 * This file looks at the playerLoss puzzle state, and triggers the UI to display a loss condition if it happens. **/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_lossListener : MonoBehaviour {

    int index;
    bool gotIndex = false;
    bool once = false;
	
	// Update is called once per frame
	void Update () {

        // If the network exists, and the playerLoss even hasn't already been found in the Puzzle states, find and save its index
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates != null && !gotIndex)
        {
            for (int i = 0; i < GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates.Count; i++)
            {
                if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[i].name == "playerLoss")
                    index = i;
            }
            gotIndex = true;
        }

        // If the playerLoss event has been found in the Puzzle States, look at it. If it is ever flipped to true, trigger the UI to display loss screen.
        if (gotIndex)
        {
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[index].state == true && !once)
            {
                GetComponent<Text>().text = "You lose";
                GetComponent<Text>().enabled = true;
                //transform.parent.FindChild("FadePanel").GetComponent<VRStandardAssets.Utils.VRCameraFade>().FadeOut(false);
                once = true;
            }
        }
	}
}
