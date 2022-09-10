using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBallSpeed : MonoBehaviour
{
    private Slider enemyBallSpeedSlider;

    public GameObject ballSpeedText;
    private Text enemyBallSpeed;

    // Start is called before the first frame update
    void Start()
    {
        enemyBallSpeedSlider = GetComponent<Slider>();
        enemyBallSpeed = ballSpeedText.GetComponent<Text>();
        enemyBallSpeedSlider.value = EnemyController.enemyBallSpeed;
        enemyBallSpeed.text = "Opponent Ball Speed" + ": " + EnemyController.enemyBallSpeed.ToString("0.00");
    }

    public void ChangeBallSpeed()
    {
        EnemyController.enemyBallSpeed = enemyBallSpeedSlider.value;
        enemyBallSpeed.text = "Opponent Ball Speed" + ": " + EnemyController.enemyBallSpeed.ToString("0.00");
    }
}
