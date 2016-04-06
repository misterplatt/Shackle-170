/*
spt_woodenDoor

Author(s): Dara Diba

Revision 1

Handling door jiggle sounds
*/

using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_woodenDoor : spt_baseInteractiveObject
    {
        private static bool jiggled = false;
        private AudioSource doorJiggles;

        override protected void Start()
        {
            doorJiggles = GetComponent<AudioSource>();
        }

        override protected void Update()
        {
            if (jiggled)
            {
                doorJiggles.Play();
                jiggled = false;
            }
        }

        override protected void clickSuccess()
        {
            jiggled = true;
        }

        // Plugging HandleDown
        override protected void HandleDown(){}
    }
}