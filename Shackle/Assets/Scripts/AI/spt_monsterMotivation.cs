/* spt_monsterMotivation.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 4/3/2016
 * 
 * This file is the one that ultimately governs the monster's motivation. **/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_monsterMotivation : NetworkBehaviour {
    
    // Handle on the players (so the monster always knows where they are).
    public GameObject players;

    public GameObject[] spawns;
    
    private spt_monsterMovement movementScript;

    private spt_monsterAudio audioScript;

    private spt_monsterAnimations animationScript;

    public GameObject darknessPlane;
    
    [SyncVar]
    public int angerLevel;
    [SyncVar]
    public int whichPlayer;
    [SyncVar]
    public bool isAttacking;
    [SyncVar]
    public bool clientRecievedSignal = false;

    private int hostThreat = 0;
    private int clientThreat = 0;

    bool attackComplete = false;

    // Used for motivation calculations
    private float fieldOfViewDegrees = 110f;
    private int visibilityDistance = 20;

    // A boolean used to track if the monster has warned the players that it is unhappy
    private bool hasGivenWarning;

    private int upperThreshold = 250;
    private int lowerThreshold = 200;

    // Used to make sure that the monster gives sufficient warning before an attack.
    //  time is the elapsed time of the playthrough (in seconds)
    //  timeOfWarning is recorded time of a warning, with -1 denoting that the players are not in danger yet
    private int time = 0;
    private int timeOfWarning = -1;

    // Use this for initialization
	void Start () {
        movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
        animationScript = GameObject.FindObjectOfType(typeof(spt_monsterAnimations)) as spt_monsterAnimations;
        if (!isServer) return;

        players = getHost();

        // Sets the initial anger level of the monster to zero.
        angerLevel = 0;
        isAttacking = false;

        //Begins the gradual anger depreciation over time.
        InvokeRepeating("angerDepreciation", 1, 1);

        GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {
        if (isAttacking && clientRecievedSignal) netAttack();

        if (!isServer) return;
        if (angerLevel >= lowerThreshold)
            attack();
        
	}

    private GameObject getHost()
    {
        GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach ( GameObject player in _players)
        {
            if (player.GetComponent<NetworkIdentity>().isServer )
            {
                return player;
            }
        }

        return null;
    }

    //Determines if the monster can see an object from its current position.
    public bool canSeeSomething(Transform item)
    {
        RaycastHit hit;

        // Position of the monster in 3D space
        Vector3 alteredMonsterPosition = transform.position;

        // Position of the item that makes the monster angry in 3D space
        Vector3 alteredItemPosition = item.transform.position;

        // Ray drawn between the monster and the item
        Vector3 rayDirection = alteredItemPosition - alteredMonsterPosition;
        
        // Detects if the item is even within the view of the monster
        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 0.5f)
        {

            Debug.DrawRay(alteredMonsterPosition, rayDirection, Color.yellow, 1.0f, true);
            // Detects if the item is blocked by another object in the world
            if (Physics.Raycast(alteredMonsterPosition, rayDirection, out hit, visibilityDistance))
            {
                Debug.DrawRay(alteredMonsterPosition, rayDirection, Color.red, 1.0f, true);
                // Makes sure the object being looked at is something that angers the monster. MUST HAVE A "target" TAG
                if (hit.transform.CompareTag("target"))
                {

                    // Makes sure the object being looked at is visible to the monster
                    if (hit.collider.gameObject.GetComponent<spt_angerObject>().getData().getVisible())
                    {
                        return (true);
                    }
                }
            }
        }
        return false;
    }


    // Attack function for the monster.
    private void attack(){
        if (!isServer) return;
        // Monster can decide to attack or not
        int attackOrNot = UnityEngine.Random.Range(0, 3);

        // the monster will attack if the random number allows it, or if its anger has exceeded the top bound of its threshold.
        if (attackOrNot == 0 || (angerLevel >= upperThreshold)){

            // If the monster has not already warned the players of its anger, it does. 
            if (hasGivenWarning == false){
                // Play warning noise.
                hasGivenWarning = true;
                timeOfWarning = time;
                audioScript = GetComponent<spt_monsterAudio>();
                audioScript.prepWarningNoise();
            }

            // If the players have already been warned, and there was a 8 second interval between the initial warning and an attack
            else if ((hasGivenWarning==true) && (timeOfWarning!= -1) && (time-timeOfWarning>8)){
                //populate network fields
                //whichPlayer = Random.Range(0, spawns.Length);

                if (hostThreat > clientThreat)
                    whichPlayer = 0;
                else
                    whichPlayer = 1;

                isAttacking = true;
                movementScript.setWaypoint(999);
                animationScript.attackPlayer(spawns[whichPlayer].transform, whichPlayer);
            }
        }
    }

    public void netAttack()
    {
        if (attackComplete) return;
        attackComplete = true;
        Debug.Log("Attacking Player : " + spawns[whichPlayer].name );

        animationScript.attackPlayer(spawns[whichPlayer].transform, whichPlayer);
        angerLevel = 0;
    }

    // Increases the anger by an integer amount.
    public void updateAnger(int i, Transform pos)
    {
        if (!isServer) return;
        angerLevel = angerLevel + i;

        if (pos != null)
        {
            if (pos.position[2] > darknessPlane.transform.position[2])
            { // Side A
                hostThreat = hostThreat + i;
            }
            else // Side B
            {
                clientThreat = clientThreat + i;
            }
        }
    }

    // Called evey second. Allows anger to depreciate over time.
    private void angerDepreciation()
    {
        if (!isServer) return;
        if (angerLevel > 0)
            angerLevel = angerLevel - 1;
        if (hostThreat > 0)
            hostThreat = hostThreat - 1;
        if (clientThreat > 0)
            clientThreat = clientThreat - 1;

        // Update the elapsed playthrough time
        time = time + 1;

        // If the level of anger dips a bit below the bottom threshold, the monster will need to warn the players again if it gets angry.
        //  (This happens before it is allowed to actually attack).
        if (angerLevel <= (lowerThreshold - 10))
        {
            timeOfWarning = -1;
            hasGivenWarning = false;
        }

    }

    public int getAnger(){
        return angerLevel;
    }

    public void raiseTheThreshold()
    {
        lowerThreshold = lowerThreshold + 25;
        upperThreshold = upperThreshold + 25;
    }

    public void lowerTheThreshold()
    {
        lowerThreshold = lowerThreshold - 25;
        upperThreshold = upperThreshold - 25;
    }
}
