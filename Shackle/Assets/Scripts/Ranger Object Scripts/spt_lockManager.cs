/*
spt_lockManager

Author(s): Hayden Platt

Revision 1

Stores the currentstate of the switches' states
in a bool array.
*/

using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class spt_lockManager : MonoBehaviour
{

    [SerializeField]
    public static int[] dialStates;
    public static int[] correctStates;

    // Use this for initialization
    void Start()
    {
        dialStates = new int[4] { 0, 0, 0, 0 };
        correctStates = new int[4] { 8, 6, 7, 5 };
    }

    //Function to handle input of channel
    //Precon: Button objects named 1-9 exist in the scene
    //Postcon: channelNumber String array is altered
    public void updateDialStates(int index, int state)
    {
        dialStates[index] = state;
        if (dialStates.SequenceEqual(correctStates))
        {
            GameObject.Find("Key Cabinet Door").transform.Translate(new Vector3(0, 2, 0));
            Debug.Log("DIALS ALL CORRECT!$@##@#$");
        }
    }
}