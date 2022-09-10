using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitionBallGenerator : MonoBehaviour
{
    public GameObject ballPrefab;
    private CompetitionBallController ballController;

    public GameObject player1;
    public GameObject player2;

    public GameObject gameManager;
    private CompetitionGameManager manager;

    public GameObject proController;
    private CompetitionGameControllerManager controllerManager;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameManager.GetComponent<CompetitionGameManager>();
        controllerManager = proController.GetComponent<CompetitionGameControllerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX1;
        float posZ1;

        float posX2;
        float posZ2;

        // When 1P is a server
        if (manager.server == "Player1")
        {
            posX1 = player1.transform.position.x;
            posZ1 = player1.transform.position.z;

            if (manager.Check("Ball") && manager.active)
            {
                if (controllerManager.PressEastButton(0))
                {
                    // After pressing the button, return it to the state where it cannot be pressed.
                    controllerManager.isPressedEast[0] = false;

                    manager.isTOS = true;
                    GameObject ball = Instantiate(ballPrefab);
                    ball.transform.position = new Vector3(posX1 - 5.0f, 6.0f, posZ1);
                    ballController = ball.GetComponent<CompetitionBallController>();
                    ballController.vy = 0.55f;
                    ballController.ballSpeed = FirstPlayerController.playerBallSpeed;
                }
            }
        }
        // When 2P is a server
        else if (manager.server == "Player2")
        {
            posX2 = player2.transform.position.x;
            posZ2 = player2.transform.position.z;

            if (manager.Check("Ball") && manager.active)
            {
                if (Input.GetKeyDown(KeyCode.Space) || controllerManager.PressEastButton(1))
                {
                    // After pressing the button, return it to the state where it cannot be pressed.
                    controllerManager.isPressedEast[1] = false;

                    manager.isTOS = true;
                    GameObject ball = Instantiate(ballPrefab);
                    ball.transform.position = new Vector3(posX2 + 5.0f, 6.0f, posZ2);
                    ballController = ball.GetComponent<CompetitionBallController>();
                    ballController.vy = 0.55f;
                    ballController.ballSpeed = SecondPlayerController.playerBallSpeed;
                }
            }
        }
    }
}
