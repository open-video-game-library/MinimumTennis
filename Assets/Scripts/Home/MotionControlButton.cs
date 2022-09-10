using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MotionControlButton : MonoBehaviour
{
    public void OnClickStartButton()
    {
        MPlayerController.setPlayerSpeed = 0.50f;
        MPlayerController.playerBallSpeed = 1.0f;
        MPlayerController.delay = 0;
        MEnemyController.setEnemySpeed = 0.50f;
        MEnemyController.enemyBallSpeed = 1.0f;
        MPlayerController.delay = 20;
        MEnemyController.distance = 0.50f;
        JoyConManager.threhold = 4.0f;

        SceneManager.LoadScene("MotionTennisScene");
    }
}
