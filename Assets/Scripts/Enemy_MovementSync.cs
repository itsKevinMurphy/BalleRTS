using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy_MovementSync : NetworkBehaviour {

    [SyncVar]
    private Vector3 enemyPos;
    [SerializeField]
    Transform enemyTransform;
    [SerializeField]
    float lerpRate = 15;

	void FixedUpdate () {
        TransmitPosition();
	
	}

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        enemyPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
       
        CmdProvidePositionToServer(enemyTransform.position);
    }
}
