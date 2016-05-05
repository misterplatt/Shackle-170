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

        override protected void Start() {
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
                if (gameObject.name == "LaserSwitch") GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", true, "LaserSwitch");

            }
            else if (currentState == false)
            {
                laserMesh.enabled = currentState;
                laserCollider.enabled = currentState;
                if (gameObject.name == "LaserSwitch") GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", false, "LaserSwitch");
            }
        }

        //Plug handleDown
        override protected void HandleDown() { }
    }
}