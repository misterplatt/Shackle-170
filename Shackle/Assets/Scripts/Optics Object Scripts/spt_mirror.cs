/*
spt_mirror

Author(s): Hayden Platt, Dara Diba

Revision 2

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
        private bool laserTouching = false;
        private MeshRenderer laserMesh;
        private BoxCollider laserCollider;

        protected override void Start()
        {
            base.Start();
            laserMesh = transform.FindChild("Laser").gameObject.GetComponent<MeshRenderer>();
            laserCollider = transform.FindChild("Laser").gameObject.GetComponent<BoxCollider>();
        }

        protected override void Update()
        {
            base.Update();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
            foreach (Collider col in hitColliders) {
                if (col.gameObject.tag == "laser") laserTouching = true;
                else laserTouching = false;
            }
            laserMesh.enabled = laserTouching;
            laserCollider.enabled = laserTouching;

        }

        override protected void clickSuccess()
        {
            base.clickSuccess();
            transform.parent = GameObject.Find("Objects").transform;
        }
    }
}

