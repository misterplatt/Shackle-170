using UnityEngine;
using System.Collections;

public class spt_flashlight : MonoBehaviour {

    public Light flashlight;

    // Use this for initialization
    void Awake()
    {
        flashlight = gameObject.GetComponent<Light>();
        flashlight.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (spt_playerControls.rightThumbstickButtonPressed() || Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
