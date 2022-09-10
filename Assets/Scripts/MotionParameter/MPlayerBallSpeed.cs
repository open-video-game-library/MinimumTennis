using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPlayerBallSpeed : MonoBehaviour
{
    private Slider playerBallSpeedSlider;

    public GameObject ballSpeedText;
    private Text playerBallSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerBallSpeedSlider = GetComponent<Slider>();
        playerBallSpeed = ballSpeedText.GetComponent<Text>();
        playerBallSpeedSlider.value = MPlayerController.playerBallSpeed;
        playerBallSpeed.text = "Player Ball Speed" + ": " + MPlayerController.playerBallSpeed.ToString("0.00");
    }

    public void ChangeBallSpeed()
    {
        MPlayerController.playerBallSpeed = playerBallSpeedSlider.value;
        playerBallSpeed.text = "Player Ball Speed" + ": " + MPlayerController.playerBallSpeed.ToString("0.00");
    }
}
