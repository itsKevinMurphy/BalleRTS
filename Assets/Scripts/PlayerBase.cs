using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerBase : NetworkBehaviour {
 
    //variables for the operation of base and spawning effigies
    private float effegies;
    public bool isHome = false;   
    public bool baseCreated = false;
    public float maxDist = 20f;
    public float dist;

    //locations
    Vector3 home;

    //Game objects
    [SerializeField] public GameObject yourBase;

	// Use this for initialization
	void Start () {
	    yourBase = SetBase();
        effegies = 0;
        home.y = home.y * 0;
	}
	
	// Update is called once per frame
	void Update () {
	 if (baseCreated)
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
	}
    public void spawnEffigies(string itemSpawn)
    {
        effegies += 1;
        if (effegies >= 10)
        {
            maxDist += 10;
        }
        if (itemSpawn == "damage")
        {
            Instantiate(Resources.Load("damage"), new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2), Quaternion.identity);
             GetComponent<PlayerControllerBall>().damage += 10;
            transform.localScale += new Vector3(.5f, .5f, .5f);
            GetComponent<PlayerGUI>().count += 1000;
            GetComponent<PlayerGUI>().woodCount -= 3;
            GetComponent<PlayerGUI>().SetCountText();
        }
        else if (itemSpawn == "speed")
        {
            Instantiate(Resources.Load("speed"), new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2), Quaternion.identity);
            GetComponent<PlayerControllerBall>().speed += 200;
            GetComponent<PlayerGUI>().count += 1500;
            GetComponent<PlayerGUI>().woodCount -= 2;
            GetComponent<PlayerGUI>().SetCountText();
        }
        else if (itemSpawn == "defense")
        {
            Instantiate(Resources.Load("defense"), new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2), Quaternion.identity);
            GetComponent<PlayerControllerBall>().defense += 0.2f;
            GetComponent<PlayerGUI>().count += 750;
            GetComponent<PlayerGUI>().woodCount -= 4;
            GetComponent<PlayerGUI>().SetCountText();
        }
    }
      
    public GameObject SetBase()
    {
        GameObject thisBase = Resources.Load("Base", typeof(GameObject)) as GameObject;
        return thisBase;
    }

    [Command]
    void CmdSpawnBase(Vector3 location, GameObject baseToBuild)
    {
        Debug.Log("Spawning base: " + baseToBuild.tag);
        GameObject homeBase = (GameObject)Instantiate(baseToBuild, location,
        Quaternion.identity);
        NetworkServer.Spawn(homeBase);
        Debug.Log("Spawning base: Success! " + baseToBuild.tag);
    }

    public void CheckPlayerForBuild(Vector3 location, GameObject baseToBuild, string player)
    {
        if (player == "Player1") 
        {
            Debug.Log("If Player 1 Passed"); 
            CmdSpawnBase(transform.position, baseToBuild); 
        }
        else
        {
            Debug.Log("location: " + location.ToString());
            Debug.Log("baseToBuild: " + GetComponent<PlayerBase>().yourBase.tag);
            Debug.Log("Player: " + player);
            Debug.Log("If Other Player Passed");

            //player1.GetComponent<PlayerControllerBall>().CmdSpawnBase(transform.position, SetBase());
        }
    }
}
