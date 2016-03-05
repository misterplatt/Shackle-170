/* spt_monsterMovement.cs
 * 
 * Created by: Lauren Cunningham
 * Networking Modifications by : Ryan Connors
 * 
 * Last Revision Date: 2/26/2016 : Networking
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
    private spt_createGraphForGarage graphScript;

    // The waypoint that the monster is currently travelling toward
    private int currentWaypoint;

    // Used to guide the monster's movement
    private NavMeshAgent agent;

    // Use this for initialization
	void Start () {
        if (!isServer) return;
        
        //Gets the waypoint graph from another script, then sets the first waypoint to the center of the room.
        graphScript = GameObject.FindObjectOfType(typeof(spt_createGraphForGarage)) as spt_createGraphForGarage;
        waypointGraph = graphScript.getWaypointGraph();       
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.SetDestination(waypoints[16].position);
        currentWaypoint = 0;
	}

	// Update is called once per frame
	void Update () {
        if (!isServer) return;
        // Chooses a new destination if the monster is within a certain distance of its current one.
        if (agent.remainingDistance <= 2 && currentWaypoint != 999){
            chooseDestination();
        }
        if (agent.remainingDistance <= 2 && currentWaypoint == 999){
            Debug.LogWarning("attempting to alter playerLoss in puzzleStates...");
            networkScript = GameObject.FindGameObjectWithTag("Player").GetComponent<spt_NetworkPuzzleLogic>();
            
            networkScript.updatePuzzleState("playerLoss", true, "MonsterStandin");
            pLoss = true;
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
}
