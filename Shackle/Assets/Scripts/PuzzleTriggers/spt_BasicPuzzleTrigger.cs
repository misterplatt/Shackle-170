using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_BasicPuzzleTrigger : NetworkBehaviour {


    //this should contain the code that evaluates if a trigger needs to take place.
    public virtual bool triggerCheck() { return false; }

    public virtual void setupTrigger() { }

    protected virtual void Start() {
        setupTrigger();
    }

    protected virtual void Update() {
        triggerCheck();
    }


}
