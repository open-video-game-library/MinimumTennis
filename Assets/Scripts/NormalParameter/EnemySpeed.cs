using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpeed : MonoBehaviour
{
    private Slider enemySpeedSlider;

    public GameObject speedText;
    private Text enemySpeed;

    // Start is called before the first frame update
    void Start()
    {
        enemySpeedSlider = GetComponent<Slider>();
        enemySpeed = speedText.GetComponent<Text>();
        enemySpeedSlider.value = EnemyController.setEnemySpeed;
        enemySpeed.text = "Opponent Speed" + ": " + (EnemyController.setEnemySpeed * 2.0f).ToString("0.00");
    }

    public void ChangeSpeed()
    {
        EnemyController.setEnemySpeed = enemySpeedSlider.value;
        enemySpeed.text = "Opponent Speed" + ": " + (EnemyController.setEnemySpeed * 2.0f).ToString("0.00");
    }
}
