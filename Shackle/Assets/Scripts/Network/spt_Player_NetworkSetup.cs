﻿/* spt_Player_NetworkSetup.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This file initializes the player object in its networked environment, enabling the components it should use.
 */

using UnityEngine;
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

            //GameObject mCam = GameObject.Find("Main Camera");
            //if (mCam != null) { GameObject.Find("Main Camera").SetActive(false); Debug.Log("TEST"); }
            GetComponent<Camera>().enabled = true;
            //GetComponent<CharacterController>().enabled = true;
            //GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            FPSCharacterCam.enabled = true;
            //Commented audiolistener out to get the basic background music to work
            //audiolistener.enabled = true;
        }
        
	}
    
}
