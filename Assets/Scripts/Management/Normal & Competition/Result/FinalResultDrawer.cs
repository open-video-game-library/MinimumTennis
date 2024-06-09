using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalResultDrawer : MonoBehaviour
{
    // 各プレイヤの獲得ゲーム数を描画するテキスト
    [SerializeField]
    private TMP_Text pointText;

    // 勝利したプレイヤの名前を描画するテキスト
    [SerializeField]
    private TMP_Text winnerNameText;

    public void DrawFinalResult(int character1GameCount, int character2GameCount, string winnerName)
    {
        pointText.text = character1GameCount + "-" + character2GameCount;
        winnerNameText.text = winnerName;
    }
}
