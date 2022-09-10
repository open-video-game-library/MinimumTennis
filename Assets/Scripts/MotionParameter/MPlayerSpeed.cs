using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPlayerSpeed : MonoBehaviour
{
    private Slider playerSpeedSlider;

    public GameObject speedText;
    private Text playerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeedSlider = GetComponent<Slider>();
        playerSpeed = speedText.GetComponent<Text>();
        playerSpeedSlider.value = MPlayerController.setPlayerSpeed;
        playerSpeed.text = "Player Speed" + ": " + (MPlayerController.setPlayerSpeed * 2.0f).ToString("0.00");
    }

    public void ChangeSpeed()
    {
        MPlayerController.setPlayerSpeed = playerSpeedSlider.value;
        playerSpeed.text = "Player Speed" + ": " + (MPlayerController.setPlayerSpeed * 2.0f).ToString("0.00");
    }
}
