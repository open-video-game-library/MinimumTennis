using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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
    private ProControllerManager controllerManager;

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
    private ResultManager result;

    // Player
    public GameObject player;
    private PlayerController playerController;

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
    void Start()
    {
        data = dataManager.GetComponent<DataManager>();
        controllerManager = proController.GetComponent<ProControllerManager>();
        scoreText = score.GetComponentInChildren<Text>();
        messageText = message.GetComponentInChildren<Text>();
        result = resultManager.GetComponent<ResultManager>();
        playerController = player.GetComponent<PlayerController>();
        resultPlayerScore = new string[2 * GameSize.gameSize - 1];
        resultEnemyScore = new string[2 * GameSize.gameSize - 1];
        resultManager.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // When on game
        if (active)
        {
            if ((playerGameCount + enemyGameCount) % 2 == 0) { server = "Player"; }
            else if ((playerGameCount + enemyGameCount) % 2 == 1) { server = "Enemy"; }

            isBREAKED = false;
            scorePanel.SetActive(false);
            nextCount = 0;
            nextGameCount = 0;
            Judgement();
            UpdateScore(player_score, enemy_score);
        }
        // When one of them scores a point
        else if (!active && !isBREAKED)
        {
            rallyCount = 0;
            if (nextCount == 0 && who == "Opponent") { audioSource.PlayOneShot(crap1); }
            nextCount++;
            messageText.text = status + " by " + who;
            scorePanel.SetActive(true);
            if (nextCount > 2 * 60) Restart();
        }
        // When one of them took the game
        else if (!active && isBREAKED) 
        {
            rallyCount = 0;
            if (nextGameCount == 0 && who == "Opponent") { audioSource.PlayOneShot(crap2); }
            nextGameCount++;
            messageText.text = status + " by " + who;
            scorePanel.SetActive(true);
            if (nextGameCount > 3 * 60) { GetNextGame(); }
        }

        // Erase the ball if bounced more than 4 times
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
                    whoBREAK = "Player";
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
                    whoBREAK = "Enemy";
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
                whoBREAK = "Player";
            }
            else if (_enemy >= 4)
            {
                int num = playerGameCount + enemyGameCount;
                resultPlayerScore[num] = Mathf.Min(_player * 15, 40).ToString();
                resultEnemyScore[num] = "40 Åö";

                scoreText.text = "GAME";
                isBREAKED = true;
                whoBREAK = "Enemy";
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
                    if (who == "Player") { doubleFaultNum++; }
                }
            }
            // When Net
            else
            {
                status = "NET";
                AddScore(who);
                if (who == "Player") { netNum++; }
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
                    if (who == "Player") { doubleFaultNum++; }
                }
            }
            else
            {
                status = "OUT";
                AddScore(who);
                if (who == "Player") { outNum++; }
            }
        }
        else if (ballCount >= 2)
        {
            active = false;
            status = "TWO-BOUNDS";
            if (who == "Player") who = "Opponent";
            else if (who == "Opponent") who = "Player";
            AddScore(who);
            if (who == "Player") { twoBoundNum++; }
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
        playerController.enemyMovePoint.z = -8.0f * whereServe;
        playerController.enemyMovePoint.x = -49.0f;
        enemy.transform.Translate(enemy.transform.position.x + 49.0f, 0.0f, enemy.transform.position.z + 8.0f * whereServe);

        controllerManager.isPressedEast = false;
        active = true;
    }

    public void GetNextGame()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Destroy(ball);

        if (playerGameCount == GameSize.gameSize)
        {
            scorePanel.SetActive(false);
            result.DrawResult("Player");
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
            result.DrawResult("Opponent");
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

            player.transform.Translate(49 - player.transform.position.x, 0.0f, 8.0f * whereServe - player.transform.position.z);
            playerController.enemyMovePoint.z = -8.0f * whereServe;
            playerController.enemyMovePoint.x = -49.0f;
            enemy.transform.Translate(enemy.transform.position.x + 49.0f, 0.0f, enemy.transform.position.z + 8.0f * whereServe);

            controllerManager.isPressedEast = false;
            active = true;
        }
    }

    public void AddScore(string _who)
    {
        if (_who == "Opponent") { player_score++; }
        else if (_who == "Player") { enemy_score++; }
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
}
