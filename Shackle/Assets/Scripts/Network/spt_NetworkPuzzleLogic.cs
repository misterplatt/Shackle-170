/* spt_Events.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This file provides the logic for puzzle states in a Network Logic Environment.
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;


//LogicTuple is a structure that stores data for Network Code Generation
// bool state : a boolean state indicating if this stage of the puzzle has been completed
// string name : the name of the stage
// itemName : the name referring to the GameObject which controls this state
// isMonsterInteractable : bool noting if the monster can alter this state.
// timeStamp : a string which stores the time this stage was completed for metric collection
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
    public float timeStamp;

    public LogicTuple(string _name="", bool _state=false, string _itemName="", bool interactable=false, float completionTime=-1.0f) {
        state = _state;
        name = _name;
        itemName = _itemName;
        isMonsterInteractable = interactable;
        timeStamp = completionTime;
    }
    //when checking for equality, just check the name of the state, that's all we care about
    public override bool Equals(System.Object obj) {
        if (obj == null) return false;

        //attempt cast
        LogicTuple cValue = (LogicTuple)obj;

        //Struct is non-nullable value, so no null check required.
        return this.name.Equals(cValue.name);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}

//required for network code generation
public class SyncListLogicPair : SyncListStruct<LogicTuple> { }

public class spt_NetworkPuzzleLogic : NetworkBehaviour {
    [SyncVar]
    public SyncListLogicPair PuzzleStates = new SyncListLogicPair();
    private spt_NetworkPuzzleLogic NPL;
    public bool loaded = false;

    void Start() {
        if (!isServer) return;
        List<dev_LogicPair> devtool_PuzzleStates = GameObject.Find("PuzzleStates").GetComponent<spt_Events>().devtool_PuzzleStates;

        for (int index = 0; index < devtool_PuzzleStates.Count; ++index) {
            
            PuzzleStates.Add(new LogicTuple(devtool_PuzzleStates[index].eventName,
                                    false,
                                    devtool_PuzzleStates[index].item.name,
                                    devtool_PuzzleStates[index].isMonstInteractable
            ));
        }
        this.loaded = true;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        //if (SceneManager.GetActiveScene().name == "LobbyManager") return;

        if (Input.GetKeyDown(KeyCode.N)) GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("keyFobPressed", true, "mdl_carKeyfob");
        GameObject monster = GameObject.FindWithTag("monster");
        if (monster == null) return;
        spt_monsterMovement mover = monster.GetComponent<spt_monsterMovement>();

        if (mover.pLoss)
        {
            GameObject uiMessager = transform.Find("Camera Player/VRCameraUI/WinMessage").gameObject;
            uiMessager.GetComponent<Text>().text = "You lose";
            GetComponent<spt_lossListener>().loss = true;
            uiMessager.GetComponent<Text>().enabled = true;

            //path to return to main menu.



            return;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                foreach (LogicTuple lPair in player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates)
                {
                    Debug.Log(player.name + " : PuzzleEvent Name : " + lPair.name + " -> State : " + lPair.state + " Controlled by : " + lPair.itemName);
                }
                //player.GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState(name, state, itmName);
            }
        }
        //If a state has been changed locally, find out which one and update the state's networked version
        Debug.Log("worldStateChanged : " + spt_WorldState.worldStateChanged);
        if (spt_WorldState.worldStateChanged)
        {

            //If garage is loaded, check the following puzzle states
            if (SceneManager.GetActiveScene().name == "net_SpookyGarage")
            {
                //If a player has used the garage door opener on the garage door, update server state
                if (VRStandardAssets.Examples.spt_garageDoor.local_puzzleCompletion)
                {
                    Debug.Log("Updating garageDoorOpen on the network to " + VRStandardAssets.Examples.spt_garageDoor.local_puzzleCompletion);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("puzzleCompletion", true, "mdl_garageDoor");
                }
                //If a player has pressed power while the extCord is plugged, update server state
                if (VRStandardAssets.Examples.spt_remotePower.local_TVpowerState != PuzzleStates[2].state)
                {
                    Debug.Log("Updating TVpowerState on the network to " + VRStandardAssets.Examples.spt_remotePower.local_TVpowerState);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("TVpowerState", VRStandardAssets.Examples.spt_remotePower.local_TVpowerState, "mdl_TV");
                }
                //If a player has plugged in the extension cord, command the server to update the state for other player
                if (VRStandardAssets.Examples.spt_extensionCord.local_extCordPlugged)
                {
                    Debug.Log("Updating extCordPlugged on the network to " + VRStandardAssets.Examples.spt_extensionCord.local_extCordPlugged);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("extCordPlugged", true, "Extension_Cord");
                }
                //If a player has pressed [4],[9], enter on the remote while TV is on, update server state
                if (VRStandardAssets.Examples.spt_remoteEnter.local_correctChannelEntered)
                {
                    Debug.Log("Updating correctChannelEntered on the network to " + VRStandardAssets.Examples.spt_remoteEnter.local_correctChannelEntered);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("correctChannelEntered", true, "mdl_TV");
                }
                //If a player has used the garage door opener on the garage door, update server state
                if (VRStandardAssets.Examples.spt_garageLock.local_garageDoorUnlocked)
                {
                    Debug.Log("Updating garageDoorOpen on the network to " + VRStandardAssets.Examples.spt_garageLock.local_garageDoorUnlocked);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("garageDoorUnlocked", true, "mdl_garageLock");
                }
                //If a player uses the screwdriver on the outlet, update server state
                if (VRStandardAssets.Examples.spt_outletKill.local_playerDeath) {
                    Debug.Log("Updating playerLoss on the network to " + VRStandardAssets.Examples.spt_outletKill.local_playerDeath);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("playerLoss", true, "MonsterStandin");
                }
                //If a player has used the garage door opener on the garage door, update server state
                if (VRStandardAssets.Examples.spt_garageDoor.local_puzzleCompletionMonster)
                {
                    Debug.Log("Updating garageDoorOpen on the network to " + VRStandardAssets.Examples.spt_garageDoor.local_puzzleCompletionMonster);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("puzzleCompletionMonster", true, "MonsterStandin");
                }
            }

            //If outpost is loaded, check the following puzzle states
            if (SceneManager.GetActiveScene().name == "net_RangerOutpost")
            {
                //If a player has successfully used the pickaxe on the hatch, update server state
                if (VRStandardAssets.Examples.spt_hatch.local_puzzleCompletion)
                {
                    Debug.Log("Updating puzzleCompletion on the network to " + VRStandardAssets.Examples.spt_hatch.local_puzzleCompletion);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("puzzleCompletion", true, "mdl_hatchTop");
                }
                //If a player has flipped the fuses correctly, update server state
                if (VRStandardAssets.Examples.spt_fuseManager.local_correctFuseCombo)
                {
                    Debug.Log("Updating correctFuseCombo on the network to " + VRStandardAssets.Examples.spt_fuseManager.local_correctFuseCombo);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("correctFuseCombo", true, "grp_fuseBox");
                }
                //If a player has pressed the key fob, update server state
                if (VRStandardAssets.Examples.spt_fobButton.local_keyFobPressed)
                {
                    Debug.Log("Updating keyFobPressed on the network to " + VRStandardAssets.Examples.spt_fobButton.local_keyFobPressed);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("keyFobPressed", true, "mdl_carKeyfob");
                }
                //If a player has used the garage door opener on the garage door, update server state
                if (VRStandardAssets.Examples.spt_hatch.local_puzzleCompletionMonster)
                {
                    Debug.Log("Updating garageDoorOpen on the network to " + VRStandardAssets.Examples.spt_hatch.local_puzzleCompletionMonster);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("puzzleCompletionMonster", true, "MonsterStandin");
                }
            }

            //If optics lab is loaded, check the following puzzle states
            if (SceneManager.GetActiveScene().name == "net_OpticsLab")
            {
                //If both levers are pressed at the same time, update server state
                if (VRStandardAssets.Examples.spt_TNTLever.local_puzzleCompletion)
                {
                    Debug.Log("Updating puzzleCompletion on the network to " + VRStandardAssets.Examples.spt_TNTLever.local_puzzleCompletion);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("puzzleCompletion", true, "MonsterStandin");
                }
                //If a laser has intersected with the chest lock, update server state
                if (VRStandardAssets.Examples.spt_chestListener.local_laserHitLock)
                {
                    Debug.Log("Updating laserHitLock on the network to " + VRStandardAssets.Examples.spt_chestListener.local_laserHitLock);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("laserHitLock", true, "mdl_chestLock");
                    spt_WorldState.worldStateChanged = false;
                    return;
                }
                //If a laser has intersected with the security panel, update server state
                if (VRStandardAssets.Examples.spt_panelListener.local_laserHitPanel)
                {
                    Debug.Log("Updating laserHitPanel on the network to " + VRStandardAssets.Examples.spt_panelListener.local_laserHitPanel);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("laserHitPanel", true, "Security Panel");
                }
                //If the player turns on or off the laser machine, update server state
                if (VRStandardAssets.Examples.spt_laserSwitch.local_isLaserOn != PuzzleStates[4].state)
                {
                    Debug.Log("Updating isLaserOn on the network to " + VRStandardAssets.Examples.spt_laserSwitch.local_isLaserOn);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("isLaserOn", VRStandardAssets.Examples.spt_laserSwitch.local_isLaserOn, "Joystick_base");
                }
                //If the player turns on or off the laser machine, update server state
                if (VRStandardAssets.Examples.spt_keypad.local_doorOpen)
                {
                    Debug.Log("Updating doorOpen on the network to " + VRStandardAssets.Examples.spt_keypad.local_doorOpen);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("doorOpen", true, "mdl_keypad+doors");
                }
                //If player A has pressed their TNT lever, update server state. State reverts after lever has risen after x seconds.
                if (VRStandardAssets.Examples.spt_TNTLever.local_leverAPressed != PuzzleStates[10].state)
                {
                    Debug.Log("Updating leverAPressed on the network to " + VRStandardAssets.Examples.spt_TNTLever.local_leverAPressed);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("leverAPressed", VRStandardAssets.Examples.spt_TNTLever.local_leverAPressed, "mdl_TNTLeverA");
                }
                //If player B has pressed their TNT lever, update server state. State reverts after lever has risen after x seconds.
                if (VRStandardAssets.Examples.spt_TNTLever.local_leverBPressed != PuzzleStates[11].state)
                {
                    Debug.Log("Updating leverBPressed on the network to " + VRStandardAssets.Examples.spt_TNTLever.local_leverBPressed);
                    GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("leverBPressed", VRStandardAssets.Examples.spt_TNTLever.local_leverBPressed, "mdl_TNTLeverB");
                }
            }

            spt_WorldState.worldStateChanged = false;
        }
    }

    //when we destroy the object, output all the metrics
    void OnDestroy() {
        outputMetrics();
    }

    //debug function which logs all events in console
    void dbg_logEvents() {
        if (!isLocalPlayer) return;
        Debug.Log("SyncList Size : " + PuzzleStates.Count);
        foreach (LogicTuple lPair in PuzzleStates) {
            Debug.Log("PuzzleEvent Name : " + lPair.name + " -> State : " + lPair.state + " Controlled by : " + lPair.itemName);
        }
    }
    
    //update puzzle tuple with correct values. Must create new tuple due to structure sync environment
    //cannot simply update current tuple.
    public void updatePuzzleState( string name, bool state, string itemName) {
        int tIndex = PuzzleStates.IndexOf(new LogicTuple(name, false, itemName));
        Debug.Log("UPL ServerLocal :  Called with : " + name + " | " + state);
        if (tIndex < 0)
        {            
            Debug.Log("Error : updatePuzzleState called with nonexistent puzzle event : " + name);
            foreach (LogicTuple lT in PuzzleStates) { Debug.Log(lT.name); }

        }
        LogicTuple original_Tuple = PuzzleStates[tIndex];
        LogicTuple newTuple = new LogicTuple(original_Tuple.name, state, original_Tuple.itemName, original_Tuple.isMonsterInteractable, Time.time);
        PuzzleStates[tIndex] = newTuple;
        PuzzleStates.Dirty(tIndex);
    }

    //step through synclist and grab name and timestamps, save to datadump so we can retrieve metrics later
    public void outputMetrics() {
        if (!isServer) return;

        StreamWriter writer = new StreamWriter("DataDump/" + SceneManager.GetActiveScene().name + "_metrics.txt", true);
        String metricLn = "";

        foreach (LogicTuple pEvent in PuzzleStates) {
            metricLn += pEvent.name + "," + pEvent.timeStamp;
            writer.WriteLine(metricLn);
            metricLn = "";
        }

        writer.Close();
    }

    //server command to update a puzzle state to some value.
    [Command]
    public void Cmd_UpdatePuzzleLogic(string name, bool state, string itmName) {
        Debug.Log("UPL Called with : " + name + " | " + state);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Debug.Log(player.name + " : " + player.GetComponent<spt_NetworkPuzzleLogic>().PuzzleStates.Count);
            player.GetComponent<spt_NetworkPuzzleLogic>().updatePuzzleState(name, state, itmName);
        }
        //updatePuzzleState(name, state, itmName);
    }
}