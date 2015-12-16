using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public GameObject speed;
    public GameObject damage;
    public GameObject health;
    public GameObject defense;
//    public Transform[] healthSpawnPoints;
//    public Transform[] damageSpawnPoints;
//    public Transform[] speedSpawnPoints;
//    public Transform[] defenseSpawnPoints;

    //max number of a specific resource on the map
    public int maxHealthResources = 50;
    public int maxSpeedResource = 50;
    public int maxDamageResource = 50;
    public int maxDefenseResource = 50;

   

    //respawn time
    public float respawnTime = 30f;

    // Use this for initialization
    void Start()
    {
        SpawnHealth();
        SpawnSpeed();
        SpawnDefense();
        SpawnDamage();
        //spawns and respawns 
       // InvokeRepeating("SpawnHealth", 1, respawnTime);
      //  InvokeRepeating("SpawnDamage", 1, respawnTime);
      //  InvokeRepeating("SpawnSpeed", 1, respawnTime);
        //InvokeRepeating("SpawnDefense", 1, respawnTime);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SpawnHealth()
    {
		//creates a list of empty spawn point for the health resource
		GameObject[] healthSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnHealth");
		List<GameObject> emptyHealthSpawnPoint = new List<GameObject>(healthSpawnPoints);
//		List<Transform> emptyHealthSpawnPoint = new List<Transform>(healthSpawnPoints);
        
        // checks if a spawn point is empty
        for (int i = 0; i < maxHealthResources; i++)
        {
            if (emptyHealthSpawnPoint.Count <= 0)
            {
                return;
            }
            int spawnPointIndex = Random.Range(0, emptyHealthSpawnPoint.Count);
            Transform pos = emptyHealthSpawnPoint[spawnPointIndex].transform;
            emptyHealthSpawnPoint.RemoveAt(spawnPointIndex);

            //spawns the health resource (Plant)
            Instantiate(health, pos.position, pos.rotation);
            Debug.Log("Health Resource Spawned");
        }
    }

    void SpawnDamage()
    {
		//creates a list of empty spawn point for the damage resource
		GameObject[] damageSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnDamage");
		List<GameObject> emptyDamageSpawnPoint = new List<GameObject>(damageSpawnPoints);
//		List<Transform> emptyDamageSpawnPoint = new List<Transform>(damageSpawnPoints);

        // checks if a spawn point is empty
        for (int i = 0; i < maxDamageResource; i++)
        {
            if (emptyDamageSpawnPoint.Count <= 0)
            {
                return;
            }
            int spawnPointIndex = Random.Range(0, emptyDamageSpawnPoint.Count);
            Transform pos = emptyDamageSpawnPoint[spawnPointIndex].transform;
            emptyDamageSpawnPoint.RemoveAt(spawnPointIndex);

            //spawns the damage resource (rock)
            Instantiate(damage, pos.position, pos.rotation);
            Debug.Log("Damage Resource Spawned");
        }
    }
    void SpawnSpeed()
    {
		//creates a list of empty spawn point for the speed resource
		GameObject[] speedSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnSpeed");
		List<GameObject> emptySpeedSpawnPoint = new List<GameObject>(speedSpawnPoints);
//		List<Transform> emptySpeedSpawnPoint = new List<Transform>(speedSpawnPoints);

        for (int i = 0; i < maxSpeedResource; i++)
        {
            if (emptySpeedSpawnPoint.Count <= 0)
            {
                return;
            }
            int spawnPointIndex = Random.Range(0, emptySpeedSpawnPoint.Count);
            Transform pos = emptySpeedSpawnPoint[spawnPointIndex].transform;
            emptySpeedSpawnPoint.RemoveAt(spawnPointIndex);

            //spawns the damage resource (speed)
            Instantiate(speed, pos.position, pos.rotation);
            Debug.Log("Speed Resource Spawned");
        }
    }

    void SpawnDefense()
    {
		//creates a list of empty spawn point for the speed resource
		GameObject[] defenseSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnDefense");
		List<GameObject> emptyDefenseSpawnPoint = new List<GameObject>(defenseSpawnPoints);
//		List<Transform> emptyDefenseSpawnPoint = new List<Transform>(defenseSpawnPoints);

        for (int i = 0; i < maxDefenseResource; i++)
        {
            if (emptyDefenseSpawnPoint.Count <= 0)
            {
                return;
            }
            int spawnPointIndex = Random.Range(0, emptyDefenseSpawnPoint.Count);
            Transform pos = emptyDefenseSpawnPoint[spawnPointIndex].transform;
            emptyDefenseSpawnPoint.RemoveAt(spawnPointIndex);

            //spawns the damage resource (speed)
            Instantiate(defense, pos.position, pos.rotation);
            Debug.Log("Defense Defense Spawned");
        }
    }
    
}
