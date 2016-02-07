﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class spt_Player_NetworkSetup : NetworkBehaviour {

    [SerializeField]
    Camera FPSCharacterCam;
    [SerializeField]
    AudioListener audiolistener;
    // Use this for initialization
    void Start () {
	
        if(isLocalPlayer)
        {
            Debug.Log("Test");
            GameObject mCam = GameObject.Find("Main Camera");
            if (mCam != null) { GameObject.Find("Main Camera").SetActive(false); Debug.Log("TEST"); }
            GetComponent<CharacterController>().enabled = true;
            //GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            FPSCharacterCam.enabled = true;
            audiolistener.enabled = true;
        }
        
	}
    
}
