using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_flashlight : NetworkBehaviour {

    public Light flashlight;

    // Get reference to Light component
    void Awake()
    {
        flashlight = transform.Find("Flashlight").gameObject.GetComponent<Light>();
        flashlight.enabled = false;
    }

    // Toggle component on and off on rightThumbstick press
    void Update () {
        if (!isLocalPlayer) return;
        if (spt_playerControls.rightThumbstickButtonPressed() || Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
            transform.Find("pFlashLight").gameObject.GetComponent<spt_angerObject>().toggleVisibility();
        }
    }
}
