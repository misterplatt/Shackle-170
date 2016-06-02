/*
spt_TNTLever

Author(s): Hayden Platt, Lauren Cunningham, Dara Diba

Revision 3

When the lever is pressed, temporarily lowers the lever. If both levers are pressed
within a time frame, the level is beaten and the ending cutscene triggers.
Added sound functionality. - Dara
*/


using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_TNTLeverA : spt_baseInteractiveObject
    {
        private AudioSource aSource;
        public AudioClip leverPushSound;
        public AudioClip leverReleasedSound;
        public AudioClip explosion;

        private Light leverLightA;
        private Light leverLightB;

        public static bool local_leverAPressed = false;

        public static bool local_puzzleCompletion = false;

        private bool pressed = false;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
            leverLightA = transform.parent.FindChild("leverLight_A").gameObject.GetComponent<Light>();
            leverLightB = transform.parent.FindChild("leverLight_B").gameObject.GetComponent<Light>();
        }

        protected override void Update()
        {
            base.Update();
            //If client side lever is pressed, enable leverLightB, otherwise disable
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[11].state == true)
            {
                leverLightB.enabled = true;
            }
            else {
                leverLightB.enabled = false;
            }

            //If both levers are pressed, cue win
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[10].state == true && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[11].state == true) {
                aSource.clip = explosion;
                if(!aSource.isPlaying) aSource.Play();
                spt_NetworkPuzzleLogic networkScript = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
                for (int i = 0; i < networkScript.PuzzleStates.Count; i++)
                {
                    if (networkScript.PuzzleStates[i].name == "puzzleCompletionMonster")
                    {
                        networkScript.updatePuzzleState("puzzleCompletionMonster", true, "MonsterStandin");
                        return;
                    }
                }
            }
        }

        override protected void clickSuccess()
        {
            //If the lever is not currently pressed...
            if (!pressed) {
                //Play pressing audio and move lever
                aSource.clip = leverPushSound;
                aSource.Play();
                transform.Translate(Vector3.down * .1f);
                leverLightA.enabled = true;

                //NPL Update
                local_leverAPressed = true;
                spt_WorldState.worldStateChanged = true;

                //Make it so the lever can't be pressed until 2 seconds later
                pressed = true;
                Invoke("raiseLever", 2f);
            }
        }

        //Plug handleDown
        override protected void HandleDown() { }

        //Function which raises the lever 
        void raiseLever() {
            //Play rising audio and move lever
            aSource.clip = leverReleasedSound;
            aSource.Play();
            transform.Translate(Vector3.up * .1f);
            leverLightA.enabled = false;

            //NPL Update
            local_leverAPressed = false;
            spt_WorldState.worldStateChanged = true;

            //Make it so the lever can be pressed again
            pressed = false;
        }
    }
}
