/* spt_monsterMovement.cs
 * 
 * Created by: Lauren Cunningham
 * Networking Modifications by : Ryan Connors
 * 
 * Last Revision Date: 4/29/2016
 * 
 * This file is the one that ultimately governs the monster's movements. **/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_monsterMovement : NetworkBehaviour {
    private spt_NetworkPuzzleLogic networkScript;

    [SyncVar]
    public bool pLoss = false;

    [SyncVar]
    public int animIndex;
    private int lastAnimIndex;
    
    // Array of waypoints, the graph that holds the waypoints, as well as the script that instantiates the graph itself 
    public Transform[] waypoints;
    private int[][] waypointGraph;
    public spt_createGraphForGarage garageScript;
    public spt_createGraphForRangerOutpost rangerOutpostScript;
    public spt_createGraphForOpticsLab opticsLabScript;
    private spt_monsterAnimations animationScript;
    private spt_monsterInteraction interactionScript;

    // The waypoint that the monster is currently travelling toward
    public int currentWaypoint;
    public int prevWaypoint;

    // Used to guide the monster's movement
    private NavMeshAgent agent;

    private bool pWin = false;

    private bool startedAttackAnimation = false;

    private int monsterPuzzleCompletionIndex = -1;

    private bool navMeshInitialized = false;
    private bool waypointInitialized = false;

    // Use this for initialization
	void Start () {
        animIndex = 0;
    }

	// Update is called once per frame
	void Update () {
        animationScript = GameObject.FindObjectOfType(typeof(spt_monsterAnimations)) as spt_monsterAnimations;
        if (animIndex != lastAnimIndex)
        {
            animationScript.animator.SetInteger("animation", animIndex);
            lastAnimIndex = animIndex;
        }

        if (!isServer) return;

        if (!navMeshInitialized || !waypointInitialized)
        {
            //Gets the waypoint graph from another script, then sets the first waypoint to the center of the room.
            if (garageScript != null)
                waypointGraph = garageScript.getWaypointGraph();
            else if (rangerOutpostScript != null)
                waypointGraph = rangerOutpostScript.getWaypointGraph();
            else
                waypointGraph = opticsLabScript.getWaypointGraph();

            agent = GetComponent<NavMeshAgent>();
            agent.enabled = true;
            agent.SetDestination(waypoints[0].position);
            currentWaypoint = 0;

            if (agent != null)
                navMeshInitialized = true;
            if (waypointGraph != null)
                waypointInitialized = true;
        }

        networkScript = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
        if (monsterPuzzleCompletionIndex == -1)
        {
            for (int i = 0; i < networkScript.PuzzleStates.Count; i++)
            {
                if (networkScript.PuzzleStates[i].name == "puzzleCompletionMonster" )
                {
                    monsterPuzzleCompletionIndex = i;
                }
            }
        }

        //Either Attack or Drag.
        if (agent.remainingDistance <= 5 && agent.remainingDistance > 2 && currentWaypoint == 999)
        {
            animIndex = 1;
            //animationScript.animator.SetInteger("animation", 1);
        }

        // If the monster is attacking, or the players have just won...
        else if (agent.remainingDistance <= 2 && currentWaypoint == 999 && currentWaypoint != 888)
        {

            // If loss needs to happen
            if (networkScript.PuzzleStates[monsterPuzzleCompletionIndex].state == false)
            {
                Debug.LogWarning("attempting to alter playerLoss in puzzleStates...");
                networkScript.updatePuzzleState("playerLoss", true, "MonsterStandin");
                pLoss = true;
            }

            // If win needs to happen
            else
            {
                Debug.LogWarning("attempting to alter puzzleCompletion in puzzleStates...");
                networkScript.updatePuzzleState("puzzleCompletion", true, networkScript.PuzzleStates[0].itemName);
            }
        }
        // If the monster is interacting with an item...
        else if (agent.remainingDistance <= 1 && currentWaypoint == 888)
        {
            animationScript = GameObject.FindObjectOfType(typeof(spt_monsterAnimations)) as spt_monsterAnimations;
            interactionScript = GameObject.FindObjectOfType(typeof(spt_monsterInteraction)) as spt_monsterInteraction;
            animationScript.disengageInteraction();
            interactionScript.interactWithObject(interactionScript.getInteractionName(), interactionScript.getInteractionItemName());
        }

        else if (agent.remainingDistance <= 2 && currentWaypoint != 999 && currentWaypoint != 888 && currentWaypoint != 777)
            chooseDestination();
	}

    // Used to choose a new destination for the monster based on its current destination (used for wandering).
    //  This is dependant on the graph of waypoints which is created in  the spt_createGraphFor... scripts.
    void chooseDestination(){
        int numOptions = waypointGraph[currentWaypoint].Length;
        int newDestination = UnityEngine.Random.Range(0, numOptions);
        currentWaypoint = waypointGraph[currentWaypoint][newDestination];
        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    public void setDestination(Transform t){
        agent.SetDestination(t.position);
    }

    public void setWaypoint(int i){
        currentWaypoint = i;
    }

    public int getWaypoint(){
        return currentWaypoint;
    }

    private void loss()
    {
        
    }
}
