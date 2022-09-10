using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBallSpeed : MonoBehaviour
{
    private Slider playerBallSpeedSlider;

    public GameObject ballSpeedText;
    private Text playerBallSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerBallSpeedSlider = GetComponent<Slider>();
        playerBallSpeed = ballSpeedText.GetComponent<Text>();
        playerBallSpeedSlider.value = PlayerController.playerBallSpeed;
        playerBallSpeed.text = "Player Ball Speed" + ": " + PlayerController.playerBallSpeed.ToString("0.00");
    }

    public void ChangeBallSpeed()
    {
        PlayerController.playerBallSpeed = playerBallSpeedSlider.value;
        playerBallSpeed.text = "Player Ball Speed" + ": " + PlayerController.playerBallSpeed.ToString("0.00");
    }
}
