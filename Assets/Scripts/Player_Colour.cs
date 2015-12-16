using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Colour : NetworkBehaviour {

    [SerializeField]
    public GameObject player;
    [SyncVar]
    private Color playerColor;
    private NetworkIdentity networkID;

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            playerColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            player.GetComponent<Renderer>().material.color = playerColor;
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	
	}

    [Command]
    void Cmd_ProvideColorToServer(Color c)
    {

        playerColor = c;
    }

    [ClientCallback]
    void TransmitColor()
    {
        if (isLocalPlayer)
        {
            Cmd_ProvideColorToServer(playerColor);
        }
    }

    public override void OnStartClient()
    {
        StartCoroutine(UpdateColor(1.5f));

    }

    IEnumerator UpdateColor(float time)
    {

        float timer = time;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            TransmitColor();
            if (!isLocalPlayer)
                GetComponentInChildren<Renderer>().material.color = playerColor;


            yield return null;
        }


    }

}