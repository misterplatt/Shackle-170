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

    public LogicTuple(string _name="", bool _state=false, string _itemName="") { state = _state; name = _name; itemName = _itemName; }

    public override bool Equals(System.Object obj) {
        if (obj == null) return false;

        //attempt cast
        LogicTuple cValue = (LogicTuple)obj;

        //Struct is non-nullable value, so no null check required.
        return this.name.Equals(cValue.name);
    }
}

[Serializable]
public struct dev_LogicPair
{
    [SerializeField]
    public string eventName;
    [SerializeField]
    public GameObject item;

    public dev_LogicPair( string _eventName) {
        this.eventName = _eventName;
        this.item = null;
    }

}
//required for network code generation
public class SyncListLogicPair : SyncListStruct<LogicTuple> { }

public class spt_NetworkPuzzleLogic : NetworkBehaviour {
    [SerializeField]
    public SyncListLogicPair PuzzleStates = new SyncListLogicPair();
    public List<dev_LogicPair> devtool_PuzzleStates = new List<dev_LogicPair>();

    void Start() {
        if (!isServer) return;
        for (int index = 0; index < devtool_PuzzleStates.Count; ++index) {
            PuzzleStates.Add( new LogicTuple( devtool_PuzzleStates[index].eventName, false, devtool_PuzzleStates[index].item.name ));
        }
    }

    void Update() {
        //if (!isServer) return;

        if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("Test");
            dbg_logEvents();
        }
        
        if (Input.GetKeyDown(KeyCode.J)) {
            if (!isServer) return;
            PuzzleStates[ PuzzleStates.IndexOf( new LogicTuple("EventB", false, "EventB_itm") ) ] = new LogicTuple("EventB", true, "EventB_itm");
        }
    }

    void dbg_logEvents() {
        Debug.Log("SyncList Size : " + PuzzleStates.Count);
        foreach (LogicTuple lPair in PuzzleStates) {
            Debug.Log("PuzzleEvent Name : " + lPair.name + " -> State : " + lPair.state + " Controlled by : " + lPair.itemName);
        }
    }
    
    public void updatePuzzleState( string name, bool state, string itemName) {
        PuzzleStates[PuzzleStates.IndexOf(new LogicTuple(name, false, itemName))] = new LogicTuple(name, state, itemName);
    }
}