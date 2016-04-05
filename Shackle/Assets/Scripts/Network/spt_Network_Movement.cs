using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_Network_Movement : NetworkBehaviour {

    [SyncVar]
    public float lStickInput;

    public const float THRESHOLD = 0.10f;
    public const  float MOVE_THRESHOLD = 1.5F;
    public const float pMoveRate = 0.005F;
    private const float playerSeperation = 2.0F;
    public float lastCli_lStick = 0.0f;

    public spt_Network_MovementListener mListener;
    public GameObject host;

    //Move toward host direction or client?
    enum movement {NONE, HOST, CLIENT};

    void Start()
    {
        mListener = GameObject.Find("WorldState").GetComponent<spt_Network_MovementListener>();
        lStickInput = 0.0F;

        if (isServer) host = this.gameObject;
        else {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach ( GameObject player in players) {
                if (this.gameObject != player) {
                    host = player;
                    break;
                }
            }
        }
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

        if (Input.GetKey(KeyCode.W)) {
            if (isServer) {
                moveHost(new Vector3(0.0F, 0.0F, 1.0F));
            }
        }
        else if (Input.GetKey(KeyCode.S)) {
            if (isServer) {
                moveHost(new Vector3(0.0F, 0.0F, -1.0F));
            }
        }

            if (!isServer) {
            Vector3 newTrans = host.transform.position;
            newTrans.z -= playerSeperation;
            this.transform.position = newTrans;
        }
        

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

    [Client]
    void moveClient(Vector3 dir) {

        this.transform.position += pMoveRate * dir;

    }

    void moveHost(Vector3 dir) {
        //move host player and also seperator and player models
        GameObject pSep = GameObject.FindGameObjectWithTag("seperator");
        GameObject[] pModels = GameObject.FindGameObjectsWithTag("pModel");

        pSep.transform.position += pMoveRate * dir;
        this.transform.position += pMoveRate * dir;

        foreach (GameObject entity in pModels) entity.transform.position += pMoveRate * dir;
        
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
