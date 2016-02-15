using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public struct LogicTuple
{ 
    //Stores state (event has occured or not)
    // name of event
    // name of item whose interaction controls the state
    // isMonsterInteractable is monster can fux wit dat yo.
    public bool state;
    public string name;
    public string itemName;
    public bool isMonsterInteractable;

    public LogicTuple(string _name="", bool _state=false, string _itemName="", bool interactable=false) {
        state = _state;
        name = _name;
        itemName = _itemName;
        isMonsterInteractable = interactable;
    }

    public override bool Equals(System.Object obj) {
        if (obj == null) return false;

        //attempt cast
        LogicTuple cValue = (LogicTuple)obj;

        //Struct is non-nullable value, so no null check required.
        return this.name.Equals(cValue.name);
    }
}

//required for network code generation
public class SyncListLogicPair : SyncListStruct<LogicTuple> { }

public class spt_NetworkPuzzleLogic : NetworkBehaviour {
    [SerializeField]
    public SyncListLogicPair PuzzleStates = new SyncListLogicPair();
    public spt_player_NetworkPuzzleLogic player;
    private spt_NetworkPuzzleLogic NPL;
    public bool loaded = false;
    void Start() {

        List<dev_LogicPair> devtool_PuzzleStates = GameObject.Find("PuzzleStates").GetComponent<spt_Events>().devtool_PuzzleStates;
        //NPL = GetComponent<spt_NetworkPuzzleLogic>();
        Debug.Log(devtool_PuzzleStates.Count);
        for (int index = 0; index < devtool_PuzzleStates.Count; ++index) {
            PuzzleStates.Add(new LogicTuple(devtool_PuzzleStates[index].eventName,
                                    false,
                                    devtool_PuzzleStates[index].item.name,
                                    devtool_PuzzleStates[index].isMonstInteractable
            ));
        }
        this.loaded = true;
    }

    void Update() {
        //if (!isServer) return;

        //dbg_logEvents();
        if (Input.GetKeyDown(KeyCode.B)) dbg_logEvents();
        //If a state has been changed locally, find out which one and update the state's networked version
        if (spt_WorldState.worldStateChanged) {

            //If a player has plugged in the extension cord, command the server to update the state for other player
            if (VRStandardAssets.Examples.spt_extensionCord.local_extCordPlugged) {
                Debug.Log("Updating extCordPlugged on the network to " + VRStandardAssets.Examples.spt_extensionCord.local_extCordPlugged);
                GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("extCordPlugged", true, "Extension_Cord");
                Debug.Log("extCordPlugged on the network is now " + PuzzleStates[0].state);
            }
            //If a player has pressed power while the extCord is plugged, update server state
            if (VRStandardAssets.Examples.spt_remotePower.local_TVpowerState != PuzzleStates[2].state) {
                Debug.Log("Updating TVOn on the network to " + VRStandardAssets.Examples.spt_remotePower.local_TVpowerState);
                GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("TVpowerState", VRStandardAssets.Examples.spt_remotePower.local_TVpowerState, "mdl_TV");
            }
            //If a player has pressed [4],[9], enter on the remote while TV is on, update server state
            if (VRStandardAssets.Examples.spt_remoteEnter.local_correctChannelEntered)
            {
                Debug.Log("Updating correctChannelEntered on the network to " + VRStandardAssets.Examples.spt_remoteEnter.local_correctChannelEntered);
                GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("correctChannelEntered", true, "mdl_TV");
            }
            //If a player has used the garage door opener on the garage door, update server state
            if (VRStandardAssets.Examples.spt_garageDoor.local_garageDoorOpen)
            {
                Debug.Log("Updating garageDoorOpen on the network to " + VRStandardAssets.Examples.spt_garageDoor.local_garageDoorOpen);
                GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("garageDoorOpen", true, "mdl_garageDoor");
            }
            

            spt_WorldState.worldStateChanged = false;

        }
    }

    void dbg_logEvents() {
        Debug.Log("SyncList Size : " + PuzzleStates.Count);
        foreach (LogicTuple lPair in PuzzleStates) {
            Debug.Log("PuzzleEvent Name : " + lPair.name + " -> State : " + lPair.state + " Controlled by : " + lPair.itemName);
        }
    }
    
    //update puzzle tuple with correct values. Must create new tuple due to structure sync environment
    //cannot simply update current tuple.
    public void updatePuzzleState( string name, bool state, string itemName) {
        int tIndex = PuzzleStates.IndexOf(new LogicTuple(name, false, itemName));

        if (tIndex < 0) Debug.Log("Error : updatePuzzleState called with nonexistent puzzle event.");

        LogicTuple original_Tuple = PuzzleStates[PuzzleStates.IndexOf(new LogicTuple(name, false, itemName))];
        LogicTuple newTuple = new LogicTuple(original_Tuple.name, true, original_Tuple.itemName, original_Tuple.isMonsterInteractable);
        PuzzleStates[PuzzleStates.IndexOf(new LogicTuple(name, false, itemName))] = newTuple;
    }

    [Command]
    public void Cmd_UpdatePuzzleLogic(string name, bool state, string itmName) {
        spt_NetworkPuzzleLogic logScript = GetComponent<spt_NetworkPuzzleLogic>();
        logScript.updatePuzzleState(name, state, itmName);
    }
}