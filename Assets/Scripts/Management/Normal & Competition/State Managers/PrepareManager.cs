using UnityEngine;

public class PrepareManager : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreObject;
    [SerializeField]
    private ScorePanelManager scorePanelManager;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        scoreObject.SetActive(false);
    }

    public void Manage()
    {
        if (GameData.isBreaked)
        {
            if (!scoreObject.activeSelf)
            {
                scoreObject.SetActive(true);
                animator.SetBool("GameCountUpdate", true);
            }
        }
        else
        {
            if (!scoreObject.activeSelf)
            {
                scoreObject.SetActive(true);
                animator.SetBool("ScoreUpdate", true);
            }
        }
    }

    // アニメーション内から呼ばれる関数
    private void UpdateScoreInformation()
    {
        scorePanelManager.CalculateScore();
    }

    // アニメーション内から呼ばれる関数
    private void Restart()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Destroy(ball);

        bool callReplay = GameData.foul != FoulState.Fault && GameData.foul != FoulState.DoubleFault;

        if (GameData.isServeIn || GameData.faultCount >= 2) { GameData.faultCount = 0; }

        GameData.lastShooter = null;
        GameData.foul = FoulState.NoFoul;
        GameData.isOut = false;
        GameData.isNet = false;
        GameData.isServeIn = false;
        GameData.ballBoundCount = 0;

        if ((GameData.character1Score + GameData.character2Score) % 2 == 0) { GameData.servePosition = ServePosition.deuce; }
        else if ((GameData.character1Score + GameData.character2Score) % 2 == 1) { GameData.servePosition = ServePosition.advantage; }

        animator.SetBool("ScoreUpdate", false);
        scoreObject.SetActive(false);

        if (callReplay) { GameData.gameState = GameState.Replay; }
        else
        {
            GameData.character1.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(8.0f * (int)GameData.servePosition, 0.0f, -49.0f));
            GameData.character2.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(-8.0f * (int)GameData.servePosition, 0.0f, 49.0f));

            GameData.gameState = GameState.Playing;
        }

        Debug.Log("Restart()");
    }

    // アニメーション内から呼ばれる関数
    private void GetNextGame()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        Destroy(ball);

        bool callReplay = GameData.foul != FoulState.Fault && GameData.foul != FoulState.DoubleFault;

        if (GameData.character1GameCount == (int)Parameters.gameSize || GameData.character2GameCount == (int)Parameters.gameSize)
        {
            animator.SetBool("GameCountUpdate", false);
            scoreObject.SetActive(false);
            GameData.isBreaked = false;

            if (callReplay) { GameData.gameState = GameState.Replay; }
            else { GameData.gameState = GameState.End; }
        }
        else
        {
            if (GameData.isServeIn || GameData.faultCount >= 2) { GameData.faultCount = 0; }

            GameData.lastShooter = null;
            GameData.foul = FoulState.NoFoul;
            GameData.isOut = false;
            GameData.isNet = false;
            GameData.isServeIn = false;
            GameData.ballBoundCount = 0;

            GameData.character1Score = 0;
            GameData.character2Score = 0;
            scorePanelManager.character1ScoreText.text = "0";
            scorePanelManager.character2ScoreText.text = "0";
            scorePanelManager.currentCharacter1Score = "0";
            scorePanelManager.currentCharacter2Score = "0";

            if ((GameData.character1Score + GameData.character2Score) % 2 == 0) { GameData.servePosition = ServePosition.deuce; }
            else if ((GameData.character1Score + GameData.character2Score) % 2 == 1) { GameData.servePosition = ServePosition.advantage; }

            animator.SetBool("GameCountUpdate", false);
            scoreObject.SetActive(false);
            GameData.isBreaked = false;

            if (callReplay) { GameData.gameState = GameState.Replay; }
            else
            {
                GameData.character1.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(8.0f * (int)GameData.servePosition, 0.0f, -49.0f));
                GameData.character2.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(-8.0f * (int)GameData.servePosition, 0.0f, 49.0f));

                GameData.gameState = GameState.Playing;
            }
        }

        Debug.Log("GetNextGame()");
    }
}
