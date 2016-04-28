﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_flashlight : NetworkBehaviour {

    public Light flashlight;

    [SyncVar]
    public bool isVisible;
    private AudioSource flashlightSound;

    public AudioClip flashlightOn;
    public AudioClip flashlightOff;

    // Get reference to Light component
    void Awake()
    {
        flashlight.enabled = false;
        isVisible = false;
        flashlightSound = GetComponent<AudioSource>();
    }

    // Toggle component on and off on rightThumbstick press
    void Update()
    {
        if (!isLocalPlayer) return;
        if (spt_playerControls.rightThumbstickButtonPressed() || Input.GetKeyDown(KeyCode.F))
        {
            if (flashlight.enabled) flashlightSound.clip = flashlightOn;
            if (!flashlight.enabled) flashlightSound.clip = flashlightOff;
            flashlightSound.Play();
            flashlight.enabled = !flashlight.enabled;
            transform.Find("Camera Player/pFlashLight").gameObject.GetComponent<spt_angerObject>().toggleVisibility();
            if (!isServer) CmdToggleFlashLightVisibility(this.name);
        }

        /*spt_monsterMotivation monster = GameObject.Find("MonsterStandin").gameObject.GetComponent<spt_monsterMotivation>();
        if ((monster.isAttacking && isServer && monster.whichPlayer == 0) || (monster.isAttacking && !isServer && monster.whichPlayer == 1))
            monster.attackAfterFlashlightToggle();
            */
    }

    [Command]
    void CmdToggleFlashLightVisibility( string pName )
    {
        GameObject pFlashlight = GameObject.Find(pName).transform.Find("Camera Player/pFlashLight").gameObject;
        pFlashlight.GetComponent<spt_angerObject>().toggleVisibility();
    }
}
