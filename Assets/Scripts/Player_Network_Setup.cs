using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Network_Setup : NetworkBehaviour {
    [SerializeField] Camera BallCamera;
    [SerializeField] GameObject Player;
    [SerializeField] MonoBehaviour script;
    [SerializeField] AudioListener listener;
    [SerializeField] MonoBehaviour cameraControlScript;
	// Use this for initialization
	void Start () {
        Debug.Log(isLocalPlayer.ToString());
        if (isLocalPlayer)
        {
            Debug.Log("if Passed");
            GameObject.Find("Scene Camera").SetActive(false);
            script.enabled = true;
            BallCamera.enabled = true;
            Player.GetComponent<Rigidbody>().isKinematic = false;
            listener.enabled = true;
            cameraControlScript.enabled = true;
        }
        else
        {
            Debug.Log("if failed");
            Debug.Log(isLocalPlayer.ToString());
        }
	}
	
}
