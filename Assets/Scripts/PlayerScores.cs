using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerScores : NetworkBehaviour
{
    public GUIStyle styleHealth;
    GameObject[] enemies;
    GameObject[] players;
    public Text playerScoretxt, playerScore1txt, playerScore2txt, playerScore3txt, playerScore4txt;
    //game variables
    int moveTextDown = 90;
    public int enemyCount;
    public int playerCount;
    public int playerScore;
    public string playerName;
    public string name;
    public List<KeyValuePair<string, int>> playerScores = new List<KeyValuePair<string, int>>();
    public GameObject player;
    protected float x, y;
    // Use this for initialization
    void Start()
    {
        x = Screen.width;
        y = Screen.height;

        playerScoretxt.text = "Player Scores:";
        playerScore1txt.text = "";
        playerScore2txt.text = "";
        playerScore3txt.text = "";
        playerScore4txt.text = "";
        CheckEnemyAndPlayerCount();
        CheckPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyAndPlayerCount();
        GetPlayerScores();
    }
    void OnGui()
    {
        playerName = GetComponent<PlayerGUI>().playerNumber;

        GUI.Label(new Rect(x - 50, y - 40, 100, 20), playerScoretxt.text, styleHealth);
        GUI.Label(new Rect(x - 50, y - 60, 100, 20), playerScore1txt.text, styleHealth);
        GUI.Label(new Rect(x - 50, y - 80, 100, 20), playerScore2txt.text, styleHealth);
        GUI.Label(new Rect(x - 50, y - 100, 100, 20), playerScore3txt.text, styleHealth);
        GUI.Label(new Rect(x - 50, y - 120, 100, 20), playerScore4txt.text, styleHealth);
    }

    void CheckEnemyAndPlayerCount()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        players = GameObject.FindGameObjectsWithTag("Player");
        enemyCount = enemies.Length;
        playerCount = players.Length;
    }
    void GetPlayerScores()
    {
        playerScore = GetComponent<PlayerGUI>().count;
        Debug.Log("Player Name: " + playerName + " - Score: " + playerScore);

        if (playerName == "Player1")
        {
            Debug.Log("If passed for Player1 Player Name: " + playerName + " - Score: " + playerScore);
            playerScore1txt.text = playerName + " - Score: " + playerScore;
        }
        else if (playerName == "Player2")
        {
            Debug.Log("If passed for Player2 Player Name: " + playerName + " - Score: " + playerScore);
            playerScore2txt.text = playerName + " - Score: " + playerScore;
        }
        else if (playerName == "Player3")
        {
            Debug.Log("If passed for Player3 Player Name: " + playerName + " - Score: " + playerScore);
            playerScore3txt.text = playerName + " - Score: " + playerScore;
        }
        else if (playerName == "Player4")
        {
            Debug.Log("If passed for Player4 Player Name: " + playerName + " - Score: " + playerScore);
            playerScore4txt.text = playerName + " - Score: " + playerScore;
        }

        //foreach (GameObject player in players)
        //{
        //    playerName = player.GetComponent<PlayerGUI>().playerNumber;
        //   playerScore = player.GetComponent<PlayerGUI>().count;
        //
        //   Debug.Log("Player Name: " + playerName + " - Score: " + playerScore);
        // }
    }
    void CheckPlayerName()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        playerCount = players.Length;

        //check for other players and assign player numbers
            playerName = "Player" + playerCount.ToString();

    }
}
