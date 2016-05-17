/*
spt_lockManager

Author(s): Hayden Platt

Revision 2

Stores the currentstate of the switches' states
in a bool array.
Changed line 47 to work for the new combo lock model - Dara
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

    private AudioSource aSource;


    // Use this for initialization
    void Start()
    {
        aSource = GetComponent<AudioSource>();
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
            aSource.Play();
            transform.FindChild("mdl_CL_metal").Translate(Vector3.up * 0.1F);
            //transform.FindChild("mdl_arch").Translate(Vector3.up * 0.1f);
            GameObject.Find("mdl_cabinetDoor").GetComponent<Rigidbody>().useGravity = true;

            Invoke("returnLock", .3f);

            GameObject.FindWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>().Cmd_UpdatePuzzleLogic("keyDoorThrowable", true, "mdl_cabinetDoor");
        }
    }

    //Return lock after correct combo
    void returnLock() {
        GetComponent<VRStandardAssets.Examples.spt_interactiveItemManipulate>().currentState = false;
    }
}