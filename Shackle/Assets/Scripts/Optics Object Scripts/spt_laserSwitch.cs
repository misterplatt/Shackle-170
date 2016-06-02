/*
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
        [SerializeField]
        LayerMask laserLayers;
        private bool currentState;

        private MeshRenderer laserMesh;
        private BoxCollider laserCollider;

        public static bool local_isLaserOn = false;

        private MeshRenderer metalSwitchOff;
        private MeshRenderer metalSwitchOn;
        private AudioSource aSource;
        public AudioClip laserStart;
        public AudioClip laserPurr;
        private bool once = false;

        override protected void Start()
        {
            currentState = false;
            metalSwitchOff = transform.FindChild("Joystick_switch").GetComponent<MeshRenderer>();
            metalSwitchOn = transform.FindChild("Joystick_switch (1)").GetComponent<MeshRenderer>();
            laserMesh = transform.FindChild("Laser").gameObject.GetComponent<MeshRenderer>();
            laserCollider = transform.FindChild("Laser").gameObject.GetComponent<BoxCollider>();
            aSource = GetComponent<AudioSource>();
        }

        override protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Comma)) resetItem();
            if (Input.GetKeyDown(KeyCode.L))
            {

                currentState = !currentState;
                local_isLaserOn = !local_isLaserOn;
                spt_WorldState.worldStateChanged = true;
            }
            //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", true, "Joystick_base")

            //Change laser LineRenderer's enabled status on switch click
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == true)
            {
                if (!once)
                {
                    laserMesh.enabled = true;
                    laserCollider.enabled = true;
                    aSource.clip = laserStart;
                    aSource.Play();
                    once = true;
                    Invoke("LaserMachinePurr", 10f);
                }
                metalSwitchOff.enabled = false;
                metalSwitchOn.enabled = true;
            }
            else if (GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == false)
            {
                laserMesh.enabled = false;
                laserCollider.enabled = false;
                aSource.loop = false;
                aSource.Stop();
                metalSwitchOff.enabled = true;
                metalSwitchOn.enabled = false;
                once = false;
                local_isLaserOn = false;
                spt_WorldState.worldStateChanged = true;
            }
            //laserMesh.enabled = currentState;
            //laserCollider.enabled = currentState;
            //local_isLaserOn = currentState;

        }

        override protected void clickSuccess()
        {
            currentState = !currentState;

            //Change laser LineRenderer's enabled status on switch click
            if (currentState == true)
            {
                //aSource.clip = laserStart;
                //aSource.Play();
                //Invoke("LaserMachinePurr", 10f);
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
                //aSource.Stop();
                //aSource.loop = false;
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitchOff.enabled = true;
                metalSwitchOn.enabled = false;

                //NPL Update
                local_isLaserOn = false;
                spt_WorldState.worldStateChanged = true;


                //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", false, "Joystick_base");
            }
            //currentState = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state;

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
            if (GameObject.Find(GameObject.Find("WorldState").GetComponent<spt_WorldState>().localPlayer).GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state == true) clickSuccess();
        }
    }
}