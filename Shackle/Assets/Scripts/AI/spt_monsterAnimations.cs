/* spt_monsterAnimations.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 3/5/2016
 * 
 * This file handles all of the monster's animations, and the translations needed to properly frame them.
 * This includes: the attacking animation/attacking capability.*/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_monsterAnimations : NetworkBehaviour {

    public Animator animator;
    public int animation = 0;

    [SyncVar]
    public bool isInteracting = false;
    [SyncVar]
    public bool clientRecieved = false;
    [SyncVar]
    public bool render = false;

    private bool monsterAttackInitiated = false;

    // Defines the position on the axis where the monster will start its attack from.
    public Vector3 attackSpawnAStartingPosition;
    public Vector3 attackSpawnBStartingPosition;

    private bool interactionInitiated = false;

    public GameObject[] interactables;
    public Vector3[] interactableAnimationStartingPositions;


    public void Update()
    {
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        renderer.enabled = render;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = render;
    }

    // This function handles the monster's attack animation.
    //  It takes two arguments: the first is the position of the player being attacked.
    //  The second is an integer designating whether is is player 1 or 2 (A or B/ Host or Client) being attacked.
    public void attackPlayer(Transform playerPosition, int player){

        if (!monsterAttackInitiated)
        {
            spt_monsterMovement movementScript;
            MeshRenderer renderer = this.GetComponent<MeshRenderer>();
            if (!isServer) return;
            /*
            if (!isServer) {
                renderer.enabled = true;
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                    r.enabled = true;
                monsterAttackInitiated = true;
                return;
            }
            */

            // If it is player 1...
            if (player == 0)
            {
                // Alter the monster's position and set its new destination to player 1 (the host)
                movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
                this.transform.position = attackSpawnAStartingPosition;
                movementScript.setDestination(playerPosition);
                render = true;
                monsterAttackInitiated = true;
                isInteracting = true;
                // play animation
            }

            // If it is player 2...
            else if (player == 1)
            {
                // Alter the monster's position and set its new destination to player 2 (the client)
                movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
                this.transform.position = attackSpawnBStartingPosition;
                movementScript.setDestination(playerPosition);
                render = true;
                monsterAttackInitiated = true;
                isInteracting = true;
                // play animation
            }
        }
    }

    public void interactWithObject(string itemName)
    {
        if (!isServer) return;
        if (!interactionInitiated)
        {
            spt_monsterMovement movementScript;

            render = true;
            interactionInitiated = true;
            isInteracting = true;

            int index = -1;
            for (int i = 0; i < interactables.Length; i++)
            {
                if (interactables[i].name == itemName)
                    index = i;
            }

            movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
            this.transform.position = interactableAnimationStartingPositions[index];
            movementScript.setDestination(interactables[index].transform);
            movementScript.prevWaypoint = movementScript.currentWaypoint;
            movementScript.currentWaypoint = 888;
        }
    }

    public void disengageInteraction()
    {
        if (!isServer) return;
        if (interactionInitiated)
        {
            spt_monsterMovement movementScript;
            MeshRenderer renderer = this.GetComponent<MeshRenderer>();

            render = false;

            interactionInitiated = false;
            isInteracting = false;
            clientRecieved = false;

            movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
            movementScript.setDestination(movementScript.waypoints[movementScript.prevWaypoint]);
            movementScript.currentWaypoint = movementScript.prevWaypoint;

        }
    }
}
