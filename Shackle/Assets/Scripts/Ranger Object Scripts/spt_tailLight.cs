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

        public Material fullDiagram;
        private AudioSource aSource;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        //Handle the Click event, breaking the tailLight and increasing the light range
        override protected void holdSuccess()
        {
            aSource.Play();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Light>().range = 10;
            GameObject.Find("tex_fuseDiagram").GetComponent<MeshRenderer>().material = fullDiagram;
            Destroy(GameObject.Find("mdl_jeepLights"));
            holding = false;
        }

        //Plug HandleClick
        override protected void HandleClick() { }
    }
}
