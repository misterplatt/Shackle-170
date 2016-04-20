/*
spt_controlsListener

Author(s): Hayden Platt

Revision 1

Listens for the player pressing select to view all controls.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class spt_controlsListener : MonoBehaviour {

    private RawImage imageref;
    public Sprite controls_noMove;

	// Use this for initialization
	void Start () {
        imageref = GetComponent<RawImage>();
        //If level is garage, change image to omit "Grip chair" controls
        if (SceneManager.GetActiveScene().name == "net_SpookyGarage") imageref.texture = controls_noMove.texture;
	}
	
	// Update is called once per frame
	void Update () {
        //If the controls image is active and the player presses B, disable the image
        if(imageref.enabled && spt_playerControls.bButtonPressed()) imageref.enabled = false;
        //If Select is pressed, toggle the image's ability
        if (Input.GetButtonDown("Select")) imageref.enabled = !imageref.enabled;
	}
}
