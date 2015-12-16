using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyColour : NetworkBehaviour
{

    [SyncVar]
    private Color enemyColor;
    private NetworkIdentity networkID;

    // Use this for initialization
    void Start()
    {
        enemyColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        this.GetComponent<Renderer>().material.color = enemyColor;

    }

    // Update is called once per frame
    void Update()
    {


    }

    [Command]
    void Cmd_ProvideColorToServer(Color c)
    {

        enemyColor = c;
    }

    [ClientCallback]
    void TransmitColor()
    {
        Cmd_ProvideColorToServer(enemyColor);
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
            GetComponentInChildren<Renderer>().material.color = enemyColor;


            yield return null;
        }


    }

}