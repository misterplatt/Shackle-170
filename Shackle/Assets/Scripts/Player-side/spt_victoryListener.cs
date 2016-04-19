/*
spt_victoryListener

Author(s): Hayden Platt

Revision 2

Listens for NPL puzzle completion to show the 
"YOU WIN" UI Text.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_victoryListener : MonoBehaviour
{
    private bool once = false;
    private bool expandLight = false;
    private AudioSource transitionA;
    private Light winLight;

    void Start()
    {
        transitionA = GameObject.Find("victory_light").GetComponent<AudioSource>();
        winLight = GameObject.Find("victory_light").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the puzzleCompletion puzzlestate is true, set Win Text to visible and start camera fadeout
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[0].state == true && !once)
        {
            GetComponent<Text>().enabled = true;
            winLight.enabled = true;
            expandLight = true;
            //GameObject.Find("player_B_light").GetComponent<Light>().enabled = true;
            transform.parent.FindChild("FadePanel").GetComponent<VRStandardAssets.Utils.VRCameraFade>().FadeOut(false);
            once = true;
            transitionA.Play();
        }

        if (expandLight && winLight.spotAngle < 179) {
            winLight.spotAngle += .25f;
        }
    }
}
