using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_NetworkManager_setSpawn : NetworkManager {

    static int hostConnection = -1;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Vector3 playerSpawnPos = new Vector3(0.0F, 0.0F, 0.0F);
        Vector3 spawnA = new Vector3(0.0F, 0.0F, 0.0F);
        Vector3 spawnB = new Vector3(0.0F, 0.0F, 0.0F);

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
            if (spawn.name == "Spawn_A") spawnA = spawn.transform.position;
            else spawnB = spawn.transform.position;
        }

        if (target == "Spawn_A") playerSpawnPos = spawnA;
        if (playerSpawnPos == new Vector3(0.0F, 0.0F, 0.0F) || target == "Spawn_B") playerSpawnPos = spawnB;

        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    
    
}
