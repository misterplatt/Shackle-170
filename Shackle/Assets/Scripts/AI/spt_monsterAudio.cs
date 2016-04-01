/* spt_monsterAudio.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 3/5/2016
 * 
 * This script handles all of the audio required for the monster. **/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_monsterAudio : NetworkBehaviour {

    // Make sure to attatch the audio souce to this script in the Unity editor!
    public AudioSource source;
    
    // Array of warning noises. This array needs to be instantiated in the Unity editor.
    public AudioClip[] warningSounds;
    public AudioClip[] ambientSounds;

    //network synced soundIndex
    [SyncVar]
    public int ambSoundInd = -1;
    public int wngSoundInd = -1;

    private AudioClip chosenWarningSound;
    private AudioClip chosenAmbientSound;

    // Called on Start, loads references to the first ambient and warning sounds to be played.
    void Start(){
        if (isServer)
        {
            ambSoundInd = Random.Range(0, warningSounds.Length);
            wngSoundInd = Random.Range(0, ambientSounds.Length);

            chosenWarningSound = warningSounds[wngSoundInd];
            chosenAmbientSound = ambientSounds[ambSoundInd];
            InvokeRepeating("checkForAmbientNoise", 15, 15);
        }
    }

    void Update()
    {
        if (chosenWarningSound == null) chosenWarningSound = warningSounds[wngSoundInd];
        if (chosenAmbientSound == null) chosenAmbientSound = ambientSounds[ambSoundInd];
    }

    //Called when a warning noise is needed. Plays the currently loaded one, then loads a new one.
    public void playWarningNoise(){
        source.clip = chosenWarningSound;
        source.PlayOneShot(source.clip);
        chosenWarningSound = warningSounds[Random.Range(0, warningSounds.Length)];
    }

    // Called when an ambient sound is needed. Plays the currently loaded one, then loads a new one.
    public void playAmbientNoise()
    {
        source.clip = chosenAmbientSound;
        source.PlayOneShot(source.clip);
        chosenAmbientSound = ambientSounds[Random.Range(0, ambientSounds.Length)];
    }

    // Will randomly play an ambient noise, at most every 15 seconds.
    private void checkForAmbientNoise()
    {
        int randomInt = Random.Range(0, 10);
        if (randomInt == 0)
            playAmbientNoise();
    }
}
