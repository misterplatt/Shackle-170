/*
spt_mirror

Author(s): Hayden Platt, Dara Diba

Revision 3

Inherits basePickup, but contains added functionality
which activates its laser child when a laser collides with it.
*/

using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_mirror : spt_interactivePickUp
    {
        private MeshRenderer laserMesh;
        private BoxCollider laserCollider;
        private spt_mirrorSync mirrorSync;

        public bool placed = false;

        private int laserCount = 0;

        protected override void Start()
        {
            base.Start();
            laserMesh = transform.FindChild("Laser").gameObject.GetComponent<MeshRenderer>();
            laserCollider = transform.FindChild("Laser").gameObject.GetComponent<BoxCollider>();
            mirrorSync = GetComponent<spt_mirrorSync>();
        }

        protected override void Update()
        {
            base.Update();

            //Accumulate list of colliders intersecting the mirrors collider
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, .1f);

            //Check each collider
            foreach (Collider col in hitColliders) {
                if (col.gameObject.tag == "laser") {
                    //If this mirror's laser is on, start a count to make sure another mirror is present
                    if (laserCollider.enabled) laserCount++;
                    //If this mirror's laser isn't on, bypass the count so that it will turn on
                    else laserCount = 2;
                }
            }
            //Set emitsLaser to true if the count is at least 2
            bool laserPresent = (laserCount > 1);
            /*if (gameObject.name == "mdl_mirrorPickup (4)")
            {
               if (GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[8].state == false) mirrorSync.emitsLaser = false;
               else mirrorSync.emitsLaser = laserPresent;
            }
            else
            {
                mirrorSync.emitsLaser = laserPresent;
            }*/

            mirrorSync.emitsLaser = laserPresent;
            //Set the mesh and collider's enable state based on emitsLaser
            laserMesh.enabled = mirrorSync.emitsLaser;
            laserCollider.enabled = mirrorSync.emitsLaser;

            //Reset the count and whether or not lasers are touching it
            laserCount = 0;
            mirrorSync.emitsLaser = false;
        }

        override protected void clickSuccess()
        {
            if (!placed) {
                base.clickSuccess();
                transform.parent = GameObject.Find("Objects").transform;
            }
        }
    }
}