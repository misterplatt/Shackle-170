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
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Current Channel: " + channelNumber[0] + " " + channelNumber[1]);
        
	}

    //Function to handle input of channel
    public void enterChannelNumber(String num) {
        if (channelNumber[0] == "-") channelNumber[0] = num;
        else if (channelNumber[1] == "-") channelNumber[1] = num;
        else {
            BroadcastMessage("deactivateButton");
            clearChannelNumber();
        } 
    }

    public static void clearChannelNumber()
    {
        channelNumber[0] = "-";
        channelNumber[1] = "-";
    }
}
