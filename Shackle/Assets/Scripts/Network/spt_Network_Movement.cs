using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_Network_Movement : NetworkBehaviour {

    [SyncVar]
    public float lStickInput;

    public const float THRESHOLD = 0.10f;
    public const  float MOVE_THRESHOLD = 1.5F;
    public const float pMoveRate = 0.005F;
    public float lastCli_lStick = 0.0f;

    //Move toward host direction or client?
    enum movement {NONE, HOST, CLIENT};

    void Start()
    {
        lStickInput = 0.0F;
    }
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;

        //if this player is not server, have it update the current thumbstick input to the server
        if (!isServer && (Mathf.Abs(spt_playerControls.leftThumb("Vertical") - lastCli_lStick) >= THRESHOLD))
        {
            Debug.Log( "currentIn: " + -1.0f * spt_playerControls.leftThumb("Vertical"));
            lastCli_lStick = spt_playerControls.leftThumb("Vertical");
            CmdSendLStickIn(-1.0f * lastCli_lStick);
            return;
        }

        lStickInput = spt_playerControls.leftThumb("Vertical");
        //dbg_PlayerInputsLog();
        //collectPlayerGroup();

        
        if (Input.GetKey(KeyCode.W)) moveClient(new Vector3(0.0F, 0.0F, 1.0F));
        
        /*
        if (Input.GetKey(KeyCode.S)) movePlayers(new Vector3(0.0F, 0.0F, -1.0F));
        */
    }

    void dbg_PlayerInputsLog() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players) {
            spt_Network_Movement pMovement = player.GetComponent<spt_Network_Movement>();
            Debug.Log(player.name + " has leftStick Input : " + pMovement.lStickInput);
        }
    }

    void senseMove() {
        switch(checkDirection()) {
            case movement.NONE:
                return;
            case movement.HOST:
                break;
            case movement.CLIENT:
                break;
        }
    }

    GameObject[] collectPlayerGroup() {
        GameObject[] pGroup = new GameObject[10];
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] pSep = GameObject.FindGameObjectsWithTag("seperator");
        GameObject[] pModels = GameObject.FindGameObjectsWithTag("pModel");
        
        players.CopyTo(pGroup, 0);
        pModels.CopyTo(pGroup, 2);
        pSep.CopyTo(pGroup, 4);

        return pGroup;
    }

    void movePlayers(Vector3 dir) {
        if (!isServer) return;

        GameObject[] playersGroup = collectPlayerGroup();
        foreach(GameObject entity in playersGroup) {
            entity.transform.position += pMoveRate * dir;
        }


    }

    [Client]
    void moveClient(Vector3 dir) {

        this.transform.position += pMoveRate * dir;

    }

    private float collectInput() {

        float aggregate = 0.0F;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players) {
            spt_Network_Movement pMovement = player.GetComponent<spt_Network_Movement>();
            aggregate += pMovement.lStickInput;
        }

        return aggregate;
    }

    private movement checkDirection() {
        float input = collectInput();

        if ( Mathf.Abs(input) < MOVE_THRESHOLD ) return movement.NONE;
        else if ( input > MOVE_THRESHOLD ) return movement.HOST;
        return movement.CLIENT;
    }
    

    [Command]
    public void CmdSendLStickIn( float input ) {
        lStickInput = input;
    }

}
