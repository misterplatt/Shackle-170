using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;


using LogicPair = System.Collections.Generic.KeyValuePair<string, bool>;

public class spt_NetworkPuzzleLogic : NetworkBehaviour {

    [SerializeField]
    public Dictionary<string, bool> PuzzleStates = new Dictionary<string, bool>();
    public List<string> devtool_EventList = new List<string>();
    
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.L ) )
        {
            dbg_logEvents();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            transmitLogic();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            PuzzleStates["EventB"] = true;
        }
    }

    //Debug Function to print LogicPairs
    void Start()
    {
        for(int index = 0; index < devtool_EventList.Count; ++index)
        {
            PuzzleStates[devtool_EventList[index]] = false;
        }
    }

    void dbg_logEvents()
    {
       foreach (LogicPair lPair in PuzzleStates)
        {
            Debug.Log("Key : " + lPair.Key + " -> Value : " + lPair.Value );
        }
    }

    [Client]
    void transmitLogic()
    {
        if (isLocalPlayer)
        {
            Debug.Log("Test");
            Cmd_SyncLogic(createLogicMsg());
        }
    }

    [Command]
    void Cmd_SyncLogic( string logMsg )
    {
        Debug.Log("Command Execution");
        translateLogMsg(logMsg);
    }

    //Create a string representing the logic pair key and values as a comma seperated list
    //each key and value is delimited by a colon.
    string createLogicMsg() {
        string logMsg = "";

        foreach(LogicPair lPair in PuzzleStates)
        {
            logMsg += lPair.Key + ":" + lPair.Value + ",";
        }
        Debug.Log(logMsg);
        return logMsg;
    }

    void translateLogMsg( string msg )
    {
        string[] logPairs = msg.Split(new string[] { ",", ":" }, StringSplitOptions.RemoveEmptyEntries);

        for (int index = 0; index < logPairs.Length; index+=2)
        {
            PuzzleStates[logPairs[index]] = logPairs[index + 1] == "True" ? true : false;
        }
    }
}
