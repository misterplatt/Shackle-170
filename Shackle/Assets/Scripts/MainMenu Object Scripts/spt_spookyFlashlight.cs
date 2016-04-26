/*
spt_spookyFlashlight

Author(s): Hayden Platt

Revision 1

Script which allows temporarily "possesses" the flashlight in the main
menu, turning it red and causing it to flicker.
*/
using UnityEngine;
using System.Collections;

public class spt_spookyFlashlight : MonoBehaviour {

    //Random range for how long the flashlight should stay normal for
    private float minNormalTime = 6f;
    private float maxNormalTime = 20f;

    private float timer = 0;
    private bool flickering = false;
    private Light flashlight;

    public float minFlickerSpeed = 0.2f;
    public float maxFlickerSpeed = 1f;

	// Use this for initialization
	void Start () {
        timer = Random.Range(minNormalTime, maxNormalTime);
        flashlight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime; //Decrement timer every frame

        //If the timer runs out and flashlight hasn't been flickering, start flickering
        if (timer < 0 && !flickering)
        {
            timer = Random.Range(.5f, 1.5f);
            flickering = true;
        }
        if(flickering) StartCoroutine("spookyFlicker");

        //If timer runs out and flashlight has been flickering, set it back to normal
        if (timer < 0 && flickering) {
            StopCoroutine("spookyFlicker");
            flashlight.enabled = true;
            flashlight.color = new Color32(215,212,104,255);
            timer = Random.Range(minNormalTime, maxNormalTime);
            flickering = false;
        }
	}

    //Coroutine which turns the flashlight red and flickers it
    IEnumerator spookyFlicker() {
        flashlight.color = Color.red;
        flashlight.enabled = false;
        yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        flashlight.enabled = true;
    }
}
