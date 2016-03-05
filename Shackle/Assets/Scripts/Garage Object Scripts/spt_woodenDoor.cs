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

        protected override void Update()
        {
            AudioSource doorJiggles = GetComponent<AudioSource>();

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
        protected override void HandleDown()
        {}
    }
}