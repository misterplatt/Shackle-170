using UnityEngine;
using System.Collections;

public class spt_flashlight : MonoBehaviour {

    public Light flashlight;

    // Get reference to Light component
    void Awake()
    {
        flashlight = gameObject.GetComponent<Light>();
        flashlight.enabled = false;
    }

    // Toggle component on and off on rightThumbstick press
    void Update () {
        if (spt_playerControls.rightThumbstickButtonPressed() || Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
