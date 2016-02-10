using UnityEngine;
using System.Collections;

public class spt_trigger_extCord : spt_BasicPuzzleTrigger
{

    public override bool triggerCheck() {
        return GetComponent<VRStandardAssets.Utils.VRInteractiveItem>().hasBeenTouched;
    }

    public override void setupTrigger() {
        return;
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        if (!isServer) return;

        if (triggerCheck()) {
            GameObject.Find("NetworkPuzzleLogic").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState("extCordPlugged", true, this.gameObject.name);
        }
    }

}