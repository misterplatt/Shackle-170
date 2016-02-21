/*
spt_remoteManager

Author(s): Hayden Platt, Dara Diba

Revision 1

Stores the current channel number as entered by the user
in a string array.
*/

using UnityEngine;
using System.Collections;
using System;

public class spt_remoteManager : MonoBehaviour {

    [SerializeField]
    public static String[] channelNumber;

    // Use this for initialization
    void Start () {
        channelNumber = new String[2] {"-", "-"};
	}

    //Function to handle input of channel
    //Precon: Button objects named 1-9 exist in the scene
    //Postcon: channelNumber String array is altered
    public void enterChannelNumber(String num) {
        if (channelNumber[0] == "-") channelNumber[0] = num;
        else if (channelNumber[1] == "-") channelNumber[1] = num;
        else {
            BroadcastMessage("deactivateDigit");
            clearChannelNumber();
        } 
    }

    //Quick function to clear the current channel number, called by remoteEnter
    //Postcon: channelNumber is set to ["-", "-"]
    public static void clearChannelNumber()
    {
        channelNumber[0] = "-";
        channelNumber[1] = "-";
    }
}
