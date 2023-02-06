using UnityEngine;
using UnityEngine.UI;

public class PlayerBallSpeed : MonoBehaviour
{
    [System.NonSerialized]
    public static float playerBallSpeed = 1.0f;

    [SerializeField]
    private GameObject ballSpeedText;
    private Text valueText;

    private Slider playerBallSpeedSlider;

    // Start is called before the first frame update
    void Start()
    {
        playerBallSpeedSlider = GetComponent<Slider>();
        valueText = ballSpeedText.GetComponent<Text>();
        playerBallSpeedSlider.value = playerBallSpeed;
        valueText.text = "Player Ball Speed" + ": " + playerBallSpeed.ToString("0.00");
    }

    public void ChangeBallSpeed()
    {
        playerBallSpeed = playerBallSpeedSlider.value;
        valueText.text = "Player Ball Speed" + ": " + playerBallSpeed.ToString("0.00");
    }
}
