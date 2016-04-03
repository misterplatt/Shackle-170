using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_NetworkSound_Logic : NetworkBehaviour {

    [SyncVar]
    public int wngSoundInd;
    [SyncVar]
    public int ambSoundInd;
    [SyncVar]
    public bool ambReady;
    [SyncVar]
    public bool wngReady;

    public void setWngSound(int min, int max)
    {
        wngSoundInd = Random.Range(min, max);
    }

    public void setAmbSound(int min, int max)
    {
        ambSoundInd = Random.Range(min, max);
    }

    public void clearInd() {
        wngSoundInd = -1;
        ambSoundInd = -1;
        ambReady = false;
        wngReady = false;
    }


    // Use this for initialization
    void Start () {
        //set intiial state to false
        clearInd();
    }	
    
	// Update is called once per frame
	void Update () {
        if (wngSoundInd != -1) wngReady = true;
        if (ambSoundInd != -1) ambReady = true;


	}
}
