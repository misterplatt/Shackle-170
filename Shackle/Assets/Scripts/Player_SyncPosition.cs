using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncPosition : NetworkBehaviour {
    [SyncVar]
    private Vector3 syncPos;

    [SerializeField]
    Transform myTransform;
    [SerializeField]
    float lerpRate = 15;

    private Vector3 lastPos;
    private float threshold = 0.5f;

    // Use this for initialization
    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    void LerpPosition()
    {
        if(!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer (Vector3 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            //move inside so we aren't checking transforms for things we care not about.
            if (Vector3.Distance(myTransform.position, lastPos) > threshold)
            {
                CmdProvidePositionToServer(myTransform.position);
                lastPos = myTransform.position;
            }
        }
    }

}
