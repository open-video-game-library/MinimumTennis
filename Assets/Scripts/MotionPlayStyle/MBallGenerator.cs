using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBallGenerator : MonoBehaviour
{
    public GameObject ballPrefab;
    private MBallController ballController;

    public GameObject player;
    public GameObject enemy;

    private GameObject[] tagObjects;

    public GameObject gameManager;
    private MGameManager manager;

    public GameObject joyCon;
    private JoyConManager joyConManager;

    private int autoServeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameManager.GetComponent<MGameManager>();
        joyConManager = joyCon.GetComponent<JoyConManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float posx;
        float posz;

        // When the player is a server
        if (manager.server == "Player")
        {
            posx = player.transform.position.x;
            posz = player.transform.position.z;

            if (Check("Ball") && manager.active)
            {
                if (joyConManager.tos)
                {
                    joyConManager.fore = false;
                    
                    manager.isTOS = true;
                    GameObject ball = Instantiate(ballPrefab);
                    ball.transform.position = new Vector3(posx - 5.0f, 6.0f, posz);
                    ballController = ball.GetComponent<MBallController>();
                    ballController.vy = 0.55f;
                    ballController.ballSpeed = MPlayerController.playerBallSpeed;
                }
            }
        }
        // When the opponent is a server
        else if (manager.server == "Enemy")
        {
            posx = enemy.transform.position.x;
            posz = enemy.transform.position.z;

            if (Check("Ball") && manager.active)
            {
                autoServeCount++;

                if (autoServeCount > 1 * 60)
                {
                    manager.isTOS = true;
                    GameObject ball = Instantiate(ballPrefab);
                    ball.transform.position = new Vector3(posx + 5.0f, 6.0f, posz);
                    ballController = ball.GetComponent<MBallController>();
                    ballController.vy = 0.55f;

                    ballController.ballSpeed = MEnemyController.enemyBallSpeed;
                    autoServeCount = 0;
                }
            }
        }
    }

    private bool Check(string _tagName)
    {
        tagObjects = GameObject.FindGameObjectsWithTag(_tagName);
        if (tagObjects.Length == 0) { return true; }
        else { return false; }
    }
}
