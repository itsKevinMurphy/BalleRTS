using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {
    //if the player has decided to collect object
    public bool isInteractingTree = false;
    public bool isInteractingRock = false;
    public bool isInteractingPlant = false;

    //GameObjects that player interacts with
    GameObject tree;
    GameObject rock;
    GameObject plant;
   

    //The variables to be fed into the GUI
    public int count; //the score number
    public int woodCount;//the amount of wood collected
    private int mineralCount;  //the amount of minerals collected    
    public float health;//health is set on start
    public int enemyCount; // the amount of enemies
    int playerCount; //the amount of players 
    string playerNumber; //the string representation of the player number ie. Player1
    public float time;

    //text for the GUI
    public Text enemyCountText; //prefab Text for enemyCount
    public Text playerCountText; //prefab Text for playerCount
    public Text gameOver; //text displayed on end game
    public Text countText;//count text is Score
    public Text woodCountText; //the display of how much wood is collected
    public Text mineralCountText; //the display of how many minerals have been collected
    public Text prompt; //what is displayed when the player collides with a minable object
    public Text healthText; //public displayed health

    //The Styles used to chabge the look of the GIU Text
    public GUIStyle styleHealth, styleRock, styleWood, stylePrompt, styleScore, styleEnemy, stylePlayer;

    //The arrays used to count the amount of enemies and players
    public GameObject[] enemies; //array of enemy GameObjects
    public GameObject[] players; //array of player GameObjects

    //variables to get screen size for GUI
    protected float x;
    protected float y;

    void Start()
    {
        //set middle of screen
        x = Screen.width / 2;
        y = Screen.height / 2;

        //instantiate variables
        health = 100;
        count = 0;
        woodCount = 100;
        mineralCount = 0;

        //run methods
        SetStyles();
        CheckEnemyAndPlayerCount();
        SetCountText();
    }
	// Update is called once per frame
	void Update () {
        #region not interacting with resource when certain distance away
        //not interacting with resource when certain distance away, clear prompt
        if (plant != null && GetComponent<PlayerControllerBall>().CheckDistance(plant) > 4)
        {
            isInteractingPlant = false;
            GUI.enabled = false;
            plant = null;
        }

        else if (tree != null && GetComponent<PlayerControllerBall>().CheckDistance(tree) > 7)
        {
            isInteractingTree = false;
            GUI.enabled = false;
            tree = null;
        }
        else if (rock != null && GetComponent<PlayerControllerBall>().CheckDistance(rock) > 4)
        {
            isInteractingRock = false;
            GUI.enabled = false;
            rock = null;
        }
        #endregion
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
    void OnGUI()
    {
        SetCountText();

        GUI.Label(new Rect(10, 10, 100, 20), healthText.text, styleHealth);
        GUI.Label(new Rect(10, 30, 100, 20), countText.text, styleScore);
        GUI.Label(new Rect(10, 50, 100, 20), enemyCountText.text, styleEnemy);
        GUI.Label(new Rect(10, 70, 100, 20), playerCountText.text, stylePlayer);
        GUI.Label(new Rect(x, 10, 100, 20), playerNumber, stylePlayer);
        if (GetComponent<PlayerBase>().baseCreated)
        {
            GUI.Label(new Rect(x, 30, 100, 20), GetComponent<PlayerBase>().yourBase.tag, stylePlayer);
        }
        if (!GetComponent<PlayerBase>().baseCreated)
        {
            if (GUI.Button(new Rect(20, 150, 120, 50), "Build Base"))
            {
                GetComponent<PlayerBase>().CheckPlayerForBuild(transform.position, GetComponent<PlayerBase>().yourBase, playerNumber);
                GetComponent<PlayerBase>().baseCreated = true;
            }
        }

        #region Interacting with collectable
        //the GUI is only active if the player is interacting with collectable
        if (isInteractingTree || isInteractingRock || isInteractingPlant)
        {

            GUI.Label(new Rect(x - 200, y - 40, 300, 20), prompt.text, stylePrompt);

            if (GUI.Button(new Rect(x - 100, y, 160, 40), "Collect Resources"))
            {
                GetComponent<PlayerControllerBall>().playerFrozen = true;
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

        if (GetComponent<PlayerBase>().isHome && woodCount >= 2)
        {
            if (woodCount >= 3)
            {
                // Make the first button
                if (GUI.Button(new Rect(120, 40, 80, 50), "Damage"))
                {
                    GetComponent<PlayerBase>().spawnEffigies("damage");
                }
            }
            // Make the second button.
            if (woodCount >= 2)
            {
                if (GUI.Button(new Rect(220, 40, 80, 50), "Speed"))
                {
                    GetComponent<PlayerBase>().spawnEffigies("speed");
                }
            }
            // Make the third button.
            if (woodCount >= 4)
            {
                if (GUI.Button(new Rect(320, 40, 80, 50), "Defense"))
                {
                    GetComponent<PlayerBase>().spawnEffigies("defense");
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
    void CheckEnemyAndPlayerCount()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        players = GameObject.FindGameObjectsWithTag("Player");
        enemyCount = enemies.Length;
        playerCount = players.Length;

        //check for other players and assign player numbers
        if (playerCount == 1)
        {
            playerNumber = "Player1";
            GetComponent<PlayerBase>().yourBase.gameObject.tag = "Player1Base";
            Debug.Log("I am " + playerNumber);
        }
        else if (playerCount == 2)
        {
            playerNumber = "Player2";
            GetComponent<PlayerBase>().yourBase.gameObject.tag = "Player2Base";
            Debug.Log("I am " + playerNumber);
        }
        else if (playerCount == 3)
        {
            playerNumber = "Player3";
            GetComponent<PlayerBase>().yourBase.gameObject.tag = "Player3Base";
            Debug.Log("I am " + playerNumber);
        }
        else if (playerCount == 4)
        {
            playerNumber = "Player4";
            GetComponent<PlayerBase>().yourBase.gameObject.tag = "Player4Base";
            Debug.Log("I am " + playerNumber);
        }
    }
    public void SetCountText()
    {
        //Updates the score and resource counts
        countText.text = "Score: " + count.ToString();
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
            if (health >= 70)
            {
                //set colour as text
                styleHealth.normal.textColor = Color.green;
                healthText.text = "Health: " + health.ToString();
            }
            else if (health >= 40 && health <= 69)
            {
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

}
