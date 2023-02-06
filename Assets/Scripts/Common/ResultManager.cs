using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private GameObject winner;
    [SerializeField]
    private GameObject result;
    [SerializeField]
    private GameObject character1Result;
    [SerializeField]
    private GameObject character2Result;

    private Text winnerText;
    private Text resultText;
    private Text character1ResultText;
    private Text character2ResultText;

    void Awake()
    {
        winnerText = winner.GetComponentInChildren<Text>();
        resultText = result.GetComponentInChildren<Text>();
        character1ResultText = character1Result.GetComponentInChildren<Text>();
        character2ResultText = character2Result.GetComponentInChildren<Text>();
    }

    void OnEnable()
    {
        if (GameManager.character1GameCount == GameSize.gameSize) { DrawResult(GameManager.character1Name); }
        else if (GameManager.character2GameCount == GameSize.gameSize) { DrawResult(GameManager.character2Name); }
    }

    private void DrawResult(string gameWinner)
    {
        winnerText.text = gameWinner + " Win";
        resultText.text = null;
        character1ResultText.text = null;
        character2ResultText.text = null;

        for (int i = 0; i < 2 * GameSize.gameSize - 1; i++)
        {
            if (GameManager.character1ScoreResult[i] == null || GameManager.character2ScoreResult[i] == null) { return; }
            resultText.text += "-" + "\n";
            character1ResultText.text += GameManager.character1ScoreResult[i] + "\n";
            character2ResultText.text += GameManager.character2ScoreResult[i] + "\n";
        }
    }
}
