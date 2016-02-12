using UnityEngine;
using System.Collections;

public class spt_trigger_remotePower : spt_BasicPuzzleTrigger
{

    public override bool triggerCheck()
    {
        return GetComponent<VRStandardAssets.Utils.VRInteractiveItem>().hasBeenTouched 
            && GameObject.Find("NetworkPuzzleLogic").GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates[0].state == true;
            //Makes sure extCord is plugged in before letting the TV be powered on, but button may have been pressed hours ago
            //If we can't check the puzzle logic, we are in the same situation
    }

    public override void setupTrigger()
    {
        return;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!isServer) return;

        if (triggerCheck())
        {
            GetComponent<VRStandardAssets.Utils.VRInteractiveItem>().isTriggered = true;
        }
    }

}
