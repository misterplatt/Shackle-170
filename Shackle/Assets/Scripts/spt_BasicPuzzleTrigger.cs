using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_BasicPuzzleTrigger : NetworkBehaviour {
    public string eventName;
    public bool wasTriggered;


    //this should contain the code that evaluates if a trigger needs to take place.
    public virtual bool triggerCheck() { return false; }

    public virtual void setupTrigger() { }

    void Start() {
        wasTriggered = false;
        setupTrigger();
    }

    public void Update() {
        if (wasTriggered) return;

        if (triggerCheck() ) {
            Debug.Log("Triggered");
            if (isServer) {
                GameObject.Find("NetworkPuzzleLogic").GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState(eventName, true, this.name);
            }
            else {
                Debug.Log("Meow");
               Cmd_UpdatePuzzleLogic(eventName, true, this.name);
            }
        }
    }

    [Command]
    public void Cmd_UpdatePuzzleLogic(string name, bool state, string itmName) {
        GameObject pLogic = NetworkServer.FindLocalObject(GameObject.Find("PuzzleLogic").GetComponent<NetworkIdentity>().netId);
        spt_NetworkPuzzleLogic logScript = pLogic.GetComponent<spt_NetworkPuzzleLogic>();
        logScript.updatePuzzleState(name, state, itmName);
    }

}
