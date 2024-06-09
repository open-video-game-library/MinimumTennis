using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScoreDrawer : MonoBehaviour
{
    // ŠeƒvƒŒƒCƒ„‚ÌŠl“¾‚µ‚½“_”
    [SerializeField]
    private TMP_Text character1ScoreText;
    [SerializeField]
    private TMP_Text character2ScoreText;

    // ‰½”Ô–Ú‚ÌƒQ[ƒ€‚©‚ğŠi”[
    [SerializeField]
    private TMP_Text gameNumberText;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DrawGameScore(string character1Score, string character2Score, int gameNumber)
    {
        character1ScoreText.text = character1Score;
        character2ScoreText.text = character2Score;
        gameNumberText.text = CovertIntOrdinalNum(gameNumber);

        if (CovertScoreInt(character1Score) > CovertScoreInt(character2Score)) { animator.SetTrigger("SlideLeft"); }
        else  { animator.SetTrigger("SlideRight"); }
    }

    private string CovertIntOrdinalNum(int intNum)
    {
        return intNum switch
        {
            1 => "1st",
            2 => "2nd",
            3 => "3rd",
            _ => intNum + "th"
        };
    }

    private int CovertScoreInt(string score)
    {
        return score switch
        {
            "A" => 60,
            "D" => 50,
            "40" => 40,
            "30" => 30,
            "15" => 15,
            _ => 0
        };
    }
}
