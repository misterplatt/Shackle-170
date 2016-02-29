/*
spt_fuseManager

Author(s): Hayden Platt

Revision 1

Stores the currentstate of the switches' states
in a bool array.
*/

using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class spt_fuseManager : MonoBehaviour
{

    [SerializeField]
    public static bool[] fuseStates;
    public static bool[] correctStates;

    // Use this for initialization
    void Start()
    {
        fuseStates = new bool[6] { false, false, false, false, false, false};
        correctStates = new bool[6] { true, true, false, false, true, true };
    }

    //Function to handle input of channel
    //Precon: Button objects named 1-9 exist in the scene
    //Postcon: channelNumber String array is altered
    public void updateFuseStates(int index, bool state)
    {
        fuseStates[index] = state;
        if (fuseStates.SequenceEqual(correctStates)) {
            GameObject.Find("Electronic Lock").transform.Translate(new Vector3(.3f,0,0));
            Debug.Log("CORRECT SWITCHES ON!$@##@#$");
        }
    }
}

