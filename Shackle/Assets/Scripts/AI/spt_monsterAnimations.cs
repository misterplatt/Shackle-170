/* spt_monsterAnimations.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 5/11/2016
 * 
 * This file handles all of the monster's animations, and the translations needed to properly frame them.
 * This includes: the attacking animation/attacking capability.
 * In addition, this is where the monster's particle system (the spooky smoke) is turned on and off. */

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

    private bool particlesTimed = false;

    public void Update()
    {
        SkinnedMeshRenderer renderer = this.GetComponent<SkinnedMeshRenderer>();
        if ( renderer != null)
            renderer.enabled = render;
        SkinnedMeshRenderer[] renderers = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].enabled = render;
        if (!particlesTimed)
        {
            Invoke("particlesOn", 30);
            particlesTimed = true;
        }
    }

    // This function handles the monster's attack animation.
    //  It takes two arguments: the first is the position of the player being attacked.
    //  The second is an integer designating whether is is player 1 or 2 (A or B/ Host or Client) being attacked.
    public void attackPlayer(Transform playerPosition, int player){

        if (!monsterAttackInitiated)
        {
            spt_monsterMovement movementScript;
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
                this.transform.LookAt(GameObject.Find("Spawn_A").transform);
                movementScript.setDestination(playerPosition);
                render = true;
                monsterAttackInitiated = true;
                isInteracting = true;
                movementScript.setWaypoint(999);
                particlesOn();
                // play animation
            }

            // If it is player 2...
            else if (player == 1)
            {
                // Alter the monster's position and set its new destination to player 2 (the client)
                movementScript = GameObject.FindObjectOfType(typeof(spt_monsterMovement)) as spt_monsterMovement;
                this.transform.position = attackSpawnBStartingPosition;
                this.transform.LookAt(GameObject.Find("Spawn_B").transform);
                movementScript.setDestination(playerPosition);
                render = true;
                monsterAttackInitiated = true;
                isInteracting = true;
                movementScript.setWaypoint(999);
                particlesOn();
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
            this.transform.LookAt(interactableAnimationStartingPositions[index]);
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

            particlesOff();
        }
    }

    public void particlesOn()
    {
        gameObject.GetComponentInChildren<ParticleSystem>().enableEmission = true;
        Invoke("particlesOff", 2);
    }

    public void particlesOff()
    {
        if (isInteracting) return;
        gameObject.GetComponentInChildren<ParticleSystem>().enableEmission = false;
        particlesTimed = false;
    }
}
