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
        NPL = GetComponent<spt_NetworkPuzzleLogic>();
        
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
        if (spt_WorldState.worldStateChanged) {

            //If a player has plugged in the extension cord, command the server to update the state for other player
            if (VRStandardAssets.Examples.spt_extensionCord.extensionCordPlugged) {
                GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("extCordPlugged", true, "Extension_Cord");
            }
            //If a player has pressed power while the extCord is plugged, update server state
            if (VRStandardAssets.Examples.spt_remotePower.TVPowered != PuzzleStates[0].state) {
                GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("tvOn", VRStandardAssets.Examples.spt_remotePower.TVPowered, "TV");
            }
            //If a player has pressed [4],[9], enter on the remote while TV is on, update server state
            if (VRStandardAssets.Examples.spt_remoteEnter.correctChannel)
            {
                GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("correctChannelEntered", true, "TV");
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