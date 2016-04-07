/*
spt_mirrorStand

Author(s): Hayden Platt, Dara Diba

Revision 1

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
			Debug.Log("hasmirror: " + HasMirror());
			if(!HasMirror()){
				inventorySpt.removeItm("Mirror Pickup");
				GameObject mirrorObj = GameObject.Find("Mirror Pickup");
				mirrorObj.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
				mirrorObj.transform.eulerAngles = new Vector3(0, 0 ,transform.eulerAngles.y);
				mirrorObj.transform.parent = transform.FindChild("Mirror Handle");
                holding = false;
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