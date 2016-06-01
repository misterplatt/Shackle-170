/*
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
using XInputDotNetPure;
using System;

public class    spt_victoryListener : MonoBehaviour
{
    private bool once = false;
    private bool expandLight = false;
    private AudioSource transitionA;
    public AudioClip finalTransition;
    private Light winLight;
    private spt_monsterMotivation monster;
    NetworkManager manager;
    static PlayerIndex playerIndex = 0;
    private static DateTime timer;
    private static DateTime timert;
    private static DateTime timerend;
    public static bool vibrationz = false;
    public static bool Rough = false;
    public static bool Smooth = false;
    public static bool Both = false;
    public static float vibrationTime = 0f;
    public static float vibrationForce = 0f;
    public static bool shitOnTheFloor = true;

    private const float TRANSITION_TIME = 14.779F;

    void Start()
    {
        monster = GameObject.FindObjectOfType<spt_monsterMotivation>();
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        once = false;

        //If a victory light exists, retrieve it's AudioSource and Light components
        if (GameObject.Find("victory_light") != null) {
            transitionA = GameObject.Find("victory_light").GetComponent<AudioSource>();
            winLight = GameObject.Find("victory_light").GetComponent<Light>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        // added vibrations here so the game would not freeze like how it does with my implementation in spt_playerControls
        if (vibrationz == true)
        {
            timer = DateTime.Now;
            if (shitOnTheFloor) timerend = timer.AddSeconds(vibrationTime);

            shitOnTheFloor = false;
            if (timer.Second < timerend.Second)
            {
                if (Rough == true) GamePad.SetVibration(playerIndex, vibrationForce, 0);
                if (Smooth == true) GamePad.SetVibration(playerIndex, 0, vibrationForce);
                if (Both == true) GamePad.SetVibration(playerIndex, vibrationForce, vibrationForce);
                timer = DateTime.Now;
            }
            else
            {
                GamePad.SetVibration(playerIndex, 0, 0);
                vibrationz = false;
                shitOnTheFloor = true;
            }
        }

            if (Input.GetKeyDown(KeyCode.Q))
        {
            spt_LayeredAudioManager.musicPlay = false;
            transitionA.Play();
            StartCoroutine(transitionRumble());
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[0].state == false) once = false;
        if (player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates.Count == 0) return;
        //If the puzzleCompletion puzzlestate is true, set Win Text to visible and start camera fadeout      
        Debug.Log("once : " + once );  

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
            if (SceneManager.GetActiveScene().name != "net_OpticsLab")
            {
                spt_LayeredAudioManager.musicPlay = false;
                //transitionA.Play();
                StartCoroutine(transitionRumble());
            }
            else {
                transitionA.clip = finalTransition;
                StartCoroutine(finalRumble());
            }
            //spt_LayeredAudioManager.musicPlay = false;
            transitionA.Play();
            //StartCoroutine(transitionRumble());

            //update dda
            GameObject.Find("DDA").GetComponent<spt_DDAStorage>().incrementDiffValue();

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
            nextLevel = "net_RangerOutpost_crash";
        }
        else if (lvlName == "net_RangerOutpost_crash")
        {
            Debug.Log("Right...  I guess I just thought...");
            nextLevel = "net_OpticsLab";
        }
        else if (lvlName == "net_OpticsLab")
        {
            nextLevel = "net_FinalScene";
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
    IEnumerator transitionRumble()
    {
        yield return new WaitForSeconds(5.9f);
        Both = true;
        vibrationForce = 1.0f;
        vibrationTime = 2.2f;
        vibrationz = true;
        //StartCoroutine(spt_playerControls.NewVibrator("Both", 1.0f, 2.2));
        //spt_playerControls.controllerVibration("Both", 1.0f, 2.2);
        //yield return new WaitForSeconds(0.8f);
        yield return new WaitForSeconds(2.2f);
        Both = true;
        vibrationForce = 1.0f;
        vibrationTime = 2.9f;
        vibrationz = true;
        //StartCoroutine(spt_playerControls.NewVibrator("Both", 1.0f, 2.9));
        //spt_playerControls.controllerVibration("Both", 1.0f, 2.9);
        //yield return new WaitForSeconds(1.2f);
        yield return new WaitForSeconds(3f);
        Both = true;
        vibrationForce = 1.0f;
        vibrationTime = 2.2f;
        vibrationz = true;
        
        //StartCoroutine(spt_playerControls.NewVibrator("Both", 1.0f, 2.2));
        //spt_playerControls.controllerVibration("Both", 1.0f, 2.2);  
    }

    // Calls the controller to rumble based off the transition sounds
    IEnumerator finalRumble()
    {
        //MAKE ME DARA
        Both = true;
        vibrationTime = 11f;
        vibrationForce = .1f;
        vibrationz = true;
        // I know this is extremely ugly code, just want to see if it works
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;
        yield return new WaitForSeconds(1f);
        vibrationForce += .1f;

        //Actual solution if the top calls work
        /*
        
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1f);
            vibrationForce += .1f;
        }
        */
    }

}
