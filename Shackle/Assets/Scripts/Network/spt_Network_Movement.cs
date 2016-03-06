using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_Network_Movement : NetworkBehaviour {

    [SyncVar]
    public float lStickInput;

    public static float THRESHOLD = 0.10f;
    public float lastCli_lStick = 0.0f;

    void Start()
    {
        lStickInput = 0.0F;
    }
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if (!isServer && (Mathf.Abs(spt_playerControls.leftThumb("Vertical") - lastCli_lStick) >= THRESHOLD))
        {
            Debug.Log( "currentIn: " + -1.0f * spt_playerControls.leftThumb("Vertical"));
            lastCli_lStick = spt_playerControls.leftThumb("Vertical");
            CmdSendLStickIn(-1.0f * lastCli_lStick);
            return;
        }

        lStickInput = spt_playerControls.leftThumb("Vertical");
        Debug.Log("Host L Stick : " + lStickInput);
    }

    [Command]
    public void CmdSendLStickIn( float input ) {
        lStickInput = input;
    }

}
