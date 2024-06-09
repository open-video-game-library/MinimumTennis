using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private StartManager start;
    private PrepareManager prepare;
    private PlayingManager playing;
    private ReplayManager replay;
    private EndManager end;
    private PauseManager pause;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = SystemParameters.fps;

        InitializeGame();

        start = GetComponent<StartManager>();
        prepare = GetComponent<PrepareManager>();
        playing = GetComponent<PlayingManager>();
        replay = GetComponent<ReplayManager>();
        end = GetComponent<EndManager>();
        pause = GetComponent<PauseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GameData.controllable = GameData.gameState == GameState.Playing;

        switch (GameData.gameState)
        {
            case GameState.Start:
                start.Manage();
                break;
            case GameState.Prepare:
                prepare.Manage();
                break;
            case GameState.Playing:
                playing.Manage();
                break;
            case GameState.Replay:
                replay.Manage();
                break;
            case GameState.End:
                end.Manage();
                break;
        }
    }

    private void InitializeGame()
    {
        // Set default parameter to "GameData"
        GameData.character1 = CharacterGenerator.GetCharacter(0);
        GameData.character2 = CharacterGenerator.GetCharacter(1);

        GameData.gameState = GameState.Start;

        GameData.character1Score = 0;
        GameData.character2Score = 0;
        GameData.character1GameCount = 0;
        GameData.character2GameCount = 0;
        GameData.character1ScoreResult = new string[2 * (int)Parameters.gameSize - 1];
        GameData.character2ScoreResult = new string[2 * (int)Parameters.gameSize - 1];

        GameData.foul = FoulState.NoFoul;
        GameData.server = null;
        GameData.lastShooter = null;
        GameData.pointLoser = null;
        GameData.isToss = false;
        GameData.isBreaked = false;
        GameData.isServeIn = false;
        GameData.isOut = false;
        GameData.isNet = false;
        GameData.rallyCount = 0;
        GameData.ballBoundCount = 0;
        GameData.faultCount = 0;
        GameData.servePosition = ServePosition.deuce;
        GameData.ballAmount = 0;

        GameData.csv = new CSVData();

        GameData.controllable = false;
        GameData.pause = false;
    }

    // アニメーション内から呼ばれる関数
    public void SwitchGameState(GameState nextState)
    {
        GameData.gameState = nextState;
    }

    // ポーズ画面から呼ばれる関数
    public void BackHome()
    {
        InitializeGame();
        SceneManager.LoadScene("HomeScene");
    }

    // ポーズ画面から呼ばれる関数
    public void RestartGame()
    {
        pause.ExitPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
