using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerControllerBall : NetworkBehaviour
{

    //The variables 
    public float damage; //damage to enemies
    public float defense;//the multiplier for defense
    public float speed = 500; //speed of the player 
    public float treeDistance; //distance from object
    public float rockDistance;//distance from object
    public float plantDistance;//distance from object
    public float playerFrozenTimeout = 5;//the amount of time the player is frozen while collecting
    public float playerFrozenTimeRemaining; //the amount of time left that the player is frozen while collecting
    public bool playerFrozen = false; //whether or not the player is frozen
    private float playerScale; //the size of the player
    public float time;
    string itemSpawn;
    private int spawnIndex;

    //control Variables
    private bool jump; // whether the jump button is currently pressed
    private float jumpPower = 5f; // the force added to the ball when it jumps
    private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.

    //GameObjects   
	public GameObject playerAttacking;
    public GameObject myPlayer;
    GameObject[] spawnPoints;
   
    
    Vector3 movement;
    private Vector3 flat = new Vector3(1, 0, 1);
  
   
   
    private Rigidbody m_Rigidbody; //rigid body for player
   
    string playerNumber;
    public GameObject player1;
    GameObject enemy;

    void Start()
    {

        m_Rigidbody = GetComponent<Rigidbody>();       
        playerFrozenTimeRemaining = playerFrozenTimeout;
        damage = 1000;
        defense = 1.2f;
    }

	void Update()
	{      
       
        # region Player Movement
        //getting the keyboard inputs
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        jump = CrossPlatformInputManager.GetButton("Jump");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        //player movement, ball is actually pushed by force
        m_Rigidbody.AddForce(movement * speed * Time.deltaTime);

        if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength) && jump)
        {
            // ... add force in upwards.
            m_Rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        #endregion

        #region Player Frozen
        if (playerFrozen && playerFrozenTimeRemaining > 0)
        {
            playerFrozen = true;
            playerFrozenTimeRemaining -= Time.deltaTime;
            GetComponent<PlayerGUI>().isInteractingTree = false;
            GetComponent<PlayerGUI>().isInteractingRock = false;
            GetComponent<PlayerGUI>().isInteractingPlant = false;
        }
        else 
        {
            //sets the time remaining to the timeout given
            playerFrozenTimeRemaining = playerFrozenTimeout;
            //unfreezes player
            playerFrozen = false;           
        }
        if (playerFrozen) { m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll; }
        else { m_Rigidbody.constraints = RigidbodyConstraints.None; }
        #endregion
     
        if(transform.position.y<=-5)
        {
            GetComponent<PlayerGUI>().health = -100;
        }
    }
    public float CheckDistance(GameObject collectable)
    {
        float distance = Vector3.Distance(collectable.transform.position, transform.position);
        return distance;
    }
	
    private void OnCollisionEnter(Collision hit)
    {
        #region Collide With Enemy
        if (hit.gameObject.tag == "Enemy")
        {
            enemy = hit.gameObject;
            float damageFromEnemy = enemy.GetComponent<EnemyFSM>().damage;
            GetComponent<PlayerGUI>().health -= damageFromEnemy / defense;
            Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            GetComponent<PlayerGUI>().SetCountText();
            if (GetComponent<PlayerGUI>().health <= 0)
            {
                Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Debug.Log("Player is dead.");                
                transform.localScale = flat;
            }
        }
        #endregion

        #region Collide With Other Player
        if (hit.gameObject.tag == "Player")
		{
			playerAttacking = hit.gameObject;
            float damageFromPlayer = playerAttacking.GetComponent<PlayerControllerBall>().damage;
            GetComponent<PlayerGUI>().health -= damageFromPlayer / defense;
			Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            GetComponent<PlayerGUI>().SetCountText();
            if (GetComponent<PlayerGUI>().health <= 0)
            {
                Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Debug.Log("Player is dead.");               
                transform.localScale = flat;
            }
        }
        #endregion
    }
   
}
