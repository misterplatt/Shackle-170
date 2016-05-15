﻿/*
spt_laserSwitch

Author(s): Hayden Platt, Dara Diba, Lauren

Revision 2

Child of the base interactiveObject class
Allows for an item to be switched from a false state to a true state and vice-versa.
Added sound functionality - Dara
*/


using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_laserSwitch : spt_baseInteractiveObject
    {
        [SerializeField] LayerMask laserLayers;
        private bool currentState = false;

        private MeshRenderer laserMesh;
        private BoxCollider laserCollider;

        public static bool local_isLaserOn = false;

        private MeshRenderer metalSwitchOff;
        private MeshRenderer metalSwitchOn;
        private AudioSource aSource;
        public AudioClip laserStart;
        public AudioClip laserPurr;

        override protected void Start() {
            metalSwitchOff = transform.FindChild("Joystick_switch").GetComponent<MeshRenderer>();
            metalSwitchOn = transform.FindChild("Joystick_switch (1)").GetComponent<MeshRenderer>();
            laserMesh = transform.FindChild("Laser").gameObject.GetComponent<MeshRenderer>();
            laserCollider = transform.FindChild("Laser").gameObject.GetComponent<BoxCollider>();
            aSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) currentState = !currentState;
            //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", true, "Joystick_base")

            currentState = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state;
            //Change laser LineRenderer's enabled status on switch click
            if (currentState == true)
            {
                aSource.clip = laserStart;
                aSource.Play();
                Invoke("LaserMachinePurr", 10f);
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitchOff.enabled = false;
                metalSwitchOn.enabled = true;

            }
            else if (currentState == false)
            {
                aSource.Stop();
                aSource.loop = false;
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitchOff.enabled = true;
                metalSwitchOn.enabled = false;
            }
        }

        override protected void clickSuccess()
        {
            currentState = !currentState;

            //Change laser LineRenderer's enabled status on switch click
            if (currentState == true)
            {
                aSource.clip = laserStart;
                aSource.Play();
                Invoke("LaserMachinePurr", 10f);
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitchOff.enabled = false;
                metalSwitchOn.enabled = true;

                //NPL Update
                local_isLaserOn = true;
                spt_WorldState.worldStateChanged = true;

                //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", true, "Joystick_base");

            }
            else if (currentState == false)
            {
                aSource.Stop();
                aSource.loop = false;
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitchOff.enabled = true;
                metalSwitchOn.enabled = false;

                //NPL Update
                local_isLaserOn = false;
                spt_WorldState.worldStateChanged = true;

                //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", false, "Joystick_base");
            }
        }

        //Plug handleDown
        override protected void HandleDown() { }

        //Simple function for changing the lasermachine sound to a loopable clip
        void LaserMachinePurr()
        {
            aSource.clip = laserPurr;
            aSource.loop = true;
            aSource.Play();
        }

        public override void resetItem()
        {
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == true) clickSuccess();
        }
    }
}