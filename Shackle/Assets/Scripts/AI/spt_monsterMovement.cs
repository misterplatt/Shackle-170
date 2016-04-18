/* spt_monsterMovement.cs
 * 
 * Created by: Lauren Cunningham
 * Networking Modifications by : Ryan Connors
 * 
 * Last Revision Date: 4/6/2016 : Networking
 * 
 * This file is the one that ultimately governs the monster's movements. **/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_monsterMovement : NetworkBehaviour {
    private spt_NetworkPuzzleLogic networkScript;

    [SyncVar]
    public bool pLoss = false;
    
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

    private bool startedAttackAnimation = false;

    public bool fuseBoxActivation = false;

    // Use this for initialization
	void Start () {
        if (!isServer) return;
        
        //Gets the waypoint graph from another script, then sets the first waypoint to the center of the room.
        if (garageScript != null)
            waypointGraph = garageScript.getWaypointGraph();
        else if (rangerOutpostScript != null)
            waypointGraph = rangerOutpostScript.getWaypointGraph();
        else
            waypointGraph = opticsLabScript.getWaypointGraph();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.SetDestination(waypoints[16].position);
        currentWaypoint = 16;
	}

	// Update is called once per frame
	void Update () {

        networkScript = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
        networkScript.updatePuzzleState("correctFuseCombo", fuseBoxActivation, "Fuse Box");

        if (!isServer) return;
        // Chooses a new destination if the monster is within a certain distance of its current one.
        if (agent.remainingDistance <= 3 && currentWaypoint != 999 && currentWaypoint != 888){
            chooseDestination();
        }
        if (agent.remainingDistance <= 2 && currentWaypoint == 999 && currentWaypoint != 888){
            Debug.LogWarning("attempting to alter playerLoss in puzzleStates...");
            networkScript = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();

            networkScript.updatePuzzleState("playerLoss", true, "MonsterStandin");
            pLoss = true;

            animationScript = GameObject.FindObjectOfType(typeof(spt_monsterAnimations)) as spt_monsterAnimations;
            animationScript.animator.SetInteger("animation", 1);
        }
        if (agent.remainingDistance <= 2 && currentWaypoint == 888)
        {
            animationScript = GameObject.FindObjectOfType(typeof(spt_monsterAnimations)) as spt_monsterAnimations;
            interactionScript = GameObject.FindObjectOfType(typeof(spt_monsterInteraction)) as spt_monsterInteraction;
            animationScript.disengageInteraction();
            interactionScript.interactWithObject(interactionScript.getInteractionName(), interactionScript.getInteractionItemName());
        }
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
