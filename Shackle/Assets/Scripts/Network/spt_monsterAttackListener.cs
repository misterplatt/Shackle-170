using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class spt_monsterAttackListener : NetworkBehaviour {
    public bool atkMsgSent = false;
    public bool interactMsgSent = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (isServer |!isLocalPlayer) return;
        if (SceneManager.GetActiveScene().name == "net_playerlobby" || SceneManager.GetActiveScene().name == "LoadScreen") return;
        GameObject monster = GameObject.FindGameObjectWithTag("monster");
        spt_monsterMotivation moto = monster.GetComponent<spt_monsterMotivation>();
        spt_monsterAnimations anim = monster.GetComponent<spt_monsterAnimations>();
        if (moto.isAttacking) Cmd_ClientSensedAttack();
        if (anim.isInteracting) Cmd_ClientSensedInteraction();
    }

    //Command tells monster that client has recieved packet update and is ready to actually attack.
    [Command]
    public void Cmd_ClientSensedAttack() {
        GameObject monster = GameObject.FindGameObjectWithTag("monster");
        spt_monsterMotivation moto = monster.GetComponent<spt_monsterMotivation>();
        moto.clientRecievedSignal = true;
    }

    [Command]
    public void Cmd_ClientSensedInteraction()
    {
        GameObject monster = GameObject.FindGameObjectWithTag("monster");
        spt_monsterAnimations anim = monster.GetComponent<spt_monsterAnimations>();
        anim.clientRecieved = true;
    }
}
