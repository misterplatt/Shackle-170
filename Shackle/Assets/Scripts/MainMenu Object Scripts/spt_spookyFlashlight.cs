using UnityEngine;
using System.Collections;

public class spt_spookyFlashlight : MonoBehaviour {

    private float timer = 0;
    private bool flickering = false;
    private Light flashlight;

    public float minFlickerSpeed = 0.1f;
    public float maxFlickerSpeed = 0.5f;

	// Use this for initialization
	void Start () {
        timer = Random.Range(3, 10);
        flashlight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if(timer < 0 && !flickering)
        {
            timer = Random.Range(.5f, 2);
            flickering = true;
        }
        if(flickering) StartCoroutine("spookyFlicker");
        if (timer < 0 && flickering) {
            StopCoroutine("spookyFlicker");
            flashlight.enabled = true;
            flashlight.color = new Color32(215,212,104, 255);
            timer = Random.Range(3, 10);
            flickering = false;
        }
	}

    IEnumerator spookyFlicker() {
        flashlight.color = Color.red;
        flashlight.enabled = false;
        yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        flashlight.enabled = true;
    }
}
