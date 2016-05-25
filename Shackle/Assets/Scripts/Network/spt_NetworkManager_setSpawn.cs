using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_NetworkManager_setSpawn : NetworkManager {

    static int hostConnection = -1;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Vector3 playerSpawnPos = new Vector3(0.0F, 0.0F, 0.0F);
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawn");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        string target = "Spawn_A";
        Quaternion rotation = Quaternion.identity;

        if (hostConnection != -1 && conn.connectionId != hostConnection) {
            target = "Spawn_B";
            rotation = Quaternion.Euler(rotation.eulerAngles + new Vector3(0.0F, 180.0F, 0.0F));
        } 
        else {
            hostConnection = conn.connectionId;
        }

        foreach (GameObject spawn in spawns ) {
            if (spawn.name == target) {
                playerSpawnPos = spawn.transform.position;

                Debug.Log(spawn.name);
            }
        }
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    

}
