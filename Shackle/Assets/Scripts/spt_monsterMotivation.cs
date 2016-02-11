//Created by: Lauren Cunningham

/** This file is the one that ultimately governs the monster's motivation. **/

using UnityEngine;
using System.Collections;

public class spt_monsterMotivation : MonoBehaviour {
    
    // Handle on the players (so the monster always knows where they are).
    public GameObject players;
    
    private spt_monsterMovement movementScript;
    
    private int angerLevel;

    // Used for motivation calculations
    private float fieldOfViewDegrees = 110f;
    private int visibilityDistance = 20;

    // A boolean used to track if the monster has warned the players that it is unhappy
    private bool hasGivenWarning;

    private int upperThreshold = 150;
    private int lowerThreshold = 100;

    // Use this for initialization
	void Start () {
        movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;

        // Sets the initial anger level of the monster to zero.
        angerLevel = 0;

        //Begins the gradual anger depreciation over time.
        InvokeRepeating("angerDepreciation", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {    
        if (angerLevel >= lowerThreshold)
            attack();
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

            // Detects if the item is blocked by another object in the world
            if (Physics.Raycast(alteredMonsterPosition, rayDirection, out hit, visibilityDistance))
            {

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

        // Monster can decide to attack or not
        int attackOrNot = UnityEngine.Random.Range(0, 3);

        // the monster will attack if the random number allows it, or if its anger has exceeded the top bound of its threshold.
        if (attackOrNot == 0 || (angerLevel >= upperThreshold)){

            // If the monster has not already warned the players of its anger, it does. 
            if (hasGivenWarning == false){
                // Play warning noise.
                hasGivenWarning = true;
            }

            // If the players have already been warned, it will attack
            else{
                movementScript.setWaypoint(999);
                movementScript.setDestination(players.transform);
            }
        }
    }

    // Increases the anger by an integer amount.
    public void updateAnger(int i)
    {
        angerLevel = angerLevel + i;
    }

    // Called evey second. Allows anger to depreciate over time.
    private void angerDepreciation()
    {
        if (angerLevel != 0)
            angerLevel = angerLevel - 1;

        // If the level of anger dips a bit below the bottom threshold, the monster will need to warn the players again if it gets angry.
        //  (This happens before it is allowed to actually attack).
        if (angerLevel <= (lowerThreshold - 10))
            hasGivenWarning = false;
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
