/*
spt_victoryListener
Author(s): Hayden Platt
Revision 1
Listens for NPL puzzle completion to show the 
"YOU WIN" UI Text.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_victoryListener : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //If the puzzleCompletion puzzlestate is true, set Win Text to visible and start camera fadeout
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[0].state == true || Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<Text>().enabled = true;
            transform.parent.FindChild("FadePanel").GetComponent<VRStandardAssets.Utils.VRCameraFade>().FadeOut(false);
        }
    }
}
