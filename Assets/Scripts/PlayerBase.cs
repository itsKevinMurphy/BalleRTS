using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerBase : NetworkBehaviour
{

    //variables for the operation of base and spawning effigies
    private float effegies;
    public bool isHome = false;
    public float maxDist = 30f;
    public float dist;

    //locations
    Vector3 home;

    // Use this for initialization
    void Start()
    {
        home = GameObject.FindGameObjectWithTag("home").transform.position;
        effegies = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
        dist = Vector3.Distance(this.transform.position, home);
        if (dist < maxDist)
        {
            isHome = true;
        }
        else
        {
            isHome = false;
        }

    }
[Command]
    public void CmdSpawnEffigies(string itemSpawn)
    {
        effegies += 1;
        if (effegies >= 10)
        {
            maxDist += 10;
        }
        if (itemSpawn == "damage")
        {                                
            var damageEffegy = (GameObject)Instantiate(Resources.Load("damage"), new Vector3(transform.position.x + 2, transform.position.y - 1, transform.position.z + 2), Quaternion.identity);
            NetworkServer.SpawnWithClientAuthority(damageEffegy, base.connectionToClient);
            GetComponent<PlayerControllerBall>().damage += 10;
            transform.localScale += new Vector3(.5f, .5f, .5f);
            GetComponent<PlayerGUI>().count += 1000;
            GetComponent<PlayerGUI>().woodCount -= 3;
            GetComponent<PlayerGUI>().SetCountText();
        }
        else if (itemSpawn == "speed")
        {
            var speedEffegy = (GameObject)Instantiate(Resources.Load("speed"), new Vector3(transform.position.x + 2, transform.position.y - 1, transform.position.z + 2), Quaternion.identity);
            NetworkServer.SpawnWithClientAuthority(speedEffegy, base.connectionToClient);
            GetComponent<PlayerControllerBall>().speed += 200;
            GetComponent<PlayerGUI>().count += 1500;
            GetComponent<PlayerGUI>().woodCount -= 2;
            GetComponent<PlayerGUI>().SetCountText();
        }
        else if (itemSpawn == "defense")
        {
            var defenseEffegy = (GameObject)Instantiate(Resources.Load("defense"), new Vector3(transform.position.x + 2, transform.position.y - 1, transform.position.z + 2), Quaternion.identity);
            NetworkServer.SpawnWithClientAuthority(defenseEffegy, base.connectionToClient); 
            GetComponent<PlayerControllerBall>().defense += 0.2f;
            GetComponent<PlayerGUI>().count += 750;
            GetComponent<PlayerGUI>().woodCount -= 4;
            GetComponent<PlayerGUI>().SetCountText();
        }
    }
}
