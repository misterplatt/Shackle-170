/*
spt_controlsListener

Author(s): Hayden Platt

Revision 1

Listens
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_controlsListener : MonoBehaviour {

    private RawImage imageref;

	// Use this for initialization
	void Start () {
        imageref = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
        //If the controls image is active and the player presses B, disable the image
        if(imageref.enabled && spt_playerControls.bButtonPressed()) imageref.enabled = false;
        //If Select is pressed, toggle the image's ability
        if (Input.GetButtonDown("Select")) imageref.enabled = !imageref.enabled;
	}
}
