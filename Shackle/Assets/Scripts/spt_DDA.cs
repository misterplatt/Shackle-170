//Created by: Lauren Cunningham

/** This file is the one that handle the dynamic difficulty adjustment. **/

using UnityEngine;
using System.Collections;

public class spt_DDA : MonoBehaviour {

    private spt_monsterMotivation motivationScript;

	// Use this for initialization
	void Start () {
        motivationScript = GameObject.FindObjectOfType(typeof(spt_monsterMotivation)) as spt_monsterMotivation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void raiseTheDifficulty(){
        motivationScript.lowerTheThreshold();
    }

    void lowerTheDifficulty(){
        motivationScript.raiseTheThreshold();
    }
}
