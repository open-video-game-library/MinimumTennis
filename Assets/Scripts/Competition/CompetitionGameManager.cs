using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CompetitionGameManager : MonoBehaviour
{
    private bool updateOnce = true;

    // Variables to manage parameters to be output in CSV
    [System.NonSerialized] public string gameResult;
    [System.NonSerialized] public int playerGameNum;
    [System.NonSerialized] public int enemyGameNum;
    [System.NonSerialized] public int netNum;
    [System.NonSerialized] public int outNum;
    [System.NonSerialized] public int twoBoundNum;
    [System.NonSerialized] public int doubleFaultNum;
    [System.NonSerialized] public int rallyCount;
    [System.NonSerialized] public int maxRallyCount;

    // Object to send data to CSV
    public GameObject dataManager;
    private DataManager data;

    // Manage operations with the Pro Controller
    public GameObject proController;
    private CompetitionGameControllerManager controllerManager;

    // Score panel object
    public GameObject scorePanel;

    // Variable for storing scores
    public GameObject score;
    private Text scoreText;

    // Variable for storing referee messages
    public GameObject message;
    private Text messageText;

    // For Results Display
    public GameObject resultManager;
    private CompetitionResultManager result;

    // Player
    public GameObject player;

    // Opponent
    public GameObject enemy;

    // Number of bounces
    [System.NonSerialized] public int ballCount;

    // Fault Count
    [System.NonSerialized] public int faultCount = 0;

    // Variables that manage status
    [System.NonSerialized] public bool active = true;
    [System.NonSerialized] public string status = "SERVE";
    [System.NonSerialized] public string who;
    [System.NonSerialized] public string whoBREAK;
    [System.NonSerialized] public string server;

    // Variables that identify the state
    [System.NonSerialized] public bool isOUT;
    [System.NonSerialized] public bool isNET;
    [System.NonSerialized] public bool isTOS;
    [System.NonSerialized] public bool isBREAKED = false;
    [System.NonSerialized] public bool isSERVE = false;

    // Variables on Scores
    private int player_score = 0;
    private int enemy_score = 0;
    [System.NonSerialized] public int playerGameCount = 0;
    [System.NonSerialized] public int enemyGameCount = 0;

    // Variable that controls which way the serve is hit from
    [System.NonSerialized] public int whereServe = 1;

    // Variables for countdown
    private int nextCount;
    private int nextGameCount;

    // Array for result screen
    [System.NonSerialized] public string[] resultPlayerScore;
    [System.NonSerialized] public string[] resultEnemyScore;

    private AudioSource audioSource;
    public AudioClip crap1;
    public AudioClip crap2;

    // Start is called before the first frame update
    private void Start()
    {
        data = dataManager.GetComponent<DataManager>();
        controllerManager = proController.GetComponent<CompetitionGameControllerManager>();
        scoreText = score.GetComponentInChildren<Text>();
        messageText = message.GetComponentInChildren<Text>();
        result = resultManager.GetComponent<CompetitionResultManager>();
        resultPlayerScore = new string[2 * GameSize.gameSize - 1];
        resultEnemyScore = new string[2 * GameSize.gameSize - 1];
        resultManager.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // When on game
        if (active)
        {
            if ((playerGameCount + enemyGameCount) % 2 == 0) { server = "Player1"; }
            else if ((playerGameCount + enemyGameCount) % 2 == 1) { server = "Player2"; }

            isBREAKED = false;
            scorePanel.SetActive(false);
            nextCount = 0;
            nextGameCount = 0;
            Judgement();
            UpdateScore(player_score, enemy_score);
        }
        // When one of them scores a point.
        else if (!active && !isBREAKED)
        {
            rallyCount = 0;
            if (nextCount == 0) { audioSource.PlayOneShot(crap1); }
            nextCount++;
            messageText.text = status + " by " + who;
            scorePanel.SetActive(true);
            if (nextCount > 2 * 60) Restart();
        }
        // When one of them took the game
        else if (!active && isBREAKED)
        {
            rallyCount = 0;
            if (nextGameCount == 0) { audioSource.PlayOneShot(crap2); }
            nextGameCount++;
            messageText.text = status + " by " + who;
            scorePanel.SetActive(true);
            if (nextGameCount > 3 * 60) { GetNextGame(); }
        }

        // Erase if bounced more than 4 times
        if (ballCount > 3)
        {
            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            Destroy(ball);
        }

        // Update maximum rally count
        if (rallyCount > maxRallyCount) { maxRallyCount = rallyCount; }
    }

    private void UpdateScore(int _player, int _enemy)
    {
        // A function that returns the current score when you enter a score
        bool deuce;
        deuce = (_enemy >= 3) && (_player >= 3);

        // If both are 3 points
        if (deuce)
        {
            if (_player == _enemy) { scoreText.text = "Deuce"; }
            else if (_player > _enemy)
            {
                if (_player - _enemy == 1) { scoreText.text = "A  Å|  40"; }
                else if (_player - _enemy >= 2)
                {
                    int num = playerGameCount + enemyGameCount;
                    resultPlayerScore[num] = "Åö A";
                    resultEnemyScore[num] = "40";

                    scoreText.text = "GAME";
                    isBREAKED = true;
                    whoBREAK = "Player1";
                }
            }
            else if (_player < _enemy)
            {
                if (_enemy - _player == 1) { scoreText.text = "40  Å|  A"; }
                else if (_enemy - _player >= 2)
                {
                    int num = playerGameCount + enemyGameCount;
                    resultPlayerScore[num] = "40";
                    resultEnemyScore[num] = "A Åö";

                    scoreText.text = "GAME";
                    isBREAKED = true;
                    whoBREAK = "Player2";
                }
            }
        }
        // Otherwise
        else 
        {
            if (_player >= 4)
            {
                int num = playerGameCount + enemyGameCount;
                resultPlayerScore[num] = "Åö 40";
                resultEnemyScore[num] = Mathf.Min(_enemy * 15, 40).ToString();

                scoreText.text = "GAME";
                isBREAKED = true;
                whoBREAK = "Player1";
            }
            else if (_enemy >= 4)
            {
                int num = playerGameCount + enemyGameCount;
                resultPlayerScore[num] = Mathf.Min(_player * 15, 40).ToString();
                resultEnemyScore[num] = "40 Åö";

                scoreText.text = "GAME";
                isBREAKED = true;
                whoBREAK = "Player2";
            }
            else { scoreText.text = Mathf.Min(_player * 15, 40) + "  Å|  " + Mathf.Min(_enemy * 15, 40); }
        }
    }

    private void Judgement()
    {
        if (isNET)
        {
            active = false;
            // When Fault (Net)
            if (isSERVE)
            {
                faultCount++;

                if (faultCount == 1) { status = "FAULT"; }
                else if (faultCount == 2)
                {
                    status = "DOUBLE FAULT";
                    AddScore(who);
                    if (who == "Player1") { doubleFaultNum++; }
                }
            }
            // When Net
            else
            {
                status = "NET";
                AddScore(who);
                if (who == "Player1") { netNum++; }
            }
        }
        else if (isOUT)
        {
            active = false;
            if (isSERVE)
            {
                faultCount++;

                if (faultCount == 1) { status = "FAULT"; }
                else if (faultCount == 2)
                {
                    status = "DOUBLE FAULT";
                    AddScore(who);
                    if (who == "Player1") { doubleFaultNum++; }
                }
            }
            else
            {
                status = "OUT";
                AddScore(who);
                if (who == "Player1") { outNum++; }
            }
        }
        else if (ballCount >= 2)
        {
            active = false;
            status = "TWO-BOUNDS";
            if (who == "Player1") who = "Player2";
            else if (who == "Player2") who = "Player1";
            AddScore(who);
            if (who == "Player1") { twoBoundNum++; }
        }
    }

    private void Restart()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Destroy(ball);

        isOUT = false;
        isNET = false;

        ballCount = 0;
        if (status != "FAULT") { faultCount = 0; }
        status = "SERVE";

        if ((player_score + enemy_score) % 2 == 0) { whereServe = 1; }
        else if ((player_score + enemy_score) % 2 == 1) { whereServe = -1; }

        player.transform.Translate(49.0f - player.transform.position.x, 0.0f, 8.0f * whereServe - player.transform.position.z);
        enemy.transform.Translate(enemy.transform.position.x + 49.0f, 0.0f, enemy.transform.position.z + 8.0f * whereServe);

        for (int i = 0; i < controllerManager.gamepadCount; i++) { controllerManager.isPressedEast[i] = false; }
        active = true;
    }

    private void GetNextGame()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Destroy(ball);

        if (playerGameCount == GameSize.gameSize)
        {
            scorePanel.SetActive(false);
            result.DrawResult("Player1");
            gameResult = "win";
            playerGameNum = playerGameCount;
            enemyGameNum = enemyGameCount;

            if (updateOnce)
            {
                // Process to save to CSV
                data.postData(gameResult, playerGameNum, enemyGameNum,
                              netNum, outNum, twoBoundNum, doubleFaultNum, maxRallyCount);
                updateOnce = false;
            }
        }
        else if (enemyGameCount == GameSize.gameSize)
        {
            scorePanel.SetActive(false);
            result.DrawResult("Player2");
            gameResult = "lose";
            playerGameNum = playerGameCount;
            enemyGameNum = enemyGameCount;

            if (updateOnce)
            {
                // Process to save to CSV
                data.postData(gameResult, playerGameNum, enemyGameNum,
                              netNum, outNum, twoBoundNum, doubleFaultNum, maxRallyCount);
                updateOnce = false;
            }
        }
        else
        {
            isOUT = false;
            isNET = false;

            player_score = 0;
            enemy_score = 0;

            ballCount = 0;
            faultCount = 0;
            status = "SERVE";

            if ((player_score + enemy_score) % 2 == 0) { whereServe = 1; }
            else if ((player_score + enemy_score) % 2 == 1) { whereServe = -1; }

            player.transform.Translate(49.0f - player.transform.position.x, 0.0f, 8.0f * whereServe - player.transform.position.z);
            enemy.transform.Translate(enemy.transform.position.x + 49.0f, 0.0f, enemy.transform.position.z + 8.0f * whereServe);

            for (int i = 0; i < controllerManager.gamepadCount; i++) { controllerManager.isPressedEast[i] = false; }
            active = true;
        }
    }

    public void AddScore(string _who)
    {
        if (_who == "Player2") { player_score++; }
        else if (_who == "Player1") { enemy_score++; }
    }

    public void BallBound()
    {
        ballCount++;
    }

    public void BallOut()
    {
        if (active && ballCount == 0) { isOUT = true; }
    }

    public void BallNet()
    {
        if (active) { isNET = true; }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool Check(string _tagName)
    {
        GameObject[] _tagObjects = GameObject.FindGameObjectsWithTag(_tagName);
        if (_tagObjects.Length == 0) { return true; }
        else { return false; }
    }
}
