/* spt_angerMovement.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 5/20/2016
 * 
 * This file accrues monster anger from player movement. **/

using UnityEngine;
using System.Collections;

public class spt_angerMovement : MonoBehaviour {

    private int timer = 0;
    private AudioSource audio;

    // Use this for initialization
	void Start () {
        InvokeRepeating("updateTimer", 1, 1);
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void updateTimer()
    {
        if (audio.isPlaying)
        {
            GameObject.Find("MonsterStandin").GetComponent<spt_monsterMotivation>().updateAngerMovement(2);
            Debug.Log("Updating anger from player movement.");
        }
    }
}
