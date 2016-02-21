/* spt_monsterAudio.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 2/21/2016
 * 
 * This script handles all of the audio required for the monster. **/

using UnityEngine;
using System.Collections;

public class spt_monsterAudio : MonoBehaviour {

    // Make sure to attatch the audio souce to this script in the Unity editor!
    public AudioSource source;
    
    // Array of warning noises. This array needs to be instantiated in the Unity editor.
    public AudioClip[] warningSounds;

    //Called when a warning noise is needed. Picks a random one from the list.
    public void playWarningNoise(){
        source.clip = warningSounds[Random.Range(0, warningSounds.Length)];
        source.PlayOneShot(source.clip);
    }
	
}
