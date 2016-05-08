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
        [SerializeField] LayerMask laserLayers;
        private bool currentState = false;

        private MeshRenderer laserMesh;
        private BoxCollider laserCollider;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private GameObject metalSwitch;
        private AudioSource aSource;
        public AudioClip laserStart;
        public AudioClip laserPurr;

        override protected void Start() {
            metalSwitch = transform.FindChild("Joystick_switch").gameObject;
            initialPosition = metalSwitch.transform.position;
            initialRotation = metalSwitch.transform.rotation;
            laserMesh = transform.FindChild("Laser").gameObject.GetComponent<MeshRenderer>();
            laserCollider = transform.FindChild("Laser").gameObject.GetComponent<BoxCollider>();
            aSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) currentState = !currentState;
            //GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", true, "Joystick_base")

            currentState = (GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[4].state);

            if (currentState == true)
            {
                aSource.clip = laserStart;
                aSource.Play();
                Invoke("LaserMachinePurr", 10f);
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitch.transform.eulerAngles = new Vector3(-25.5f, 2.3f, -4f);
                if (gameObject.name == "Joystick_base") GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", true, "Joystick_base");

            }
            else if (currentState == false)
            {
                aSource.Stop();
                aSource.loop = false;
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitch.transform.position = initialPosition;
                metalSwitch.transform.rotation = initialRotation;
                if (gameObject.name == "Joystick_base") GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", false, "Joystick_base");
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
                metalSwitch.transform.eulerAngles = new Vector3(-25.5f, 2.3f, -4f);
                if (gameObject.name == "Joystick_base") GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", true, "Joystick_base");

            }
            else if (currentState == false)
            {
                aSource.Stop();
                aSource.loop = false;
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitch.transform.position = initialPosition;
                metalSwitch.transform.rotation = initialRotation;
                if (gameObject.name == "Joystick_base") GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", false, "Joystick_base");
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
            clickSuccess();
        }
    }
}