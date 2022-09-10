using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEnemyBallSpeed : MonoBehaviour
{
    private Slider enemyBallSpeedSlider;

    public GameObject ballSpeedText;
    private Text enemyBallSpeed;

    // Start is called before the first frame update
    void Start()
    {
        enemyBallSpeedSlider = GetComponent<Slider>();
        enemyBallSpeed = ballSpeedText.GetComponent<Text>();
        enemyBallSpeedSlider.value = MEnemyController.enemyBallSpeed;
        enemyBallSpeed.text = "Opponent Ball Speed" + ": " + MEnemyController.enemyBallSpeed.ToString("0.00");
    }

    public void ChangeBallSpeed()
    {
        MEnemyController.enemyBallSpeed = enemyBallSpeedSlider.value;
        enemyBallSpeed.text = "Opponent Ball Speed" + ": " + MEnemyController.enemyBallSpeed.ToString("0.00");
    }
}
