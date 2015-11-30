using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerController : NetworkBehaviour
{
    //health is set on start
    public float health;
    //damage to enemies
    public float damage;
    //the multiplier for defense
    public float defense;
    //speed of the player
	public float speed = 500;
    //count text is Score
	public Text countText;
    //the display of how much wood is collected
    public Text woodCountText;
    //the display of how many minerals have been collected
    public Text mineralCountText;
    //what is displayed when the player collides with a minable object
    public Text prompt;
    //public displayed health
    public Text healthText;
    //the score number
	public int count;
    //the amount of wood collected
    public int woodCount;
    //the amount of minerals collected
    private int mineralCount;
    //if the player has decided to collect object
    bool isInteractingTree = false;
    bool isInteractingRock = false;
    bool isInteractingPlant = false;
    public float treeDistance;
    public float rockDistance;
    public float plantDistance;
    protected float x;
    protected float y;
    //the amount of time the player is frozen while collecting
    public float playerFrozenTimeout = 5;
    //the amount of time left that the player is frozen while collecting
    public float playerFrozenTimeRemaining;
    //whether or not the player is frozen
    bool playerFrozen = false;
    private float playerScale;
    private float effegies;
    GameObject tree;
    GameObject rock;
	GameObject plant;
	GameObject enemy;
    [SerializeField] GameObject yourBase;
	public GameObject playerAttacking;
    public float time;
    bool isHome = false;
    Vector3 home;
    public Transform player;
    string itemSpawn;
    public float maxDist = 20f;
    public float dist;
    Vector3 movement;
    public GUIStyle styleHealth, styleRock, styleWood, stylePrompt, styleScore, styleEnemy, stylePlayer;
    private int spawnIndex;
    public GameObject myPlayer;
    GameObject[] spawnPoints;
    private Vector3 flat = new Vector3(1, 0, 1);
    public Text enemyCountText; //prefab Text for enemyCount
    public Text playerCountText; //prefab Text for playerCount
    public Text gameOver; //text displayed on end game
    public GameObject[] enemies; //array of enemy GameObjects
    public GameObject[] players; //array of player GameObjects
    public int enemyCount; // the amount of enemies
    int playerCount; //the amount of players 
    private bool jump; // whether the jump button is currently pressed
    private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
    private Rigidbody m_Rigidbody; //rigid body for player
    private float jumpPower = 5f; // the force added to the ball when it jumps
    string playerNumber;
    public GameObject player1;
    bool baseCreated = false;

	void Start()
	{
      x = Screen.width / 2;
      y = Screen.height / 2;
      SetStyles();
  
      m_Rigidbody = GetComponent<Rigidbody>();
      enemies = GameObject.FindGameObjectsWithTag("Enemy");
      players = GameObject.FindGameObjectsWithTag("Player");
      enemyCount = enemies.Length;
      playerCount = players.Length;
      yourBase = SetBase();

      foreach (GameObject play in players)
      {
          if (play.GetComponent<PlayerController>().playerCount < this.playerCount)
          {
              play.GetComponent<PlayerController>().playerCount = this.playerCount;
          }
      }

      if (playerCount == 1)
      {
          playerNumber = "Player1";
          yourBase.gameObject.tag = "Player1Base";
          Debug.Log("I am " + playerNumber);
      }
      else if (playerCount == 2)
      {
          playerNumber = "Player2";
          yourBase.gameObject.tag = "Player2Base";
          Debug.Log("I am " + playerNumber);
      }
      else if (playerCount == 3)
      {
          playerNumber = "Player3";
          yourBase.gameObject.tag = "Player3Base";
          Debug.Log("I am " + playerNumber);
      }
      else if (playerCount == 4)
      {
          playerNumber = "Player4";
          yourBase.gameObject.tag = "Player4Base";
          Debug.Log("I am " + playerNumber);
      }

      players = GameObject.FindGameObjectsWithTag("Player");
      foreach (GameObject play in players)
      {
          Debug.Log("ForEach set player 1 - I am " + playerNumber);

          if (play.GetComponent<PlayerController>().playerNumber == "Player1")
          {
              Debug.Log(play.name.ToString());
              player1 = play;
              Debug.Log(player1.name.ToString());
          }
      }
      
        effegies = 0;
        home.y = home.y * 0;
        playerFrozenTimeRemaining = playerFrozenTimeout;
        health = 100;
        damage = 1000;
        defense = 1.2f;
        count = 0; 
        woodCount = 100; 
        mineralCount = 0;
        SetCountText();
	}

	void Update()
	{      
       
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
            isInteractingTree = false;
            isInteractingRock = false;
            isInteractingPlant = false;
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
        #region not interacting with resource when certain distance away
        //not interacting with resource when certain distance away, clear prompt
        if (plant != null && CheckDistance(plant) > 4)
        {
            isInteractingPlant = false; 
            GUI.enabled = false;
            plant = null;
        }
        
        else if (tree != null && CheckDistance(tree) > 7) 
        {
            isInteractingTree = false; 
            GUI.enabled = false;
            tree = null;
        }        
        else if (rock != null && CheckDistance(rock) > 4) 
        {
            isInteractingRock = false; 
            GUI.enabled = false;
            rock = null;
        }
        #endregion
        if(transform.position.y<=-5)
        {
            health = -100;
        }
    }
    public float CheckDistance(GameObject collectable)
    {
        float distance = Vector3.Distance(collectable.transform.position, transform.position);
        return distance;
    }
	void OnTriggerEnter(Collider other)
	{
        //if player runs into something, the collider is set by tagging the game object you want to check for
		if (other.gameObject.tag == "PickUp") 
		{
            //auto destroys pickup and adds to count, then sets count text
			other.gameObject.SetActive(false);
			count = count + 100;
			SetCountText();
		}
        if (other.gameObject.tag == "Tree" || other.gameObject.tag == "Bush")
        {
            //sets prompt text for harvesting tree
            prompt.text = "Would you like harvest?";
            //player is now interacting with tree - true
            isInteractingTree = true;  
            //sets the variable gameObject tree to the tree that the player collided with
            tree = other.gameObject;
        }

        if (other.gameObject.tag == "Health")
        {
            prompt.text = "Collect plant to restore health?";
            isInteractingPlant = true;
            //sets the variable gameObject tree to the tree that the player collided with
            plant = other.gameObject;
        }

        if (other.gameObject.tag == "Rock")
        {
            //sets prompt text for harvesting minerals
            prompt.text = "Mine these minerals?";
            //player is now interacting with rock - true
            isInteractingRock = true;       
            //sets the variable gameObject tree to the tree that the player collided with
            rock = other.gameObject;          
        }
	}
    private void OnCollisionEnter(Collision hit)
    {
        #region Collide With Enemy
        if (hit.gameObject.tag == "Enemy")
        {
            enemy = hit.gameObject;
            float damageFromEnemy = enemy.GetComponent<EnemyFSM>().damage;
            health -= damageFromEnemy / defense;
            Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            SetCountText();
            if (health <= 0)
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
			float damageFromPlayer = playerAttacking.GetComponent<PlayerController>().damage;
            health -= damageFromPlayer  / defense;
			Instantiate(Resources.Load("Explosion"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
			SetCountText();
            if (health <= 0)
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

    public void SetStyles()
    {
        //Set original colours
        styleHealth.normal.textColor = Color.green;
        styleScore.normal.textColor = Color.blue;
        styleWood.normal.textColor = Color.yellow;
        styleRock.normal.textColor = Color.grey;
        stylePrompt.normal.textColor = Color.white;
        styleEnemy.normal.textColor = Color.red;
        stylePlayer.normal.textColor = Color.black;

        //set font sizes
        styleHealth.fontSize = 20;
        styleScore.fontSize = 20;
        styleWood.fontSize = 20;
        styleRock.fontSize = 20;
        stylePrompt.fontSize = 30;
        styleEnemy.fontSize = 20;
        stylePlayer.fontSize = 20;
    }
    void OnGUI() 
    {
        SetCountText();
        
        GUI.Label(new Rect(10, 10, 100, 20), healthText.text, styleHealth);
        GUI.Label(new Rect(10, 30, 100, 20), countText.text, styleScore);
        GUI.Label(new Rect(10, 50, 100, 20), enemyCountText.text, styleEnemy);
        GUI.Label(new Rect(10, 70, 100, 20), playerCountText.text, stylePlayer);
        GUI.Label(new Rect(x, 10, 100, 20), playerNumber, stylePlayer);
        if(baseCreated)
        {
            GUI.Label(new Rect(x, 30, 100, 20), yourBase.tag, stylePlayer);
        }
        if (!baseCreated)
        {
            if (GUI.Button(new Rect(20, 150, 120, 50), "Build Base"))
            {                
                CheckPlayerForBuild(transform.position, yourBase, playerNumber);
                baseCreated = true;
            }
        }

        #region Interacting with collectable
        //the GUI is only active if the player is interacting with collectable
        if (isInteractingTree || isInteractingRock || isInteractingPlant)
        {

            GUI.Label(new Rect(x - 200, y - 40, 300, 20), prompt.text, stylePrompt);

            if (GUI.Button(new Rect(x - 100, y, 160, 40), "Collect Resources"))
            {                
                playerFrozen = true;
                #region isInteractingTree
                if (isInteractingTree)
                {                    
                    time = Time.time;
                    //grabs the particle effect attached to the tree and starts it
                    tree.GetComponent<ParticleSystem>().Play();                    
                    //Destroys the tree game object after 5 seconds, the time is the second argument
                    Destroy(tree, 5f);
                    //adds the wood resource to count 
                    woodCount = woodCount + 1;
                    //Add Points to the overall score
                    count = count + 300;
                    //Updates the score and resource counts
                    SetCountText();
                }
                #endregion
                #region isInteractingRock
                else if (isInteractingRock)
                {
                    time = Time.time;
                    //grabs the particle effect attached to the rock and starts it
                    rock.GetComponent<ParticleSystem>().Play();
                    //Destroys the rock game object after 5 seconds, the time is the second argument
                    Destroy(rock, 5f);
                    //adds the wood resource to count 
                    mineralCount = mineralCount + 1;
                    //Add Points to the overall score
                    count = count + 500;
                    //Updates the score and resource counts
                    SetCountText();
                }
                #endregion
                #region isInteractingPlant
                else if (isInteractingPlant)
                {
                    time = Time.time;
                    //grabs the particle effect attached to the rock and starts it
                    plant.GetComponent<ParticleSystem>().Play();
                    //Destroys the rock game object after 5 seconds, the time is the second argument
                    Destroy(plant, 5f);
                    //adds the wood resource to count 
                    health = health + 10;
                    //Add Points to the overall score
                    count = count + 1000;
                    //Updates the score and resource counts
                    SetCountText();
                }
                #endregion
            }
        }
        #endregion

        if (isHome && woodCount >= 2)
        {
            if (woodCount >= 3)
            {
                // Make the first button
                if (GUI.Button(new Rect(120, 40, 80, 50), "Damage"))
                {
                    spawnEffigies("damage");
                }
            }
            // Make the second button.
            if (woodCount >= 2)
            {
                if (GUI.Button(new Rect(220, 40, 80, 50), "Speed"))
                {
                    spawnEffigies("speed");
                }
            }
            // Make the third button.
            if (woodCount >= 4)
            {
                if (GUI.Button(new Rect(320, 40, 80, 50), "Defense"))
                {
                    spawnEffigies("defense");
                }
            }
        }
        if (health <= 0)
        {
            stylePrompt.normal.textColor = Color.red;
            stylePrompt.fontSize = 70;
            stylePrompt.fontStyle = FontStyle.Bold;
            gameOver.text = "You're Dead\n" + countText.text + "\nEnemies Left: " + enemyCount;
            GUI.Label(new Rect(x - 300, y - 40, 300, 20), gameOver.text, stylePrompt);
        }

        if (enemyCount <= 0 && playerCount <= 1)
        {
            stylePrompt.normal.textColor = Color.green;
            stylePrompt.fontSize = 70;
            stylePrompt.fontStyle = FontStyle.Bold;
            gameOver.text = "Congratulations!\n" + countText.text;
            GUI.Label(new Rect(x - 300, y - 40, 300, 20), gameOver.text, stylePrompt);
        }
    }

    public void CheckPlayerForBuild(Vector3 location, GameObject baseToBuild, string player)
    {
        if (player == "Player1") { Debug.Log("If Player 1 Passed"); CmdSpawnBase(transform.position, baseToBuild); }
        else 
        { 
            Debug.Log("location: " + location.ToString());
            Debug.Log("baseToBuild: " + yourBase.tag);
            Debug.Log("Player: " + player);
            Debug.Log("If Other Player Passed");
            
            player1.GetComponent<PlayerController>().CmdSpawnBase(transform.position, SetBase());
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

	void SetCountText()
	{
        //Updates the score and resource counts
		countText.text = "Score: " + count.ToString ();
        healthText.text = "Health: " + health.ToString();
        enemyCountText.text = "Enemies: " + enemyCount.ToString();
        playerCountText.text = "Players: " + playerCount.ToString();
        if (woodCount > 0)
        {
            //refreshes the screen text
            woodCountText.text = "Wood Collected: " + woodCount.ToString();
            GUI.Label(new Rect(10, 90, 200, 20), woodCountText.text, styleWood);          
        }
            //have no count text if none is collected
        else { woodCountText.text = ""; }
        if (mineralCount > 0)
        {
            //refreshes the screen text
            mineralCountText.text = "Minerals Collected: " + mineralCount.ToString();
            GUI.Label(new Rect(10, 110, 200, 20), mineralCountText.text, styleRock); 
        }
        //have no count if none is collected
        else { mineralCountText.text = ""; }

        #region change the colour of the Health Text
        //change the colour of the Health Text based on health left
        if (health >= 0)
        {
            //refreshes the screen text
            healthText.text = "Health: " + health.ToString();
            if (health >= 70){
                //set colour as text
                styleHealth.normal.textColor = Color.green;
                healthText.text = "Health: " + health.ToString();
            }
            else if (health >= 40 && health <=69){
                //set colour as text
                styleHealth.normal.textColor = Color.yellow;
                healthText.text = "Health: " + health.ToString();
            }
            else if (health >= 1 && health <= 39)
            {
                //set colour as text
                styleHealth.normal.textColor = Color.red;
                healthText.text = "Health: " + health.ToString();
            }
        }
        //have no count if none is collected
        else { healthText.text = ""; }
        #endregion

        #region change the colour of the Enemies Text
        //change the colour of the Health Text based on health left
        if (enemyCount >= 0)
        {
            //refreshes the screen text
            enemyCountText.text = "Enemies: " + enemyCount.ToString();
            if (enemyCount >= 15)
            {
                //set colour as text
                styleEnemy.normal.textColor = Color.red;
                enemyCountText.text = "Enemies: " + enemyCount.ToString();
            }
            else if (enemyCount >= 5 && enemyCount <= 14)
            {
                //set colour as text
                styleEnemy.normal.textColor = Color.yellow;
                enemyCountText.text = "Health: " + enemyCount.ToString();
            }
            else if (enemyCount >= 1 && enemyCount <= 4)
            {
                //set colour as text
                styleEnemy.normal.textColor = Color.green;
                enemyCountText.text = "Health: " + enemyCount.ToString();
            }
        }
        #endregion
    }

    void spawnEffigies(string itemSpawn)
    {
        effegies += 1;
        if (effegies >= 10)
        {
            maxDist += 10;
        }
        if (itemSpawn == "damage")
        {
            Instantiate(Resources.Load("damage"), new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2), Quaternion.identity);
            damage += 10;
            transform.localScale += new Vector3(.5f,.5f,.5f);
            count += 1000;
            woodCount -= 3;
            SetCountText();
        }
        else if (itemSpawn == "speed")
        {
            Instantiate(Resources.Load("speed"), new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2), Quaternion.identity);
            speed += 200;
            count += 1500;
            woodCount -= 2;
            SetCountText();
        }
        else if (itemSpawn == "defense")
        {
            Instantiate(Resources.Load("defense"), new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2), Quaternion.identity);
            defense += 0.2f;
            count = count + 750;
            woodCount -= 4;
            SetCountText();
        }
    }
}
