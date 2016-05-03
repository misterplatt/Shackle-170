/*
spt_TVScreenListener

Author(s): Hayden Platt, Dara Diba

Revision 2

Listens for event cues to show and alter TV screen sprite and sound.
*/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_TVScreenListener : NetworkBehaviour {

    private AudioSource tvStaticSound;
    private bool soundPlayed;

    private SpriteRenderer staticSprite;
    private Light staticLight;
    
    void Start(){
        tvStaticSound = GetComponent<AudioSource>();
        staticSprite = GetComponent<SpriteRenderer>();
        staticLight = GetComponent<Light>();
        soundPlayed = false;
    }

    // Update is called once per frame
    void Update () {

        //If the tvOn network state is true, turn on the static and noise
        if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true &&
            GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == false)
        {
            staticSprite.enabled = true;
            staticLight.enabled = true;
            if (!soundPlayed)
            {
                tvStaticSound.Play();
                soundPlayed = true;
            }
        }

        //If tvOn and correctChannel are true, turn on the success screen and make sure static is playing
        else if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == true &&
            GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == true)
        {
            transform.FindChild("Arrow").GetComponent<SpriteRenderer>().enabled = true;
            staticSprite.color = Color.green;
            staticLight.color = Color.green;
            staticSprite.enabled = true;
            staticLight.enabled = true;
            if (!soundPlayed)
            {
                tvStaticSound.Play();
                soundPlayed = true;
            }
        }

        //Otherwise, turn the screen off and stop the static sound
        else if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[2].state == false ||
            GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[3].state == false)
        {
            transform.FindChild("Arrow").GetComponent<SpriteRenderer>().enabled = false;
            staticSprite.enabled = false;
            staticLight.enabled = false;
            tvStaticSound.Stop();
            soundPlayed = false;
        }
    }
}
