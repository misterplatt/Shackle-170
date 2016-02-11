using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public struct LogicTuple
{ 
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

    void Start() {

        List<dev_LogicPair> devtool_PuzzleStates = GameObject.Find("PuzzleStates").GetComponent<spt_Events>().devtool_PuzzleStates;
        
        
        for (int index = 0; index < devtool_PuzzleStates.Count; ++index) {
            PuzzleStates.Add(new LogicTuple(devtool_PuzzleStates[index].eventName,
                                                false,
                                                devtool_PuzzleStates[index].item.name,
                                                devtool_PuzzleStates[index].isMonstInteractable
            ));



        }
        
    }

    void Update() {
        //if (!isServer) return;

        dbg_logEvents();
        if (spt_WorldState.worldStateChanged) {
            
            if (VRStandardAssets.Examples.spt_extensionCord.extCodePlugged) {
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("extCordPlugged", true, "Extension_Cord");
            }
        

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