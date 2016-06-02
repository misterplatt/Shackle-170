/* spt_Player_NetworkSetup.cs
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

    bool spawnAdjustment;
    Vector3 spawnPos;
    // Use this for initialization
    void Start () {

        spawnAdjustment = false;
        spawnPos = new Vector3(0.0F, 2.1F, -1.0F);
        if (isLocalPlayer)
        {

            //GameObject mCam = GameObject.Find("Main Camera");
            //if (mCam != null) { GameObject.Find("Main Camera").SetActive(false); Debug.Log("TEST"); }
            FPSCharacterCam.enabled = true;
            //GetComponent<CharacterController>().enabled = true;
            //GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            FPSCharacterCam.enabled = true;
            //Commented audiolistener out to get the basic background music to work
            //audiolistener.enabled = true;
        }
        
	}

    void Update()
    {
        if (isServer) return;

        if ( !this.transform.position.Equals(spawnPos) )this.transform.position = spawnPos;
        if (spawnAdjustment == false)
        {
            if (isServer)
            {
                spawnAdjustment = true;
                return;
            }
            else
            {
                this.transform.position = new Vector3(0.0F, 2.1F, -1.0F);
                spawnAdjustment = true;

            }
        }
    }
    
}
