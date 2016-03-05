using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spt_monsterAttackListener : NetworkBehaviour {
    public bool atkMsgSent = false;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (isServer |!isLocalPlayer) return;

        GameObject monster = GameObject.FindGameObjectWithTag("monster");
        spt_monsterMotivation moto = monster.GetComponent<spt_monsterMotivation>();
        if (moto.isAttacking) Cmd_ClientSensedAttack();
	}

    //Command tells monster that client has recieved packet update and is ready to actually attack.
    [Command]
    public void Cmd_ClientSensedAttack() {

        GameObject monster = GameObject.FindGameObjectWithTag("monster");
        spt_monsterMotivation moto = monster.GetComponent<spt_monsterMotivation>();
        moto.clientRecievedSignal = true;
    }

}
