/*
spt_laserSwitch

Author(s): Hayden Platt, Dara Diba, Lauren

Revision 2

Child of the base interactiveObject class
Allows for an item to be switched from a false state to a true state and vice-versa.
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

        override protected void Start() {
            metalSwitch = transform.FindChild("Joystick_switch").gameObject;
            initialPosition = metalSwitch.transform.position;
            initialRotation = metalSwitch.transform.rotation;
            laserMesh = transform.FindChild("Laser").gameObject.GetComponent<MeshRenderer>();
            laserCollider = transform.FindChild("Laser").gameObject.GetComponent<BoxCollider>();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) currentState = !currentState;
        }

        override protected void clickSuccess()
        {
            currentState = !currentState;

            //Change laser LineRenderer's enabled status on switch click
            if (currentState == true)
            {
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitch.transform.eulerAngles = new Vector3(-25.5f, 2.3f, -4f);
                if (gameObject.name == "Joystick_base") GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", true, "Joystick_base");

            }
            else if (currentState == false)
            {
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                metalSwitch.transform.position = initialPosition;
                metalSwitch.transform.rotation = initialRotation;
                if (gameObject.name == "Joystick_base") GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", false, "Joystick_base");
            }
        }

        //Plug handleDown
        override protected void HandleDown() { }
    }
}