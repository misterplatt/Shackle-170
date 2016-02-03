using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;


//using LogicPair = System.Collections.Generic.KeyValuePair<string, bool>;


public struct LogicPair
{ 
    public bool state;
    public string name;

    public LogicPair(string _name="", bool _state=false) { state = _state; name = _name; }

    public override bool Equals(System.Object obj) {
        if (obj == null) return false;

        //attempt cast
        LogicPair cValue = (LogicPair)obj;

        //Struct is non-nullable value, so no null check required.
        return this.name.Equals(cValue.name);
    }
}

//required for network code generation
public class SyncListLogicPair : SyncListStruct<LogicPair> { }

public class spt_NetworkPuzzleLogic : NetworkBehaviour {
    [SerializeField]
    public SyncListLogicPair PuzzleStates = new SyncListLogicPair();
    public List<string> devtool_PuzzleStates = new List<string>();

    void Start() {
        if (!isServer) return;
        for (int index = 0; index < devtool_PuzzleStates.Count; ++index) {
            PuzzleStates.Add( new LogicPair( devtool_PuzzleStates[index], false ));
        }
    }

    void Update() {
        //if (!isServer) return;

        if (Input.GetKeyDown(KeyCode.L)) {
            dbg_logEvents();
        }
        
        if (Input.GetKeyDown(KeyCode.J)) {
            if (!isServer) return;
            PuzzleStates[ PuzzleStates.IndexOf( new LogicPair("EventB", false) ) ] = new LogicPair("EventB", true);
        }
    }

    void dbg_logEvents() {
        Debug.Log("SyncList Size : " + PuzzleStates.Count);
        foreach (LogicPair lPair in PuzzleStates) {
            Debug.Log("PuzzleEvent Name : " + lPair.name + " -> State : " + lPair.state);
        }
    }
    
    public void updatePuzzleState( string name, bool state) {
        PuzzleStates[PuzzleStates.IndexOf(new LogicPair(name, false))] = new LogicPair(name, state);
    }
}
