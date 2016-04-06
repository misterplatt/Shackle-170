using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class spt_PlayerNetworkID : NetworkBehaviour {

    [SyncVar]
    public string playerUniqueName;
    private NetworkInstanceId playerNetID;
    private Transform myTransform;

    public override void OnStartLocalPlayer() {
        getNetIdentity();
        setIdentity();
    }

    [Client]
    void getNetIdentity() {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerID(makeUniqueIdentity() );
    }

    string makeUniqueIdentity() {
        string uniqueName = "Player " + playerNetID.ToString();
        return uniqueName;
    }

    [Command]
    void CmdTellServerID( string name ) {
        playerUniqueName = name;
    }
    
    void setIdentity() {
        if (!isLocalPlayer) myTransform.name = playerUniqueName;
        else myTransform.name = makeUniqueIdentity();
    }

    void Awake() {
        myTransform = transform;
    }

    void Update() {
        if (myTransform.name == "" || myTransform.name == "Parented_Camera_Player(Clone)") {
            setIdentity();
        }
    }
}
