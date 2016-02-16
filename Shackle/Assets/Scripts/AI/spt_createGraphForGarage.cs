/* spt_createGraphForGarage.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 2/15/2016
 * 
 * This file will be used to set up the undirected graph that will govern the monster's movement, specifically for the 
 * gargage level. Another file like this will be created for each of Shackle's levels. */

using UnityEngine;
using System.Collections;

public class spt_createGraphForGarage : MonoBehaviour {

    //There are 20 waypoints in the garage.
    //Their indices are stored in a two dimensional array to mark links between them.
    private int[][] waypointGraph;

    // Use this for initialization
    void Awake()
    {
        setWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Sets up the graph (two-dimensional array).
    void setWaypoints()
    {
        waypointGraph = new int[20][];

        waypointGraph[0] = new int[] {1, 4};
        waypointGraph[1] = new int[] {0, 2, 3, 4};
        waypointGraph[2] = new int[] {1, 3};
        waypointGraph[3] = new int[] {0, 1, 2, 4, 5, 7};
        waypointGraph[4] = new int[] {0, 1, 3, 5, 7};
        waypointGraph[5] = new int[] {3, 4, 6, 7, 10};
        waypointGraph[6] = new int[] {5, 10};
        waypointGraph[7] = new int[] {3, 4, 5, 8};
        waypointGraph[8] = new int[] {7, 9, 10};
        waypointGraph[9] = new int[] {8, 10, 13};
        waypointGraph[10] = new int[] {6, 8, 9, 11, 12};
        waypointGraph[11] = new int[] {10, 12, 15};
        waypointGraph[12] = new int[] {10, 11, 13, 14, 15};
        waypointGraph[13] = new int[] {9, 12, 14};
        waypointGraph[14] = new int[] {12, 13, 15, 16};
        waypointGraph[15] = new int[] {11, 12, 14, 16};
        waypointGraph[16] = new int[] {14, 15, 17, 18, 19};
        waypointGraph[17] = new int[] {16, 18, 19};
        waypointGraph[18] = new int[] {16, 17, 19};
        waypointGraph[19] = new int[] {16, 17, 18};
    }

    // Returns the graph so that it can be used by other scripts.
    public int[][] getWaypointGraph()
    {
        return waypointGraph;
    }
}
