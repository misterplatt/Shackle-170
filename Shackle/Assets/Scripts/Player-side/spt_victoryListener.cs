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
    private spt_monsterMotivation monster;


    void Start()
    {
<<<<<<< HEAD
        transitionA = GameObject.Find("victory_light").GetComponent<AudioSource>();
        winLight = GameObject.Find("victory_light").GetComponent<Light>();
        monster = GameObject.FindObjectOfType<spt_monsterMotivation>();

=======
        if (GameObject.Find("victory_light") != null) {
            transitionA = GameObject.Find("victory_light").GetComponent<AudioSource>();
            winLight = GameObject.Find("victory_light").GetComponent<Light>();
        }
>>>>>>> b6516ca4d05a77d6048c3a334b9d9a44db722436
    }

    // Update is called once per frame
    void Update()
    {
        //If the puzzleCompletion puzzlestate is true, set Win Text to visible and start camera fadeout
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[0].state == true && !once)
        {
            monster.angerUpdateDisabled = true;
            GetComponent<Text>().enabled = true;
            winLight.enabled = true;
            expandLight = true;
            //GameObject.Find("player_B_light").GetComponent<Light>().enabled = true;
            transform.parent.FindChild("FadePanel").GetComponent<VRStandardAssets.Utils.VRCameraFade>().FadeOut(false);
            once = true;
            transitionA.Play();
            StartCoroutine(transitionRumbleShit());
        }

        if (expandLight && winLight.spotAngle < 179) {
            winLight.spotAngle += .25f;
        }
    }

    // Calls the controller to rumble based off the transition sounds
    IEnumerator transitionRumbleShit()
    {
        yield return new WaitForSeconds(5.8f);
        spt_playerControls.controllerVibration("Both", 1.0f, 4f);
        yield return new WaitForSeconds(1.5f);
        spt_playerControls.controllerVibration("Both", 1.0f, 8f);
        yield return new WaitForSeconds(2f);
        spt_playerControls.controllerVibration("Both", 1.0f, 5f);
    }


}
