using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float speedX = 0.0f;
    private float speedZ = 0.0f;

    [System.NonSerialized] public static float setEnemySpeed = 0.50f;
    [System.NonSerialized] public static float enemyBallSpeed = 1.0f;

    [System.NonSerialized] public float power = 1.0f;
    [System.NonSerialized] public static float distance = 0.50f;

    [System.NonSerialized] public bool isHit = false;
    [System.NonSerialized] public bool isSmash = false;
    [System.NonSerialized] public bool isRight = false;

    public GameObject player;
    private PlayerController playerController;

    private BallController ballController;

    public GameObject gameManager;
    private GameManager manager;

    private AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip serveSound;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerController.enemyMovePoint.x = transform.position.x;
        playerController.enemyMovePoint.z = transform.position.z;
        manager = gameManager.GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.active && !manager.isTOS && !Check("Ball")) { Move(); }
    }

    private void Move()
    {
        // Controls movement in the x-axis direction
        if (!(playerController.enemyMovePoint.x - setEnemySpeed / 2.0f <= transform.position.x
              && transform.position.x <= playerController.enemyMovePoint.x + setEnemySpeed / 2.0f))
        {
            if (transform.position.x >= playerController.enemyMovePoint.x) { speedX = -setEnemySpeed / 2.0f; }
            else if (transform.position.x < playerController.enemyMovePoint.x) { speedX = setEnemySpeed; }
        }
        else { speedX = 0.0f; }

        if (-78.0f < transform.position.x + speedX && transform.position.x + speedX < -2.0f)
        {
            transform.Translate(speedX, 0.0f, 0.0f, Space.World);
        }

        // Controls movement in the z-axis direction
        if (!(playerController.enemyMovePoint.z - setEnemySpeed / 2.0f <= transform.position.z
              && transform.position.z <= playerController.enemyMovePoint.z + setEnemySpeed / 2.0f))
        {
            if (transform.position.z >= playerController.enemyMovePoint.z) { speedZ = -setEnemySpeed; }
            else if (transform.position.z < playerController.enemyMovePoint.z) { speedZ = setEnemySpeed; }
        }
        else { speedZ = 0.0f; }

        if (-32.0f < transform.position.z + speedZ && transform.position.z + speedZ < 32.0f)
        {
            transform.Translate(0.0f, 0.0f, speedZ, Space.World);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && !isHit && manager.active && !manager.isTOS)
        {
            GameObject ball = other.gameObject;
            ballController = ball.GetComponent<BallController>();

            if (!manager.isSERVE)
            {
                // Hit the ball back
                ballController.t = 0.0f;
                if (transform.position.x < -55.0f) { Shot(1.2f, Mathf.PI / 5.0f); }
                else if (transform.position.x > -25.0f) { Shot(0.7f, Mathf.PI / 5.0f); }
                else { Shot(1.0f, Mathf.PI / 6.0f); }

                // Determines the direction in which the ball is hit back
                if (transform.position.z < -15.0f) { ballController.vz = Random.Range(0.25f, 0.40f); }
                else if (transform.position.z > 15.0f) { ballController.vz = -Random.Range(0.25f, 0.40f); }
                else
                {
                    int random = Random.Range(-1, 3);
                    int randomTarget;
                    if (random < 0) { randomTarget = -1; }
                    else { randomTarget = 1; }

                    // When the player is on the left side of the court, aim based on (x, z) = (40.0f, 15.0f)
                    if (playerController.transform.position.z <= 0.0f)
                    {
                        float disZ = 15.0f * randomTarget - ballController.transform.position.z;
                        float disX = 40.0f - ballController.transform.position.x;
                        float arriveTimeZ = Mathf.Abs(disX / ballController.vx);
                        ballController.vz = (disZ / arriveTimeZ) * 1.2f * distance;
                    }
                    // When the player is on the right side of the court, aim based on (x, z) = (40.0f, -15.0f)
                    else if (playerController.transform.position.z > 0.0f)
                    {
                        float disZ = -15.0f * randomTarget - ballController.transform.position.z;
                        float disX = 40.0f - ballController.transform.position.x;
                        float arriveTimeZ = Mathf.Abs(disX / ballController.vx);
                        ballController.vz = (disZ / arriveTimeZ) * 1.2f * distance;
                    }
                }
            }

            manager.ballCount = 0;
            if (!manager.isOUT) manager.who = "Opponent";

            // For animation
            isHit = true;
            if (ball.transform.position.z > gameObject.transform.position.z && ball.transform.position.y < 9.0f) isRight = false;
            else if (ball.transform.position.z <= gameObject.transform.position.z && ball.transform.position.y < 90f) { isRight = true; }
            else if (ball.transform.position.y >= 90f) { isSmash = true; }
        }
        // When in tos motion
        else if (other.gameObject.CompareTag("Ball") && !isHit && manager.active && manager.isTOS)
        {
            GameObject ball = other.gameObject;
            ballController = ball.GetComponent<BallController>();

            if (ball.transform.position.y >= 14.0f)
            {
                // For animation
                isSmash = true;
                ballController.t = 0.0f;
                Serve(power);
                manager.isSERVE = true;
                manager.isTOS = false;
                manager.status = "NORMAL";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            isSmash = false;
            isHit = false;
        }
    }

    private void Serve(float _power)
    {
        audioSource.PlayOneShot(serveSound);
        ballController.vx = _power * 1.8f;
        ballController.vy = -Mathf.Sin(Mathf.PI / 24.0f);
        ballController.vz = Random.Range(0.30f, 0.70f) * manager.whereServe;
        manager.who = "Opponent";
    }

    private void Shot(float _powerRate, float _theta)
    {
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = power * _powerRate;
        ballController.vy = Mathf.Sin(_theta);
        ballController.ballSpeed = enemyBallSpeed;
        manager.who = "Opponent";
        manager.rallyCount++;
    }

    private bool Check(string _tagName)
    {
        GameObject[] _tagObjects = GameObject.FindGameObjectsWithTag(_tagName);
        if (_tagObjects.Length == 0) { return true; }
        else { return false; }
    }
}
