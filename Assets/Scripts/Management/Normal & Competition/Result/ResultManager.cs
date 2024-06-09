using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    // 各プレイヤの名前
    public static string character1Name = "Character 1";
    public static string character2Name = "Character 2";

    // 何ゲーム先取かを格納
    public static GameSize gameSize = GameSize.nineGames;

    // 実際に何ゲーム行ったかを格納
    public static int gameAmount = 6;

    // 各プレイヤの各ゲームにおける獲得得点
    public static string[] character1ScoreResult = { "15", "A", "40", "A", "40" ,"40"};
    public static string[] character2ScoreResult = { "40", "D", "0", "0" , "0" , "0" };

    // 各プレイヤの獲得ゲーム数
    public static int character1GameCount = 5;
    public static int character2GameCount = 1;

    // 遷移してきたシーンの名前
    public static string previousSceneName;

    // 各プレイヤの獲得ゲームの推移を描画するクラス
    [SerializeField]
    private GameResultDrawer gameResultDrawer;

    // 各プレイヤの最終的な獲得ゲーム数と勝利メッセージを描画するクラス
    [SerializeField]
    private FinalResultDrawer finalResultDrawer;

    private Animator animator;

    // CSVにデータをセットしたかを管理する変数
    private bool isSetCSVData = false;

    IEnumerator Start()
    {
        Application.targetFrameRate = SystemParameters.fps;

        animator = GetComponent<Animator>();

        // 勝者の名前を格納
        string winner = DecideWinner(character1GameCount, character2GameCount);

        if (!isSetCSVData)
        {
            // ゲームデータをCSVファイルにセット
            RecordData(winner);
            CSVDataManager.SetData(GameData.csv);
            isSetCSVData = true;
        }

        // 各プレイヤの獲得ゲームの推移を描画する
        yield return StartCoroutine(gameResultDrawer.DrawGameResult(character1Name, character2Name, gameAmount, character1ScoreResult, character2ScoreResult));

        // 演出のため、0.5秒待機する
        yield return new WaitForSecondsRealtime(0.50f);

        // 各プレイヤの最終的な獲得ゲーム数と勝利メッセージを描画する
        finalResultDrawer.DrawFinalResult(character1GameCount, character2GameCount, winner);

        animator.SetTrigger("DrawResult");
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(previousSceneName);
    }

    public void GetCSV()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) { CSVDataManager.GetData(); }
        else { CSVDataManager.ExportCSV(); }
    }

    private string DecideWinner(int character1GameCount, int character2GameCount)
    {
        string winner = "";

        if (character1GameCount == (int)gameSize) { winner = character1Name; }
        else if (character2GameCount == (int)gameSize) { winner = character2Name; }

        return winner;
    }

    private void RecordData(string winnerName)
    {
        if (GameData.csv == null) { return; }

        GameData.csv.winner = winnerName;
        GameData.csv.character1GameCount = GameData.character1GameCount;
        GameData.csv.character2GameCount = GameData.character2GameCount;
    }
}
