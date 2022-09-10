using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalControlButton : MonoBehaviour
{
    public void OnClickStartButton()
    {
        PlayerController.setPlayerSpeed = 0.50f;
        PlayerController.playerBallSpeed = 1.0f;
        EnemyController.setEnemySpeed = 0.50f;
        EnemyController.enemyBallSpeed = 1.0f;
        PlayerController.delay = 20;
        EnemyController.distance = 0.5f;

        SceneManager.LoadScene("TennisScene");
    }
}
