﻿/*
spt_victoryListener

Author(s): Hayden Platt, Dara Diba

Revision 3

Listens for NPL puzzle completion to show the 
"YOU WIN" UI Text.
Adjusted transition rumbles. - Dara
*/

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class    spt_victoryListener : MonoBehaviour
{
    private bool once = false;
    private bool expandLight = false;
    private AudioSource transitionA;
    private Light winLight;
    private spt_monsterMotivation monster;
    NetworkManager manager;

    private const float TRANSITION_TIME = 14.779F;

    void Start()
    {
        monster = GameObject.FindObjectOfType<spt_monsterMotivation>();
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        //If a victory light exists, retrieve it's AudioSource and Light components
        if (GameObject.Find("victory_light") != null) {
            transitionA = GameObject.Find("victory_light").GetComponent<AudioSource>();
            winLight = GameObject.Find("victory_light").GetComponent<Light>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            spt_LayeredAudioManager.musicPlay = false;
            transitionA.Play();
            StartCoroutine(transitionRumbleShit());
        }
        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates.Count == 0) return;
        //If the puzzleCompletion puzzlestate is true, set Win Text to visible and start camera fadeout        
        if (player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[0].state == true && !once)
        {
            /* monster.angerUpdateDisabled = true;
            GetComponent<Text>().enabled = true;
            winLight.enabled = true;
            expandLight = true;
             * */
            //GameObject.Find("player_B_light").GetComponent<Light>().enabled = true;
            transform.parent.FindChild("FadePanel").GetComponent<VRStandardAssets.Utils.VRCameraFade>().FadeOut(false);
            once = true;
            spt_LayeredAudioManager.musicPlay = false;
            transitionA.Play();
            StartCoroutine(transitionRumbleShit());
            spt_LayeredAudioManager.musicPlay = false;
            transitionA.Play();
            StartCoroutine(transitionRumbleShit());
            loadNextLevel();
        }

        spt_NetworkPuzzleLogic networkScript = GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
        for (int i = 0; i < networkScript.PuzzleStates.Count; i++)
        {
            if (networkScript.PuzzleStates[i].name == "puzzleCompletionMonster" && networkScript.PuzzleStates[i].state == true)
            {
                monster.angerUpdateDisabled = true;
                GetComponent<Text>().enabled = true;
                winLight.enabled = true;
                expandLight = true;
            }
        }

        if (expandLight && winLight.spotAngle < 179) {
            winLight.spotAngle += .25f;
        }
    }

    void loadNextLevel() {        
        string nextLevel;
        NetworkIdentity thisId = GetComponentInParent<NetworkIdentity>();
        string lvlName = SceneManager.GetActiveScene().name;
        if (lvlName == "net_SpookyGarage")
        {
            nextLevel = "net_RangerOutpost";
        }
        else if (lvlName == "net_RangerOutpost")
        {
            nextLevel = "net_OpticsLab";
        }
        else return;

        //if hosting, start new level
        if (thisId.isServer) {
            StartCoroutine(changeLevel(nextLevel));
        }
    }

    IEnumerator changeLevel(string lvl) {
        yield return new WaitForSeconds(TRANSITION_TIME);
        string currentLevel = SceneManager.GetActiveScene().name;
        manager.ServerChangeScene(lvl);
        SceneManager.UnloadScene(currentLevel);
        
    }

    // Calls the controller to rumble based off the transition sounds
    IEnumerator transitionRumbleShit()
    {
        yield return new WaitForSeconds(5.9f);
        spt_playerControls.controllerVibration("Both", 1.0f, 2.2);
        yield return new WaitForSeconds(0.8f);
        spt_playerControls.controllerVibration("Both", 1.0f, 2.9);
        yield return new WaitForSeconds(1.2f);
        spt_playerControls.controllerVibration("Both", 1.0f, 2.2);
    }


}
