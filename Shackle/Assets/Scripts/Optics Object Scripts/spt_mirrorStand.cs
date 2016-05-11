/*
spt_mirrorStand

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 2

Mirror stand script. When a mirror is used on it, it's attached
to the top of the stand, and made a child object. The mirror can be
picked back up.
*/


using UnityEngine;
using UnityEngine.Networking;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_mirrorStand : spt_baseInteractiveObject
    {

        public float mountAngle = 270;
        NetworkTransformChild networkedTransform;

        protected override void Start()
        {
            base.Start();
            NetworkTransformChild[] networkTransforms = GetComponents<NetworkTransformChild>();
            foreach (NetworkTransformChild child in networkTransforms) {
                if (child.target.gameObject.name == "Temp_Transform" || child.target.gameObject.name.Contains("Pickup")) {
                    networkedTransform = child;
                    break;
                }
            }
            //networkedTransform = GetComponent<NetworkTransformChild>();
        }
        override protected void holdSuccess(){            
            //If the stand doesn't already have a mirror child, remove one from your inventory,
            //then child it, set it's position above the stand, and the rotation accordingly
            if (!HasMirror()){
                getLocal().GetComponent<VRStandardAssets.Utils.VREyeRaycaster>().heldSuccess = true;


                GameObject mirrorObj = inventorySpt.retrieveObjectFromInventory(inventorySpt.activeItem);
                inventorySpt.removeItm(mirrorObj.name);

                mirrorObj.transform.position = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
                mirrorObj.GetComponent<spt_mirror>().placed = true;
                foreach (Transform child in this.transform)
                {
                    if (child.gameObject.tag == "mirrorHandle")
                    {
                        mirrorObj.transform.parent = child;
                        break;
                    }
                }
                
                //mirrorObj.transform.parent = //transform.FindChild("mdl_mirrorHandle");
                
                //mirrorObj.transform.eulerAngles = new Vector3(mountAngle, 0, transform.eulerAngles.y);

                holding = false;
                bindMirror(mirrorObj);


                spt_NetworkPuzzleLogic network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
                for (int i = 0; i < network.PuzzleStates.Count; ++i)
                {
                    if (network.PuzzleStates[i].itemName == gameObject.name && network.PuzzleStates[i].isMonsterInteractable)
                    {
                        network.Cmd_UpdatePuzzleLogic(network.PuzzleStates[i].name, true, gameObject.name);
                    }
                }
			}
        }

        GameObject getLocal()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach ( GameObject player in players) 
            {
                NetworkIdentity nid = player.GetComponent<NetworkIdentity>();
                if (nid.isLocalPlayer) return player;
            }

            Debug.Log("Error : spt_mirrorStand.cs getLocal() called but could not locate local player.");
            return null;
        }

        void unbindMirror()
        {
            GameObject tempMirrorRef = networkedTransform.target.gameObject;
            networkedTransform.target = null;
            tempMirrorRef.GetComponent<NetworkIdentity>().enabled = true;
        }

        void bindMirror( GameObject mirror )
        {
            mirror.GetComponent<NetworkIdentity>().enabled = false;
            networkedTransform.target = mirror.transform;
            
        }

        //Plug handleClick
        override protected void HandleClick() { }

		bool HasMirror(){
            GameObject handle = null;

            foreach (Transform child in transform)
            {
                if (child.gameObject.tag == "mirrorHandle")
                {
                    handle = child.gameObject;
                }
            }

            if (handle == null)
            {
                Debug.Log("Error : spt_mirrorStand, hasMirror called but could not locate mirrorHandle");
                return false;
            }

            foreach (Transform child in handle.transform) {
                if (child.gameObject.tag == "mirror") return true;
            }
            return false;
		}
    }
}