using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_flashlight : NetworkBehaviour {

    public Light flashlight;

    [SyncVar]
    public bool isVisible; 

    // Get reference to Light component
    void Awake()
    {
        flashlight = transform.Find("Flashlight").gameObject.GetComponent<Light>();
        flashlight.enabled = false;
        isVisible = false;
    }

    // Toggle component on and off on rightThumbstick press
    void Update () {
        if (!isLocalPlayer) return;
        if (spt_playerControls.rightThumbstickButtonPressed() || Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
            transform.Find("pFlashLight").gameObject.GetComponent<spt_angerObject>().toggleVisibility();
            if (!isServer) CmdToggleFlashLightVisibility(this.name);
        }
    }

    [Command]
    void CmdToggleFlashLightVisibility( string pName )
    {
        GameObject pFlashlight = GameObject.Find(pName).transform.Find("pFlashLight").gameObject;
        pFlashlight.GetComponent<spt_angerObject>().toggleVisibility();
    }
}
