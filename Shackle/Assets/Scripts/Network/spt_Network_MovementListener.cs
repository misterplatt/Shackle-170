using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_Network_MovementListener : NetworkBehaviour {

    [SyncVar]
    public float aggregateLStickInput = 0.0F;

    void Update()
    {
        if (!isServer) return;
        float stickInput = 0.0F;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach ( GameObject player in players)
        {
            stickInput += player.GetComponent<spt_Network_Movement>().lStickInput;
        }
        aggregateLStickInput = stickInput;
    }

}
