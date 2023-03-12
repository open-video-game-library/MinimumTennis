using UnityEngine;
using UnityEngine.SceneManagement;

public class CompetitionStartButton : MonoBehaviour
{
    public void OnClickCompetitionButton()
    {
        SceneManager.LoadScene("CompetitionScene");

        PlayerColor.playerColor = new Color32(255, 255, 255, 255);
        OpponentColor.opponentColor = new Color32(30, 30, 30, 255);
    }
}
