using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalControlStartButton : MonoBehaviour
{
    public void OnClickStartButton()
    {
        PlayerMaximumSpeed.playerMaximumSpeed = 30.0f;
        PlayerAcceleration.playerAcceleration = 3.0f;
        PlayerBallSpeed.playerBallSpeed = 1.0f;

        OpponentMaximumSpeed.opponentMaximumSpeed = 30.0f;
        OpponentAcceleration.opponentAcceleration = 1.0f;
        OpponentBallSpeed.opponentBallSpeed = 1.0f;
        OpponentReactionDelay.delay = 20;
        Distance.distance = 0.50f;

        PlayerColor.playerColor = new Color32(255, 255, 255, 255);
        OpponentColor.opponentColor = new Color32(30, 30, 30, 255);

        SceneManager.LoadScene("TennisScene");
    }
}
