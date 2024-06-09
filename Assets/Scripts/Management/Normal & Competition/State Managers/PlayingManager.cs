using UnityEngine;

public class PlayingManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip crap;
    [SerializeField]
    private AudioClip cheer;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Manage()
    {
        if ((GameData.character1GameCount + GameData.character2GameCount) % 2 == 0) { GameData.server = GameData.character1.name; }
        else if ((GameData.character1GameCount + GameData.character2GameCount) % 2 == 1) { GameData.server = GameData.character2.name; }

        JudgeFoul();

        GameData.ballAmount = CheckBallAmount();

        if (GameData.rallyCount > GameData.csv.maxRallyCount) { GameData.csv.maxRallyCount = GameData.rallyCount; }
    }

    private void JudgeFoul()
    {
        GameData.pointLoser = GameData.lastShooter;

        if (GameData.isNet)
        {
            if (!GameData.isServeIn)
            {
                GameData.faultCount++;
                if (GameData.faultCount == 1)
                {
                    GameData.foul = FoulState.Fault;
                    GameData.rallyCount = 0;
                    GameData.gameState = GameState.Prepare;
                }
                else if (GameData.faultCount == 2)
                {
                    GameData.foul = FoulState.DoubleFault;
                    if (GameData.pointLoser == GameData.character1.name)
                    {
                        AddScore(GameData.character2.name);
                        GameData.csv.character1DoubleFaultCount++;
                    }
                    else if (GameData.pointLoser == GameData.character2.name)
                    {
                        AddScore(GameData.character1.name);
                        GameData.csv.character2DoubleFaultCount++;
                    }
                }
            }
            else
            {
                GameData.foul = FoulState.Net;
                if (GameData.pointLoser == GameData.character1.name)
                {
                    AddScore(GameData.character2.name);
                    GameData.csv.character1NetCount++;
                }
                else if (GameData.pointLoser == GameData.character2.name)
                {
                    AddScore(GameData.character1.name);
                    GameData.csv.character2NetCount++;
                }
            }
        }
        else if (GameData.isOut)
        {
            if (!GameData.isServeIn)
            {
                GameData.faultCount++;
                if (GameData.faultCount == 1)
                {
                    GameData.foul = FoulState.Fault;
                    GameData.rallyCount = 0;
                    GameData.gameState = GameState.Prepare;
                }
                else if (GameData.faultCount == 2)
                {
                    GameData.foul = FoulState.DoubleFault;
                    if (GameData.pointLoser == GameData.character1.name)
                    {
                        AddScore(GameData.character2.name);
                        GameData.csv.character1DoubleFaultCount++;
                    }
                    else if (GameData.pointLoser == GameData.character2.name)
                    {
                        AddScore(GameData.character1.name);
                        GameData.csv.character2DoubleFaultCount++;
                    }
                }
            }
            else
            {
                GameData.foul = FoulState.Out;
                if (GameData.pointLoser == GameData.character1.name)
                {
                    AddScore(GameData.character2.name);
                    GameData.csv.character1OutCount++;
                }
                else if (GameData.pointLoser == GameData.character2.name)
                {
                    AddScore(GameData.character1.name);
                    GameData.csv.character2OutCount++;
                }
            }
        }
        else if (GameData.ballBoundCount >= 2)
        {
            GameData.foul = FoulState.TwoBounds;
            if (GameData.pointLoser == GameData.character1.name) { GameData.pointLoser = GameData.character2.name; }
            else if (GameData.pointLoser == GameData.character2.name) { GameData.pointLoser = GameData.character1.name; }

            if (GameData.pointLoser == GameData.character1.name)
            {
                AddScore(GameData.character2.name);
                GameData.csv.character1TwoBoundCount++;
            }
            else if (GameData.pointLoser == GameData.character2.name)
            {
                AddScore(GameData.character1.name);
                GameData.csv.character2TwoBoundCount++;
            }
        }
        JudgeGameBreaked();
    }

    private void JudgeGameBreaked()
    {
        if (GameData.isBreaked) { return; }

        bool deuce = (GameData.character2Score >= 3) && (GameData.character1Score >= 3);
        if (deuce)
        {
            if (GameData.character1Score - GameData.character2Score >= 2)
            {
                int num = GameData.character1GameCount + GameData.character2GameCount;
                GameData.character1ScoreResult[num] = "A";
                GameData.character2ScoreResult[num] = "40";

                GameData.isBreaked = true;
                AddGameCount(GameData.character1.name);
            }
            if (GameData.character2Score - GameData.character1Score >= 2)
            {
                int num = GameData.character1GameCount + GameData.character2GameCount;
                GameData.character1ScoreResult[num] = "40";
                GameData.character2ScoreResult[num] = "A";

                GameData.isBreaked = true;
                AddGameCount(GameData.character2.name);
            }
        }
        else
        {
            if (GameData.character1Score >= 4)
            {
                int num = GameData.character1GameCount + GameData.character2GameCount;
                GameData.character1ScoreResult[num] = "40";
                GameData.character2ScoreResult[num] = Mathf.Min(GameData.character2Score * 15, 40).ToString();

                GameData.isBreaked = true;
                AddGameCount(GameData.character1.name);
            }
            else if (GameData.character2Score >= 4)
            {
                int num = GameData.character1GameCount + GameData.character2GameCount;
                GameData.character1ScoreResult[num] = Mathf.Min(GameData.character1Score * 15, 40).ToString();
                GameData.character2ScoreResult[num] = "40";

                GameData.isBreaked = true;
                AddGameCount(GameData.character2.name);
            }
        }
    }

    private void AddScore(string winner)
    {
        if (winner == GameData.character1.name)
        {
            GameData.character1Score++;
            audioSource.PlayOneShot(crap);
        }
        else if (winner == GameData.character2.name)
        {
            GameData.character2Score++;
            if (Parameters.playMode == PlayMode.competition) { audioSource.PlayOneShot(crap); }
        }

        GameData.rallyCount = 0;
        GameData.gameState = GameState.Prepare;
    }

    private void AddGameCount(string winner)
    {
        if (GameData.character1GameCount >= (int)Parameters.gameSize || GameData.character2GameCount >= (int)Parameters.gameSize) { return; }
        if (winner == GameData.character1.name)
        {
            GameData.character1GameCount++;
            audioSource.PlayOneShot(cheer);
        }
        else if (winner == GameData.character2.name)
        {
            GameData.character2GameCount++;
            if (Parameters.playMode == PlayMode.competition) { audioSource.PlayOneShot(cheer); }
        }

        GameData.rallyCount = 0;
        GameData.gameState = GameState.Prepare;
    }

    private int CheckBallAmount()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        return balls.Length;
    }
}
