using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Home_ID : NetworkBehaviour {

    [SyncVar]
    public string homeName;

    private Transform homeTransform;


	// Use this for initialization
	void Start () {
        homeTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (homeTransform.name == "" || homeTransform.name.StartsWith("PlayerSpawnPoint")) {
            SetHomeIdentity();
        }
	}
    void SetHomeIdentity() {
        homeName = "";
    }


}
