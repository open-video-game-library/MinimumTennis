using UnityEngine;
using TMPro;

public class ScorePanelManager : MonoBehaviour
{
    public TMP_Text character1ScoreText;
    public TMP_Text character2ScoreText;

    [SerializeField]
    private TMP_Text foulMessageText;
    [SerializeField]
    private TMP_Text character1NameText;
    [SerializeField]
    private TMP_Text character2NameText;

    [System.NonSerialized]
    public string currentCharacter1Score;
    [System.NonSerialized]
    public string currentCharacter2Score;

    private Animator animator;

    void Start()
    {
        character1NameText.text = GameData.character1.name;
        character2NameText.text = GameData.character2.name;

        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        UpdateMessage(GameData.foul, GameData.pointLoser);
    }

    public void CalculateScore()
    {
        bool deuce = (GameData.character1Score >= 3) && (GameData.character2Score >= 3);
        if (deuce)
        {
            if (GameData.character1Score == GameData.character2Score)
            {
                currentCharacter1Score = "D";
                currentCharacter2Score = "D";
            }
            else if (GameData.character1Score > GameData.character2Score 
                && GameData.character1Score - GameData.character2Score == 1) { currentCharacter1Score = "A"; }
            else if (GameData.character1Score < GameData.character2Score 
                && GameData.character2Score - GameData.character1Score == 1) { currentCharacter2Score = "A"; }
        }
        else
        {
            currentCharacter1Score = Mathf.Min(GameData.character1Score * 15, 40).ToString();
            currentCharacter2Score = Mathf.Min(GameData.character2Score * 15, 40).ToString();
        }

        // ここで各スコアの更新を検出して、更新が検出された場合は、各アニメーターに信号を送る
        if (character1ScoreText.text != currentCharacter1Score) { animator.SetTrigger("UpdateCharacter1Score"); }
        if (character2ScoreText.text != currentCharacter2Score) { animator.SetTrigger("UpdateCharacter2Score"); }
    }

    private void UpdateCharacter1Score()
    {
        // アニメーション内から呼ばれる関数
        character1ScoreText.text = currentCharacter1Score;
    }

    private void UpdateCharacter2Score()
    {
        // アニメーション内から呼ばれる関数
        character2ScoreText.text = currentCharacter2Score;
    }

    private void UpdateMessage(FoulState currentStatus, string lastShooter)
    {
        foulMessageText.text = lastShooter + "'s " + currentStatus + " !";
    }
}
