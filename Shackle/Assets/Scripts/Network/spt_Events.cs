/* spt_Events.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This file provides an editor interface with which designers can implement puzzle logic without need for comprehension of the underlying systems.
 */


using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

/*Dev LogicPair is a structure that stores information about a particular puzzle state on the game.
    eventName : the name of the event
    item : the gameObject that controls this puzzle state
    isMonsterInteractable : can the monster change the state of this puzzle.

    these structures are stored in a List which is in turn retrieved by Networking code during level initialization for state
    synchronization across host and client.
*/
[Serializable]
public struct dev_LogicPair
{
    [SerializeField]
    public string eventName;
    [SerializeField]
    public GameObject item;
    [SerializeField]
    public bool isMonstInteractable;
    [SerializeField]

    //Constructor for use by editor
    public dev_LogicPair(string _eventName) {
        this.eventName = _eventName;
        this.item = null;
        this.isMonstInteractable = false;
    }
}

public class spt_Events : NetworkBehaviour {

    public List<dev_LogicPair> devtool_PuzzleStates = new List<dev_LogicPair>();
    // Use this for initialization
}
