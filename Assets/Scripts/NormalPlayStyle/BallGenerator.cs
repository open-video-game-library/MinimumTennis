using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public GameObject ballPrefab;
    private BallController ballController;

    public GameObject player;
    public GameObject enemy;

    private GameObject[] tagObjects;

    public GameObject gameManager;
    private GameManager manager;

    public GameObject proController;
    private ProControllerManager controllerManager;

    private int autoServeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameManager.GetComponent<GameManager>();
        controllerManager = proController.GetComponent<ProControllerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX;
        float posZ;

        // When the player is a server
        if (manager.server == "Player")
        {
            posX = player.transform.position.x;
            posZ = player.transform.position.z;

            if (Check("Ball") && manager.active)
            {
                if (Input.GetKeyDown(KeyCode.Space) || controllerManager.PressEastButton())
                {
                    // After pressing the button, return it to the state where it cannot be pressed.
                    controllerManager.isPressedEast = false;

                    manager.isTOS = true;
                    GameObject ball = Instantiate(ballPrefab);
                    ball.transform.position = new Vector3(posX - 5.0f, 6.0f, posZ);
                    ballController = ball.GetComponent<BallController>();
                    ballController.vy = 0.55f;
                    ballController.ballSpeed = PlayerController.playerBallSpeed;
                }
            }
        }
        // When the opponent is a server
        else if (manager.server == "Enemy")
        {
            posX = enemy.transform.position.x;
            posZ = enemy.transform.position.z;

            if (Check("Ball") && manager.active)
            {
                autoServeCount++;

                if (autoServeCount > 1 * 60)
                {
                    manager.isTOS = true;
                    GameObject ball = Instantiate(ballPrefab);
                    ball.transform.position = new Vector3(posX + 5.0f, 6.0f, posZ);
                    ballController = ball.GetComponent<BallController>();
                    ballController.vy = 0.55f;
                    ballController.ballSpeed = EnemyController.enemyBallSpeed;

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
