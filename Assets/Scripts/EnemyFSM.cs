using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum EnemyState { Patrol, Attack, Collect, Dying, Dead }

public class EnemyFSM : MonoBehaviour
{
	
	private GameObject player;
	private GameObject resource;
    private GameObject enemy, enemyAttacking;
	private EnemyState currentState;

	public float destinationNearPoint = 2f;
	public float attackNearPoint = 10f;
	public float attackStopPoint = 15f;
	public float collectNearPoint = 5f;
	public float speed = 10;	//use navmesh agent speed
	public float hP = 25;
    public float defense;
    public float damage;
    public Text status;
	private string resourceTag;
	private int destinationIndex;
//	public Transform[] destinationPoints;
	private Transform currentDestinationPoint;
    private NavMeshAgent agent;
	void Start()
    {
		//set initial random point to visit on patrol
		agent = GetComponent<NavMeshAgent>();
        SetDestinationPoint();
        currentState = EnemyState.Patrol;
        damage = 5;
        defense = 1.05f;
    }

    void Update()
    {
        status.transform.position = this.transform.position;
       // agent.SetDestination(currentDestinationPoint.position);
        if (hP <= 10 && hP >= 1)
        {
            currentState = EnemyState.Dying;
        }
        if (hP <= 0)
        {
            currentState = EnemyState.Dead;
        }
        switch (currentState)
        {
            case EnemyState.Patrol:
                status.text = "Patrolling";
                //move the enemy towards random point
                MoveTowardsTarget(currentDestinationPoint);
                resource = FindNearestResource();
				enemy = FindEnemies();
                player = FindPlayer();
                //if random point has been visited, set another random point
                if (Vector3.Distance(transform.position, currentDestinationPoint.position) < destinationNearPoint)
				{
                    currentState = EnemyState.Patrol;
					SetDestinationPoint();                   
				}

                //if player gets close, change state to attack and set destination to player location
                if (Vector3.Distance(transform.position, player.transform.position) < attackNearPoint)
                {
                    Debug.Log("KRISTIAN IS AWESOME");
                    currentState = EnemyState.Attack;
                    currentDestinationPoint = player.transform;
                   // agent.SetDestination(currentDestinationPoint.position);
                }

                //if player another enemy close, change state to attack and set destination to that enemy location
                if (Vector3.Distance(transform.position, enemy.transform.position) < attackNearPoint)
                {
                    currentState = EnemyState.Attack;
                    currentDestinationPoint = enemy.transform;
                  // agent.SetDestination(currentDestinationPoint.position);
                }
              
				//if any resource is close, change state to collec and set destination to resource location
				if (Vector3.Distance(transform.position, resource.transform.position) < collectNearPoint)
				{
					currentState = EnemyState.Collect;
					currentDestinationPoint = this.resource.transform;
                   // agent.SetDestination(currentDestinationPoint.position);
				}
                break;

            case EnemyState.Attack:
                status.text = "Attacking";
				//move enemy towards player
                MoveTowardsTarget(currentDestinationPoint);
				
				//if player gets far, go back to patrol and set random point to visit
				if (Vector3.Distance(transform.position, player.transform.position) > attackStopPoint)
				{
					currentState = EnemyState.Patrol;
			        SetDestinationPoint();
                   // agent.SetDestination(currentDestinationPoint.position);
				}
                break;

			case EnemyState.Collect:
                status.text = "Collecting";
				if (resource!=null) {
                    MoveTowardsTarget(currentDestinationPoint);
                    resourceTag = resource.gameObject.tag;
                    resource.GetComponent<ParticleSystem>().Play();
                    Destroy(resource, 5f);              
                   
				}

				if (resource==null) {
					//then add effect of the resource collected
					if (resourceTag.Equals("Health")) {
						hP += 10;
						Debug.Log("Enemy hP = " + hP);
                    }
                    if (resourceTag.Equals("Tree"))
                    {
                        speed += 5;
                    }
                    if (resourceTag.Equals("Bush"))
                    {
                        damage += 5;
                    }
                    if (resourceTag.Equals("Rock"))
                    {
                        defense += .05f;
                    }
					//set new random point and go back to patrol
					SetDestinationPoint();
					currentState = EnemyState.Patrol;
				}
                break;

            case EnemyState.Dying:
                status.text = "Dying...";
				resource = FindNearestHP();
				currentState = EnemyState.Collect;
				currentDestinationPoint = resource.transform;
                //agent.SetDestination(currentDestinationPoint.position);
                break;

            case EnemyState.Dead:
                status.text = "Dead";
                Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                Destroy(this.gameObject, 1);
				//destroy this enemy
				break;
        }

    }

	//sets a random destination point
    private void SetDestinationPoint()
	{        
		GameObject[] patrolPoints = GameObject.FindGameObjectsWithTag("SpawnPatrol");
		int newIndex = Random.Range(1, patrolPoints.Length - 1);
        destinationIndex = (newIndex + destinationIndex) % patrolPoints.Length;
        currentDestinationPoint = patrolPoints[destinationIndex].transform;


    }
	//moves enemy towards whatever is passed as the target
	//target can either be a random point, the player or a resource
    private void MoveTowardsTarget(Transform target)
	{
        if (agent && target)
        {
            agent.destination = target.position;
        }
      //  GetComponent<Rigidbody>().AddForce(new Vector3(target.position.x, 0, target.position.z) * speed * Time.deltaTime);
      //  transform.position = Vector3.MoveTowards(transform.position, agent.destination, Time.deltaTime * speed);
//		agent.SetDestination(target.position);         

	}


	//if under attack
	private void OnCollisionEnter(Collision hit){
		if ("Player" == hit.gameObject.tag) {
			//hP = hP - damage;
            player = hit.gameObject;
            float damageReceived = player.GetComponent<PlayerController>().damage;
            Debug.Log("Enemy hP = " + hP + " - Damage Received: " + damageReceived);
			hP -= (damageReceived/defense);
            Debug.Log("Enemy hP = " + hP);
            Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);          

            if (hP <= 10 && hP >= 1) {
				currentState = EnemyState.Dying;
			}
			if (hP <= 0) {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject play in players)
                {
                    play.GetComponent<PlayerController>().enemyCount -= 1;
                }
                player.GetComponent<PlayerController>().count += 2000;
				currentState = EnemyState.Dead;
			}
           
		}

        if (hit.gameObject.tag == "Enemy")
        {
            Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);        

            enemyAttacking = hit.gameObject;
            float damageReceivedEnemy = enemyAttacking.GetComponent<EnemyFSM>().damage;
            Debug.Log("Enemy hP = " + hP + " - Damage Received from enemy: " + damageReceivedEnemy);
            hP -= (damageReceivedEnemy / defense);
            Debug.Log("Enemy hP = " + hP);
        }
	}

	//find the nearest resource (any type)
	private GameObject FindNearestResource() {
		GameObject nearest = null;
		GameObject[] defensePoints = GameObject.FindGameObjectsWithTag("Defense");
		GameObject[] speedPoints = GameObject.FindGameObjectsWithTag("Tree");
		GameObject[] damagePoints = GameObject.FindGameObjectsWithTag("Bush");
		List<GameObject> resourcePoints = new List<GameObject>();

		float distance = Mathf.Infinity;
		Vector3 position = transform.position;

        foreach (GameObject defensePoint in defensePoints)
        {
            resourcePoints.Add(defensePoint);
		}
		foreach (GameObject speedPoint in speedPoints) {
			resourcePoints.Add (speedPoint);
		}
		foreach (GameObject damagePoint in damagePoints) {
			resourcePoints.Add (damagePoint);
		}

		foreach (GameObject resourcePoint in resourcePoints) {
			Vector3 difference = resourcePoint.transform.position - position;
			float currentDistance = difference.sqrMagnitude;
			if (currentDistance < distance) {
				nearest = resourcePoint;
				distance = currentDistance;
			}
		}
		return nearest;
	}

	//find the nearest health
	private GameObject FindNearestHP() {
		GameObject nearest = null;
		GameObject[] healthPoints;
		healthPoints = GameObject.FindGameObjectsWithTag("Health");
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;

		foreach (GameObject healthPoint in healthPoints) {
			Vector3 difference = healthPoint.transform.position - position;
			float currentDistance = difference.sqrMagnitude;
			if (currentDistance < distance) {
				nearest = healthPoint;
				distance = currentDistance;
			}
		}
		return nearest;
	}

    //find other enemies
    private GameObject FindEnemies()
    {
        GameObject nearest = null;
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
//        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 difference = enemy.transform.position - transform.position;
            float currentDistance = difference.sqrMagnitude;
            if (currentDistance < distance && currentDistance > 2)
            {
                nearest = enemy;
                distance = currentDistance;
            }
        }
        return nearest;
    }

    //find other enemies
    private GameObject FindPlayer()
    {
        GameObject nearest = null;
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        float distance = Mathf.Infinity;
        //        Vector3 position = transform.position;

        foreach (GameObject player in players)
        {
            Vector3 difference = player.transform.position - transform.position;
            float currentDistance = difference.sqrMagnitude;
            if (currentDistance < distance && currentDistance > 2)
            {
                nearest = player;
                distance = currentDistance;
            }
        }
        return nearest;
    }

}
