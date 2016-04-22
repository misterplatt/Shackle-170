/*
spt_mirrorStand

Author(s): Hayden Platt, Dara Diba, Lauren Cunningham

Revision 2

Mirror stand script. When a mirror is used on it, it's attached
to the top of the stand, and made a child object. The mirror can be
picked back up.
*/


using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_mirrorStand : spt_baseInteractiveObject
    {

        override protected void holdSuccess(){
            //If the stand doesn't already have a mirror child, remove one from your inventory,
            //then child it, set it's position above the stand, and the rotation accordingly
            if (!HasMirror()){
                GameObject mirrorObj = inventorySpt.retrieveObjectFromInventory(inventorySpt.activeItem);
                inventorySpt.removeItm(mirrorObj.name);
                mirrorObj.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                mirrorObj.transform.parent = transform.FindChild("Mirror Handle");
                mirrorObj.transform.eulerAngles = new Vector3(270, 0, transform.eulerAngles.y);
                holding = false;
                
                spt_NetworkPuzzleLogic network = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
                for (int i = 0; i < network.PuzzleStates.Count; ++i)
                {
                    if (network.PuzzleStates[i].itemName == gameObject.name && network.PuzzleStates[i].isMonsterInteractable)
                    {
                        network.updatePuzzleState(network.PuzzleStates[i].name, true, gameObject.name);
                    }
                }
			}
        }

        //Plug handleDown
        override protected void HandleClick() { }

		bool HasMirror(){
			if (transform.FindChild ("Mirror Handle/Mirror Pickup") != null) return true;
			else return false;
		}
    }
}