using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool inPlay;
    public static string server;
    public static string lastShooter;
    public static string pointLoser;
    public static string foul;
    public static bool isToss;
    public static bool isBreaked;
    public static bool isServeIn;
    public static int rallyCount;
    public static int ballBoundCount;
    public static int servePosition;
    public static int ballAmount;

    [SerializeField]
    private GameObject character1;
    [SerializeField]
    private GameObject character2;
    public static string character1Name;
    public static string character2Name;
    public static int character1Score;
    public static int character2Score;
    public static int character1GameCount;
    public static int character2GameCount;
    public static string[] character1ScoreResult;
    public static string[] character2ScoreResult;

    public static readonly Vector3 courtAreaBegin = new Vector3(-47.54f, 0.0f, -16.8f);
    public static readonly Vector3 courtAreaEnd = new Vector3(47.54f, 0.0f, 16.8f);
    public static readonly float netHight = 4.0f;

    private bool isOut = false;
    private bool isNet = false;
    
    private int faultCount = 0;

    private float nextCount = 0.0f;
    private float nextGameCount = 0.0f;
    private bool isSetCSVData = false;

    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private GameObject csvDataManager;
    private CSVDataSetter csv;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip crap;
    [SerializeField]
    private AudioClip cheer;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        inPlay = true;
        server = null;
        lastShooter = null;
        pointLoser = null;
        foul = "NO FOUL";
        isToss = false;
        isBreaked = false;
        isServeIn = false;
        rallyCount = 0;
        ballBoundCount = 0;
        servePosition = 1;
        ballAmount = 0;

        character1Name = character1.name;
        character2Name = character2.name;
        character1Score = 0;
        character2Score = 0;
        character1GameCount = 0;
        character2GameCount = 0;
        character1ScoreResult = new string[2 * GameSize.gameSize - 1];
        character2ScoreResult = new string[2 * GameSize.gameSize - 1];

        csv = csvDataManager.GetComponent<CSVDataSetter>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((character1GameCount + character2GameCount) % 2 == 0) { server = character1.name; }
        else if ((character1GameCount + character2GameCount) % 2 == 1) { server = character2.name; }

        if (inPlay)
        {
            JudgeFoul();
            if (scorePanel.activeSelf) { scorePanel.SetActive(false); }
        }
        else
        {
            rallyCount = 0;
            if (isBreaked)
            {
                if ((int)nextGameCount >= 3) { GetNextGame(); }
                else
                {
                    if (!scorePanel.activeSelf) { scorePanel.SetActive(true); }
                    nextGameCount += Time.deltaTime;
                }
            }
            else
            {
                if ((int)nextCount >= 2) { Restart(); }
                else
                {
                    if (!scorePanel.activeSelf) { scorePanel.SetActive(true); }
                    nextCount += Time.deltaTime;
                }
            }
        }
        //　ラリーの最大回数を更新する
        if (rallyCount > csv.maxRallyCount) { csv.maxRallyCount = rallyCount; }

        ballAmount = CheckBallAmount();
    }

    private static int CheckBallAmount()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        return balls.Length;
    }

    private void JudgeFoul()
    {
        pointLoser = lastShooter;

        if (isNet)
        {
            if (!isServeIn)
            {
                faultCount++;
                if (faultCount == 1)
                {
                    foul = "FAULT";
                    inPlay = false;
                }
                else if (faultCount == 2)
                {
                    foul = "DOUBLE FAULT";
                    if (pointLoser == character1.name)
                    {
                        AddScore(character2.name);
                        csv.character1DoubleFaultCount++;
                    }
                    else if (pointLoser == character2.name)
                    {
                        AddScore(character1.name);
                        csv.character2DoubleFaultCount++;
                    }
                }
            }
            else
            {
                foul = "NET";
                if (pointLoser == character1.name)
                {
                    AddScore(character2.name);
                    csv.character1NetCount++;
                }
                else if (pointLoser == character2.name)
                {
                    AddScore(character1.name);
                    csv.character2NetCount++;
                }
            }
        }
        else if (isOut)
        {
            if (!isServeIn)
            {
                faultCount++;
                if (faultCount == 1)
                {
                    foul = "FAULT";
                    inPlay = false;
                }
                else if (faultCount == 2)
                {
                    foul = "DOUBLE FAULT";
                    if (pointLoser == character1.name)
                    {
                        AddScore(character2.name);
                        csv.character1DoubleFaultCount++;
                    }
                    else if (pointLoser == character2.name)
                    {
                        AddScore(character1.name);
                        csv.character2DoubleFaultCount++;
                    }
                }
            }
            else
            {
                foul = "OUT";
                if (pointLoser == character1.name)
                {
                    AddScore(character2.name);
                    csv.character1OutCount++;
                }
                else if (pointLoser == character2.name)
                {
                    AddScore(character1.name);
                    csv.character2OutCount++;
                }
            }
        }
        else if (ballBoundCount >= 2)
        {
            foul = "TWO-BOUNDS";
            if (pointLoser == character1.name) { pointLoser = character2.name; }
            else if (pointLoser == character2.name) { pointLoser = character1.name; }

            if (pointLoser == character1.name)
            {
                AddScore(character2.name);
                csv.character1TwoBoundCount++;
            }
            else if (pointLoser == character2.name)
            {
                AddScore(character1.name);
                csv.character2TwoBoundCount++;
            }
        }
        JudgeGameBreaked();
    }

    private void JudgeGameBreaked()
    {
        if (isBreaked) { return; }

        bool deuce = (character2Score >= 3) && (character1Score >= 3);
        if (deuce)
        {
            if (character1Score - character2Score >= 2)
            {
                int num = character1GameCount + character2GameCount;
                character1ScoreResult[num] = "★ A";
                character2ScoreResult[num] = "40";

                isBreaked = true;
                AddGameCount(character1.name);
            }
            if (character2Score - character1Score >= 2)
            {
                int num = character1GameCount + character2GameCount;
                character1ScoreResult[num] = "40";
                character2ScoreResult[num] = "A ★";

                isBreaked = true;
                AddGameCount(character2.name);
            }
        }
        else
        {
            if (character1Score >= 4)
            {
                int num = character1GameCount + character2GameCount;
                character1ScoreResult[num] = "★ 40";
                character2ScoreResult[num] = Mathf.Min(character2Score * 15, 40).ToString();

                isBreaked = true;
                AddGameCount(character1.name);
            }
            else if (character2Score >= 4)
            {
                int num = character1GameCount + character2GameCount;
                character1ScoreResult[num] = Mathf.Min(character1Score * 15, 40).ToString();
                character2ScoreResult[num] = "40 ★";

                isBreaked = true;
                AddGameCount(character2.name);
            }
        }
    }

    private void Restart()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Destroy(ball);

        lastShooter = null;
        if (isServeIn || faultCount >= 2) { faultCount = 0; }
        foul = "NO FOUL";
        isOut = false;
        isNet = false;
        isServeIn = false;
        ballBoundCount = 0;

        if ((character1Score + character2Score) % 2 == 0) { servePosition = 1; }
        else if ((character1Score + character2Score) % 2 == 1) { servePosition = -1; }

        character1.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(49.0f, 3.5f, 8.0f * servePosition));
        character2.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(-49.0f, 3.5f, -8.0f * servePosition));

        nextCount = 0;
        inPlay = true;
    }

    private void GetNextGame()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Destroy(ball);

        if (character1GameCount == GameSize.gameSize)
        {
            csv.winner = character1.name;
            csv.character1GameCount = character1GameCount;
            csv.character2GameCount = character2GameCount;

            if (scorePanel.activeSelf) { scorePanel.SetActive(false); }
            if (!resultPanel.activeSelf) { resultPanel.SetActive(true); }

            if (!isSetCSVData)
            {
                csv.SetCSVData();
                isSetCSVData = true;
            }
        }
        else if (character2GameCount == GameSize.gameSize)
        {
            csv.winner = character2.name;
            csv.character1GameCount = character1GameCount;
            csv.character2GameCount = character2GameCount;

            if (scorePanel.activeSelf) { scorePanel.SetActive(false); }
            if (!resultPanel.activeSelf) { resultPanel.SetActive(true); }

            if (!isSetCSVData)
            {
                csv.SetCSVData();
                isSetCSVData = true;
            }
        }
        else
        {
            lastShooter = null;
            if (isServeIn || faultCount >= 2) { faultCount = 0; }
            foul = "NO FOUL";
            isOut = false;
            isNet = false;
            isServeIn = false;
            ballBoundCount = 0;

            character1Score = 0;
            character2Score = 0;

            if ((character1Score + character2Score) % 2 == 0) { servePosition = 1; }
            else if ((character1Score + character2Score) % 2 == 1) { servePosition = -1; }

            character1.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(49.0f, 3.5f, 8.0f * servePosition));
            character2.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(-49.0f, 3.5f, -8.0f * servePosition));

            nextGameCount = 0;
            isBreaked = false;
            inPlay = true;
        }
    }

    public void AddScore(string winner)
    {
        inPlay = false;
        if (winner == character1.name)
        {
            character1Score++;
            audioSource.PlayOneShot(crap);
        }
        else if (winner == character2.name) { character2Score++; }
    }

    private void AddGameCount(string winner)
    {
        inPlay = false;
        if (character1GameCount >= GameSize.gameSize || character2GameCount >= GameSize.gameSize) { return; }
        if (winner == character1.name)
        {
            character1GameCount++;
            audioSource.PlayOneShot(cheer);
        }
        else if (winner == character2.name) { character2GameCount++; }
    }

    public void ReportBallOut()
    {
        // GameManagerにアウトであることを報告するための関数
        if (inPlay && ballBoundCount == 0) { isOut = true; }
    }

    public void ReportBallNet()
    {
        // GameManagerにネットであることを報告するための関数
        if (inPlay) { isNet = true; }
    }

    public void Retry()
    {
        // このシーンをリロードする
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
