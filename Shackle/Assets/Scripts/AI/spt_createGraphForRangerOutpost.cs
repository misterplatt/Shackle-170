/* spt_createGraphForRangerOutpost.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 4/6/2016
 * 
 * This file will be used to set up the undirected graph that will govern the monster's movement, specifically for the 
 * ranger outpost level. Another file like this will be created for each of Shackle's levels. */

using UnityEngine;
using System.Collections;

public class spt_createGraphForRangerOutpost : MonoBehaviour
{

    //There are 15 waypoints in the outpost.
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
        waypointGraph = new int[15][];

        waypointGraph[0] = new int[] { 1, 6, 14 };
        waypointGraph[1] = new int[] { 0, 2, 3, 6 };
        waypointGraph[2] = new int[] { 1, 3, 6 };
        waypointGraph[3] = new int[] { 1, 2, 4, 5, 6 };
        waypointGraph[4] = new int[] { 3, 5, 6 };
        waypointGraph[5] = new int[] { 4, 6, 7 };
        waypointGraph[6] = new int[] { 0, 1, 2, 3, 4, 5, 7 };
        waypointGraph[7] = new int[] { 5, 6, 8 };
        waypointGraph[8] = new int[] { 7, 9, 13 };
        waypointGraph[9] = new int[] { 8, 10, 13 };
        waypointGraph[10] = new int[] { 8, 9, 11, 13, 14 };
        waypointGraph[11] = new int[] { 10, 12, 13 };
        waypointGraph[12] = new int[] { 10, 11, 13, 14 };
        waypointGraph[13] = new int[] { 8, 9, 10, 11, 12, 14 };
        waypointGraph[14] = new int[] { 0, 12, 13 };
    }

    // Returns the graph so that it can be used by other scripts.
    public int[][] getWaypointGraph()
    {
        return waypointGraph;
    }
}
