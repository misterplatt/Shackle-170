/* spt_createGraphForOpticsLab.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 4/15/2016
 * 
 * This file will be used to set up the undirected graph that will govern the monster's movement, specifically for the 
 * optics lab level (level 3). Another file like this will be created for each of Shackle's levels. */

using UnityEngine;
using System.Collections;

public class spt_createGraphForOpticsLab : MonoBehaviour
{

    //There are 16 waypoints in the lab.
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
        waypointGraph = new int[16][];

        waypointGraph[0] = new int[] { 1, 15 };
        waypointGraph[1] = new int[] { 0, 2, 3, 7 };
        waypointGraph[2] = new int[] { 1, 3, 7 };
        waypointGraph[3] = new int[] { 1, 2, 4, 5, 7 };
        waypointGraph[4] = new int[] { 3, 5, 7 };
        waypointGraph[5] = new int[] { 3, 4, 6, 7 };
        waypointGraph[6] = new int[] { 5, 8 };
        waypointGraph[7] = new int[] { 1, 2, 3, 4, 5 };
        waypointGraph[8] = new int[] { 6, 9, 12 };
        waypointGraph[9] = new int[] { 8, 10, 11, 12 };
        waypointGraph[10] = new int[] { 9 };
        waypointGraph[11] = new int[] { 8, 9, 12, 13, 14 };
        waypointGraph[12] = new int[] { 8, 9, 11, 13, 14 };
        waypointGraph[13] = new int[] { 11, 12, 14 };
        waypointGraph[14] = new int[] { 11, 12, 13, 15 };
        waypointGraph[15] = new int[] { 0, 14 };
    }

    // Returns the graph so that it can be used by other scripts.
    public int[][] getWaypointGraph()
    {
        return waypointGraph;
    }
}
