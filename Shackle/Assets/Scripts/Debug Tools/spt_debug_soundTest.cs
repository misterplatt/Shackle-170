using UnityEngine;
using System.Collections;

public class spt_debug_soundTest : MonoBehaviour {
    // Make sure to attatch the audio souce to this script in the Unity editor!
    public AudioSource source;

    // Array of warning noises. This array needs to be instantiated in the Unity editor.
    public AudioClip sound;
    	
	// Update is called once per frame
	void Update () {
        if (source == null)
        {
            source = GameObject.Find("MonsterStandin").GetComponent<AudioSource>();
            source.clip = sound;
        }
	    if (Input.GetKeyDown(KeyCode.P)) source.PlayOneShot(source.clip);
    }
}
