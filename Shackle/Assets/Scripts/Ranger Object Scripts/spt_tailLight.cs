/*
spt_tailLight

Author(s): Hayden Platt, Dara Diba

Revision 2

Script which resides on each remote button. When pressed,
the name of the gameObject channelNumber string array in remote manager.
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    //Script that handles player interaction with digit buttons on the remote
    public class spt_tailLight : spt_baseInteractiveObject
    {

        public Sprite fullDiagram;

        //Handle the Click event, alternates states on every press
        override protected void holdSuccess()
        {
            GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("Fuse Diagram").GetComponent<SpriteRenderer>().sprite = fullDiagram;
        }

        //Plug HandleClick
        override protected void HandleClick() { }
    }
}
