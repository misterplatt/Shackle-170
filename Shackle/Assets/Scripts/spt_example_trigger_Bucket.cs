using UnityEngine;
using System.Collections;

public class spt_example_trigger_Bucket : spt_BasicPuzzleTrigger {
    Vector3 lastPosition;

    public override void setupTrigger() {
        lastPosition = this.transform.position;    
    }

    override public bool triggerCheck() {
        if (this.transform.position != lastPosition) return true;
        return false;
    }
}
