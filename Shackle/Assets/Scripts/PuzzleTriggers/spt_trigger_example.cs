using UnityEngine;
using System.Collections;

public class spt_trigger_example : spt_BasicPuzzleTrigger
{

    
    public override bool triggerCheck() {
        //check custom trigger logic here
        return base.triggerCheck();

    }

    public override void setupTrigger() {
        //put custom trigger logic here.
        base.setupTrigger();
    }

    protected override void Start() {
        //ensure that child uses parent functions for start and update
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

}
