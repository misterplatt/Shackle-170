/* spt_Network_Movement.cs
 * 
 * Created by: Ryan Connors
 * 
 * Last Revision Date: 2/25/2016
 * 
 * This file Allow for network based movement.
 */


using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_Network_Movement : NetworkBehaviour {

    [SyncVar]
    public float lStickInput;
    [SyncVar]
    public int hostAnimator_var;
    public int clientAnimator_var;

    public const float THRESHOLD = 0.10f;
    public const  float MOVE_THRESHOLD = 1.5F;
    public const float pMoveRate = 0.005F;
    private const float playerSeperation = 2.0F;
    public float lastCli_lStick = 0.0f;

    public spt_Network_MovementListener mListener;
    public GameObject host;

    //DEBUG USAGE
    public bool toggleStick = false;
    public bool revStick = false;
    //Move toward host direction or client?
    enum movement {NONE, HOST, CLIENT};

    public Animator animator;
    public GameObject pModel;
    public GameObject pSpawn;

    void Start()
    {
        mListener = GameObject.Find("WorldState").GetComponent<spt_Network_MovementListener>();
        lStickInput = 0.0F;

        //if this is the host player, assign it the gameobject
        if (isServer) host = this.gameObject;
        //otherwise find the host player and set the reference so we can get it's position.
        else {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach ( GameObject player in players) {
                if (this.gameObject != player) {
                    host = player;
                    break;
                }
            }
        }

        linkModelPrefab();
        linkSpawnPrefab();
        hostAnimator_var = 0;
        clientAnimator_var = 0;
    }

    //linkModelPrefab find's the proper player model and uses it to collect the animator.
    void linkModelPrefab() {
        GameObject[] playerModels = GameObject.FindGameObjectsWithTag("pModel");
        GameObject closest = playerModels[0];
        float minDist = Vector3.Distance(this.transform.position, playerModels[0].transform.position);

        //find the closest playermodel and assign it's reference to out instance var
        for (int index = 1; index < playerModels.Length; ++index) {
            float thisDist = Vector3.Distance(this.transform.position, playerModels[index].transform.position);
            if (thisDist < minDist) {
                minDist = thisDist;
                closest = playerModels[index];
            }

        }

        pModel = closest;
        animator = pModel.GetComponent<Animator>();
    }

    void linkSpawnPrefab() {
        GameObject[] playerSpawns = GameObject.FindGameObjectsWithTag("spawn");
        GameObject closest = playerSpawns[0];
        float minDist = Vector3.Distance(this.transform.position, playerSpawns[0].transform.position);

        //find the closest playermodel and assign it's reference to out instance var
        for (int index = 1; index < playerSpawns.Length; ++index) {
            float thisDist = Vector3.Distance(this.transform.position, playerSpawns[index].transform.position);
            if (thisDist < minDist) {
                minDist = thisDist;
                closest = playerSpawns[index];
            }

        }

        pSpawn = closest;
    }

    //returns true if both bumpers are pressed, else false.
    bool bumpers()
    {
        return (spt_playerControls.rightBumperPressed() && spt_playerControls.leftBumperPressed());
    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;

        //if this player is not server, have it update the current thumbstick input to the server
        if (!isServer && (Mathf.Abs(spt_playerControls.leftThumb("Vertical") - lastCli_lStick) >= THRESHOLD))
        {
            Debug.Log( "currentIn: " + -1.0f * spt_playerControls.leftThumb("Vertical"));
            lastCli_lStick = spt_playerControls.leftThumb("Vertical");
            if (bumpers()) CmdSendLStickIn(-1.0f * lastCli_lStick);
            else CmdSendLStickIn(0.0f);
            return;
        }

        //get left thumb stick input.
        lStickInput = spt_playerControls.leftThumb("Vertical");
        /* DEBUG Movement Check
        
        if (Input.GetKey(KeyCode.F10)) {
            lStickInput = 2.0F;
        }
        
        */


        //if the left thumb stick input is greater than threshold...
        if (mListener.aggregateLStickInput > 1.5F) {
            //if it's the server, just move it since we own the object. Client won't do anything
            if (isServer && bumpers()) {
                moveHost(new Vector3(0.0F, 0.0F, 1.0F));
                animator.SetInteger("animation", 2);
                hostAnimator_var = 2;
            }
        }
        //if its greater than the negative threshold, move in opposite direction
        else if (mListener.aggregateLStickInput < -1.5F) {
            if (isServer && bumpers()) {
                moveHost(new Vector3(0.0F, 0.0F, -1.0F));
                animator.SetInteger("animation", 1);
                hostAnimator_var = 1;
            }
        }
        else {
            //otherwise ensure animator in rest state
            animator.SetInteger("animation", 0);
            hostAnimator_var = 0;
        }

        //if this isn't the server...
        if (!isServer) {
            //first update clientAnimator_Var
            clientAnimator_var = host.GetComponent<spt_Network_Movement>().hostAnimator_var;
            animator.SetInteger("animation", clientAnimator_var);

            //move my transform such that it always syncs with hosts movement.
            Vector3 newTrans = host.transform.position;
            newTrans.z -= playerSeperation;
            this.transform.position = newTrans;
            //pSpawn.transform.position = newTrans;
        }
    }

    void dbg_PlayerInputsLog() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players) {
            spt_Network_Movement pMovement = player.GetComponent<spt_Network_Movement>();
            Debug.Log(player.name + " has leftStick Input : " + pMovement.lStickInput);
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
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawn");

        if (pSep != null) pSep.transform.position += pMoveRate * dir;
        this.transform.position += pMoveRate * dir;
        pSpawn.transform.position += pMoveRate * dir;
        
        foreach (GameObject entity in pModels) entity.transform.position += pMoveRate * dir;
        foreach (GameObject spawn in spawns) spawn.transform.position += pMoveRate * dir;
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
