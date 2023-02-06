using UnityEngine;
using UnityEngine.UI;

public class OpponentBallSpeed : MonoBehaviour
{
    [System.NonSerialized]
    public static float opponentBallSpeed = 1.0f;

    [SerializeField]
    private GameObject ballSpeedText;
    private Text valueText;

    private Slider opponentBallSpeedSlider;

    // Start is called before the first frame update
    void Start()
    {
        opponentBallSpeedSlider = GetComponent<Slider>();
        valueText = ballSpeedText.GetComponent<Text>();
        opponentBallSpeedSlider.value = opponentBallSpeed;
        valueText.text = "Opponent Ball Speed" + ": " + opponentBallSpeed.ToString("0.00");
    }

    public void ChangeBallSpeed()
    {
        opponentBallSpeed = opponentBallSpeedSlider.value;
        valueText.text = "Opponent Ball Speed" + ": " + opponentBallSpeed.ToString("0.00");
    }
}
