using UnityEngine;
using UnityEngine.UI;

public class ScorePanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject score;
    [SerializeField]
    private GameObject message;

    private Text scoreText;
    private Text messageText;

    // Start is called before the first frame update
    void Awake()
    {
        scoreText = score.GetComponentInChildren<Text>();
        messageText = message.GetComponentInChildren<Text>();
    }

    void OnEnable()
    {
        UpdateScore(GameManager.character1Score, GameManager.character2Score);
        UpdateMessage(GameManager.foul, GameManager.pointLoser);
    }

    private void UpdateScore(int character1Score, int character2Score)
    {
        bool deuce = (character2Score >= 3) && (character1Score >= 3);
        if (deuce)
        {
            if (character1Score == character2Score) { scoreText.text = "Deuce"; }
            else if (character1Score > character2Score)
            {
                if (character1Score - character2Score == 1) { scoreText.text = "A  Å|  40"; }
                else if (character1Score - character2Score >= 2) { scoreText.text = "GAME"; }
            }
            else if (character1Score < character2Score)
            {
                if (character2Score - character1Score == 1) { scoreText.text = "40  Å|  A"; }
                else if (character2Score - character1Score >= 2) { scoreText.text = "GAME"; }
            }
        }
        else
        {
            if (character1Score >= 4) { scoreText.text = "GAME"; }
            else if (character2Score >= 4) { scoreText.text = "GAME"; }
            else { scoreText.text = Mathf.Min(character1Score * 15, 40) + "  Å|  " + Mathf.Min(character2Score * 15, 40); }
        }
    }

    private void UpdateMessage(string currentStatus, string lastShooter)
    {
        messageText.text = currentStatus + " by " + lastShooter;
    }
}
