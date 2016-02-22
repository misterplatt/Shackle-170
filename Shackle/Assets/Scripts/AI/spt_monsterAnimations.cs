/* spt_monsterAnimations.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 2/22/2016
 * 
 * This file handles all of the monster's animations, and the translations needed to properly frame them.
 * This includes: the attacking animation/attacking capability.*/

using UnityEngine;
using System.Collections;

public class spt_monsterAnimations : MonoBehaviour {

    public Animator animator;
    public int animation = 0;

    private bool monsterAttackInitiated = false;

    // Defines the position on the axis where the monster will start its attack from.
    public Vector3 attackSpawnAStartingPosition;
    public Vector3 attackSpawnBStartingPosition;

    // This function handles the monster's attack animation.
    //  It takes two arguments: the first is the position of the player being attacked.
    //  The second is an integer designating whether is is player 1 or 2 (A or B/ Host or Client) being attacked.
    public void attackPlayer(Transform playerPosition, int player){

        if (!monsterAttackInitiated)
        {
            spt_monsterMovement movementScript;
            MeshRenderer renderer = this.GetComponent<MeshRenderer>();

            // If it is player 1...
            if (player == 0)
            {
                // Alter the monster's position and set its new destination to player 1 (the host)
                movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
                this.transform.position = attackSpawnAStartingPosition;
                movementScript.setDestination(playerPosition);
                renderer.enabled = true;
                monsterAttackInitiated = true;
                // play animation
            }

            // If it is player 2...
            else if (player == 1)
            {
                // Alter the monster's position and set its new destination to player 2 (the client)
                movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
                this.transform.position = attackSpawnBStartingPosition;
                movementScript.setDestination(playerPosition);
                renderer.enabled = true;
                monsterAttackInitiated = true;
                // play animation
            }
        }
    }
}
