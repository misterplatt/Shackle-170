using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

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
