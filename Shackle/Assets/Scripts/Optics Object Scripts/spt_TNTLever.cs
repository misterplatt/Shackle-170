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
    public class spt_TNTLever : spt_baseInteractiveObject
    {
        private AudioSource aSource;
        public AudioClip leverPushSound;
        public AudioClip leverReleasedSound;
        public static bool local_leverAPressed = false;
        public static bool local_leverBPressed = false;

        public static bool local_puzzleCompletion = false;

        private bool pressed = false;

        protected override void Start()
        {
            aSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            base.Update();
            if (GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[10].state == true && GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[11].state == true) {
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
            Debug.Log("PRESSING" + pressed);
            if (!pressed) {
                aSource.clip = leverPushSound;
                aSource.Play();
                transform.Translate(Vector3.down * .1f);
                Debug.Log("I AM " + transform.name + ". HEAR ME ROAR");
                if (transform.name == "leverA") local_leverAPressed = true;
                if (transform.name == "leverB") local_leverBPressed = true;
                spt_WorldState.worldStateChanged = true;
                pressed = true;
                Invoke("raiseLever", 1.5f);
            }
        }

        //Plug handleDown
        override protected void HandleDown() { }

        //Function which raises the lever 
        void raiseLever() {
            aSource.clip = leverReleasedSound;
            aSource.Play();
            transform.Translate(Vector3.up * .1f);
            Debug.Log("I AM " + transform.name + ". HEAR ME ROAR");
            if (transform.name == "leverA") local_leverAPressed = false;
            if (transform.name == "leverB") local_leverBPressed = false;
            spt_WorldState.worldStateChanged = true;
            pressed = false;
        }
    }
}
