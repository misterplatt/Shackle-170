using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class spt_NetworkLobby_ButtonSync_Client : NetworkBehaviour {
    //connection status syncing
    [SyncVar]
    public bool p1Connected;

    [SyncVar]
    public bool p2Connected;

    //PlayerReady Syncing
    [SyncVar]
    public bool p1Ready;

    [SyncVar]
    public bool p2Ready;

    //local scope
    Button p1Rdy;
    Button p2Rdy;

    spt_NetworkLobby_ButtonSync hostUI;

    void Start()
    {
        p1Rdy = this.transform.Find("P1_readystate").gameObject.GetComponent<Button>();
        p2Rdy = this.transform.Find("P2_readystate").gameObject.GetComponent<Button>();
        hostUI = GameObject.Find("Host_UI").GetComponent<spt_NetworkLobby_ButtonSync>();
        updateLogic();
        updateButtons();
    }

    void Update()
    {
        updateLogic();
        updateButtons();
    }

    void updateLogic()
    {
        
        p1Connected = hostUI.p1Connected;
        p2Connected = hostUI.p2Connected;
        p1Ready = hostUI.p1Ready;
        p2Ready = hostUI.p2Ready;

    }

    void updateButtons()
    {
        ColorBlock p1cb = p1Rdy.colors;
        ColorBlock p2cb = p2Rdy.colors;

        if (p1Connected) p1cb.disabledColor = Color.yellow;
        else p1cb.disabledColor = Color.grey;
        if (p2Connected) p2cb.disabledColor = Color.yellow;
        else p2cb.disabledColor = Color.grey;

        if (p1Ready && p1Connected)
        {
            p1cb.disabledColor = Color.green;
        }
        if (p2Ready && p2Connected) p2cb.disabledColor = Color.green;

        p1Rdy.colors = p1cb;
        p2Rdy.colors = p2cb;
    }
}
