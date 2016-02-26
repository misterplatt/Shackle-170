using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_network_playermonster_ifc : NetworkBehaviour {


    [Command]
    public void CmdUpdateAnger(int i)
    {
        GameObject netMonster = NetworkServer.FindLocalObject(GameObject.FindGameObjectWithTag("Monster").GetComponent<NetworkIdentity>().netId);
        netMonster.GetComponent<spt_monsterMotivation>().updateAnger(i);
        //Debug.Log("anger delta : " + i);
        //angerLevel += i;
    }

}
