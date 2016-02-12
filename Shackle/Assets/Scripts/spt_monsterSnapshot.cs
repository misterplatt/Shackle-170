using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class spt_monsterSnapshot : MonoBehaviour {

    private spt_monsterMovement movementScript;
    private spt_monsterMotivation motivationScript;
    
    // The writer for the data dump and the amount of elapsed time (in seconds) of the current playthrough.
    private StreamWriter writer;
    private int elapsedTime = 0;

    // Use this for initialization
	void Start () {
        movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
        motivationScript = GameObject.FindObjectOfType(typeof(spt_monsterMotivation)) as spt_monsterMotivation;

        writer = new StreamWriter("DataDump/aiSnapshotDataDump.txt");
        writer.WriteLine("Datetime,Elapsed Time (in Seconds),Current Waypoint,Anger Level");
        InvokeRepeating("snapshot", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void snapshot()
    {
        elapsedTime = elapsedTime + 1;
        writer.WriteLine(DateTime.Now + "," + elapsedTime + "," + movementScript.getWaypoint() + "," + motivationScript.getAnger());
    }

    void OnApplicationQuit()
    {
        writer.Close();
    }
}
