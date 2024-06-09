using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameResultDrawer : MonoBehaviour
{
    // 各プレイヤの名前を描画するテキスト
    [SerializeField]
    private TMP_Text character1NameText;
    [SerializeField]
    private TMP_Text character2NameText;

    // 各プレイヤの獲得ゲームの推移を描画するゲームオブジェクト
    [SerializeField]
    private GameObject gameResultScorePanel;

    // 各プレイヤが各ゲームで獲得した点数を描画するためのPrefab
    [SerializeField]
    private GameObject gameScorePrefab;

    private GameObject[] gameScores;

    public IEnumerator DrawGameResult(string character1Name, string character2Name, 
        int gameAmount, 
        string[] character1ScoreResult, string[] character2ScoreResult)
    {
        character1NameText.text = character1Name;
        character2NameText.text = character2Name;

        gameScores = new GameObject[gameAmount];
        GameScoreDrawer[] gameScoreDrawers = new GameScoreDrawer[gameAmount];

        for (int i = 0; i < gameAmount; i++)
        {
            gameScores[i] = Instantiate(gameScorePrefab);
            gameScores[i].transform.SetParent(gameResultScorePanel.transform);
            gameScores[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            gameScoreDrawers[i] = gameScores[i].GetComponent<GameScoreDrawer>();
        }

        for (int i = 0; i < gameAmount; i++)
        {
            gameScoreDrawers[i].DrawGameScore(character1ScoreResult[i], character2ScoreResult[i], i + 1);
            yield return new WaitForSecondsRealtime(0.50f); 
        }
    }
}
