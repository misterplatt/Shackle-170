/* spt_lossListener.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 5/5/2016
 * 
 * This file looks at the playerLoss puzzle state, and triggers the UI to display a loss condition if it happens. 
 * Added attackSound call
 **/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_lossListener : MonoBehaviour {

    int index;
    bool gotIndex = false;
    bool once = false;
    GameObject player;
    spt_monsterAudio monsterAudio;


    void Start()
    {
        player = this.transform.root.gameObject;
        monsterAudio = GameObject.Find("MonsterStandin").GetComponent<spt_monsterAudio>();
    }
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("i")) monsterAudio.playAttackSound();
        // If the network exists, and the playerLoss even hasn't already been found in the Puzzle states, find and save its index
        if (player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates != null && !gotIndex)
        {
            for (int i = 0; i < player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates.Count; i++)
            {
                if (player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[i].name == "playerLoss")
                {
                    index = i;
                }
                    
            }
            gotIndex = true;
        }

        // If the playerLoss event has been found in the Puzzle States, look at it. If it is ever flipped to true, trigger the UI to display loss screen.
        if (gotIndex)
        {
            if (player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates.Count == 0) return;
            if ((player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[index].state == true)) //&& !once)
            {
                monsterAudio.playAttackSound();
                Debug.Log(transform.parent.parent.parent.name + " " + player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[index].name);
                GetComponent<Text>().text = "You lose";
                GetComponent<Text>().enabled = true;
                //transform.parent.FindChild("FadePanel").GetComponent<VRStandardAssets.Utils.VRCameraFade>().FadeOut(false);
                once = true;
                spt_LayeredAudioManager.musicPlay = false;
            }
        }
	}
}
