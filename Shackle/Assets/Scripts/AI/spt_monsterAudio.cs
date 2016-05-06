/* spt_monsterAudio.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 5/6/2016
 * 
 * This script handles all of the audio required for the monster. 
 * Added Attacksound - Dara
 **/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_monsterAudio : NetworkBehaviour
{

    // Make sure to attatch the audio souce to this script in the Unity editor!
    public AudioSource source;

    // Array of warning noises. This array needs to be instantiated in the Unity editor.
    public AudioClip[] warningSounds;
    public AudioClip[] ambientSounds;
    public AudioClip attackSound;

    //Networking Variables:
    [SyncVar]
    int ambSoundInd = -1;
    [SyncVar]
    int wngSoundInd = -1;
    bool soundPlayed = false;
    bool once = false;
    bool attackSoundPlayed = false;
    private spt_NetworkPuzzleLogic network;

    // Called on Start, loads references to the first ambient and warning sounds to be played.
    void Start()
    {
        /*
        chosenWarningSound = warningSounds[Random.Range(0, warningSounds.Length)];
        chosenAmbientSound = ambientSounds[Random.Range(0, warningSounds.Length)];
        */
        if (isServer) InvokeRepeating("checkForAmbientNoise", 15, 15);
    }

    void Update()
    {
        if (network == null) network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
        if (isServer)
        {
            if (soundPlayed)
            {
                ambSoundInd = -1;
                wngSoundInd = -1;
            }
        }

        if (ambSoundInd != -1 && !once) playAmbientNoise();
        else if (wngSoundInd != -1 && !once) playWarningNoise();
        if (soundPlayed) soundPlayed = false;
    }

    //code to ensure index is not reset until after sound completion
    //do so by only calling the function to change the indices to -1 after audio completes
    public delegate void AudioCallback();
    public void PlaySoundWithCallback(AudioClip clip, AudioCallback callback)
    {
        source.PlayOneShot(clip);
        StartCoroutine(DelayedCallback(clip.length, callback));
    }

    public IEnumerator DelayedCallback( float time, AudioCallback callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    void setPlayFlags()
    {
        soundPlayed = true;
        once = false;
    }

    //Called when a warning noise is needed. Plays the currently loaded one, then loads a new one.
    public void playWarningNoise()
    {
        once = true;
        source.clip = warningSounds[wngSoundInd];
        PlaySoundWithCallback(source.clip, setPlayFlags);
    }

    // Called when an ambient sound is needed. Plays the currently loaded one, then loads a new one.
    public void playAmbientNoise()
    {
        once = true;
        source.clip = ambientSounds[ambSoundInd];
        PlaySoundWithCallback(source.clip, setPlayFlags);
    }

    public void prepAmbientNoise()
    {
        if (!isServer) return;
        ambSoundInd = Random.Range(0, ambientSounds.Length);
    }

    public void prepWarningNoise()
    {
        if (!isServer) return;
        wngSoundInd = Random.Range(0, warningSounds.Length);
    }

    // Will randomly play an ambient noise, at most every 15 seconds.
    private void checkForAmbientNoise()
    {
        if (!isServer) return;
        int randomInt = Random.Range(0, 10);
        if (randomInt == 0)
            prepAmbientNoise();
    }

    public void playAttackSound()
    {
        if (!attackSoundPlayed)
        {
            source.clip = attackSound;
            PlaySoundWithCallback(source.clip, setPlayFlags);
            attackSoundPlayed = true;
        }
    }

}