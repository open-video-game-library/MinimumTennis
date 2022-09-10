using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetitionResultManager : MonoBehaviour
{
    public GameObject gameManager;
    private CompetitionGameManager manager;

    public GameObject scorePanel;

    public GameObject winnerText;
    private Text winner;

    public GameObject resultText;
    private Text result;

    public GameObject playerResultText;
    private Text playerResult;

    public GameObject enemyResultText;
    private Text enemyResult;

    private bool drawOnce = true;

    public void DrawResult(string _winner)
    {
        if (drawOnce)
        {
            manager = gameManager.GetComponent<CompetitionGameManager>();
            winner = winnerText.GetComponentInChildren<Text>();
            result = resultText.GetComponentInChildren<Text>();
            playerResult = playerResultText.GetComponentInChildren<Text>();
            enemyResult = enemyResultText.GetComponentInChildren<Text>();

            scorePanel.SetActive(false);
            gameObject.SetActive(true);
            manager.active = false;

            winner.text = _winner + " Win";
            result.text = null;
            playerResult.text = null;
            enemyResult.text = null;

            for (int i = 0; i < 2 * GameSize.gameSize - 1; i++)
            {
                if (manager.resultPlayerScore[i] == null || manager.resultEnemyScore[i] == null) { return; }
                result.text += "-" + "\n";
                playerResult.text += manager.resultPlayerScore[i] + "\n";
                enemyResult.text += manager.resultEnemyScore[i] + "\n";
            }
            drawOnce = false;
        }
    }
}
