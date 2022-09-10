using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MEnemyController : MonoBehaviour
{
    private float speedX = 0.0f;
    private float speedZ = 0.0f;

    [NonSerialized] public static float setEnemySpeed = 0.50f;
    [NonSerialized] public static float enemyBallSpeed = 1.0f;

    [NonSerialized] public Vector3 playerMovePoint;

    [NonSerialized] public static int delay = 0;
    [NonSerialized] public float power = 1.0f;
    [NonSerialized] public static float distance = 0.50f;

    [NonSerialized] public bool isHit = false;
    [NonSerialized] public bool isServe = false;
    [NonSerialized] public bool isFore = false;
    [NonSerialized] public bool takeBackRight = false;
    [NonSerialized] public bool takeBackLeft = false;

    public GameObject player;
    private MPlayerController playerController;

    private MBallController ballController;

    public GameObject gameManager;
    private MGameManager manager;

    private AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip serveSound;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<MPlayerController>();
        playerController.enemyMovePoint.x = transform.position.x;
        playerController.enemyMovePoint.z = transform.position.z;
        manager = gameManager.GetComponent<MGameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.active && !manager.isTOS && !Check("Ball")) { AutoMove(); }
    }

    void AutoMove()
    {
        // Controls movement in the x-axis direction
        if (!(playerController.enemyMovePoint.x - setEnemySpeed / 2.0f <= transform.position.x
              && transform.position.x <= playerController.enemyMovePoint.x + setEnemySpeed / 2.0f))
        {
            if (transform.position.x >= playerController.enemyMovePoint.x) { speedX = -setEnemySpeed / 2.0f; }
            else if (transform.position.x < playerController.enemyMovePoint.x) { speedX = setEnemySpeed; }
        }
        else { speedX = 0.0f; }

        if (-78.0f < transform.position.x + speedX && transform.position.x + speedX < -2.0f) { transform.Translate(speedX, 0.0f, 0.0f, Space.World); }

        // Controls z-axis movement
        if (!(playerController.enemyMovePoint.z - 5.0f <= transform.position.z
              && transform.position.z <= playerController.enemyMovePoint.z + 5.0f))
        {
            if (transform.position.z >= playerController.enemyMovePoint.z) { speedZ = -setEnemySpeed; }
            else if (transform.position.z < playerController.enemyMovePoint.z) { speedZ = setEnemySpeed; }
        }
        else { speedZ = 0.0f; }

        if (-32.0f < transform.position.z + speedZ && transform.position.z + speedZ < 32.0f) { transform.Translate(0.0f, 0.0f, speedZ, Space.World); }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && !isHit && manager.active && !manager.isTOS)
        {
            GameObject ball = other.gameObject;
            ballController = ball.GetComponent<MBallController>();

            if (!manager.isSERVE)
            {
                // Hit the ball back
                ballController.t = 0.0f;
                if (transform.position.x < -55.0f) { Shot(1.2f, Mathf.PI / 5.0f); }
                else if (transform.position.x > -25.0f) { Shot(0.7f, Mathf.PI / 5.0f); }
                else { Shot(1.0f, Mathf.PI / 6.0f); }

                // Determines the direction in which the ball is hit back
                if (transform.position.z < -15.0f) { ballController.vz = UnityEngine.Random.Range(0.25f, 0.40f); }
                else if (transform.position.z > 15.0f) { ballController.vz = -UnityEngine.Random.Range(0.25f, 0.40f); }
                else
                {
                    int random = UnityEngine.Random.Range(-1, 3);
                    int randomTarget;
                    if (random < 0) { randomTarget = -1; }
                    else { randomTarget = 1; }

                    // When the player is on the left side of the court, aim based on (x, z) = (40.0f, 15.0f)
                    if (playerController.transform.position.z <= 0.0f)
                    {
                        float distanceZ = 15.0f * randomTarget - ballController.transform.position.z;
                        float distanceX = 40.0f - ballController.transform.position.x;
                        float arriveTime = Mathf.Abs(distanceX / ballController.vx);
                        ballController.vz = (distanceZ / arriveTime) * 1.2f * distance;
                    }
                    // When the player is on the right side of the court, aim based on (x, z) = (40.0f, -15.0f)
                    else if (playerController.transform.position.z > 0.0f)
                    {
                        float disZ = -15.0f * randomTarget - ballController.transform.position.z;
                        float distanceX = 40.0f - ballController.transform.position.x;
                        float arriveTime = Mathf.Abs(distanceX / ballController.vx);
                        ballController.vz = (disZ / arriveTime) * 1.2f * distance;
                    }
                }
            }

            // Arrival point of the hit ball in the opponent's court (x-coordinate)
            float g = 2.5f * ballController.deltaTime;
            float v0 = -ballController.vy;
            float boundTime = -v0 / g + Mathf.Sqrt(v0 * v0 + 2.0f * g * ballController.transform.position.y) / g;
            float boundPointX = ballController.vx * boundTime + ballController.transform.position.x;
            float playerMovePointX = boundPointX + ballController.vx * 25.0f;

            // Arrival point of the hit ball in the opponent's court (z-coordinate)
            float ballPosX = ballController.transform.position.x;
            float ballPosZ = ballController.transform.position.z;
            float disX = ballPosX - playerMovePointX;
            float arriveTimeZ = Mathf.Abs(disX / ballController.vx);

            float ballSpeedX = ballController.vx;
            float ballSpeedZ = ballController.vz;

            StartCoroutine(DelayMethod(delay, () =>
            {
                // Add 25 frames of travel distance to hit a little time after the bounce
                playerMovePoint.x = boundPointX + ballController.vx * 25.0f;

                playerMovePoint.z = ballPosZ + arriveTimeZ * ballController.vz;
                if (playerMovePoint.z < -32.0f) { playerMovePoint.z = -32.0f; }
                else if (32.0f < playerMovePoint.z) { playerMovePoint.z = 32.0f; }

                if (playerMovePoint.z >= player.transform.position.z) { playerController.takeBackRight = true; }
                else if (playerMovePoint.z < player.transform.position.z) { playerController.takeBackLeft = true; }
            }));

            manager.ballCount = 0;
            if (!manager.isOUT) manager.who = "Opponent";

            // For animation
            isHit = true;
            if (ball.transform.position.z > gameObject.transform.position.z) { isFore = false; }
            else if (ball.transform.position.z <= gameObject.transform.position.z) { isFore = true; }
        }
        // When in tos motion
        else if (other.gameObject.CompareTag("Ball") && !isHit && manager.active && manager.isTOS)
        {
            GameObject ball = other.gameObject;
            ballController = ball.GetComponent<MBallController>();

            if (ball.transform.position.y >= 14.0f)
            {
                // For animation
                isServe = true;
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
            isServe = false;
            isHit = false;
            takeBackRight = false;
            takeBackLeft = false;
        }
    }

    private void Serve(float _power)
    {
        audioSource.PlayOneShot(serveSound);
        ballController.vx = _power * 1.8f;
        ballController.vy = -Mathf.Sin(Mathf.PI / 24.0f);
        ballController.vz = UnityEngine.Random.Range(0.30f, 0.70f) * manager.whereServe;

        // Arrival point in the opponent's court of the ball hit (x-coordinate)
        float g = 2.5f * ballController.deltaTime;
        float v0 = -ballController.vy;
        float boundTime = -v0 / g + Mathf.Sqrt(v0 * v0 + 2.0f * g * ballController.transform.position.y) / g;
        float boundPointX = ballController.vx * boundTime + ballController.transform.position.x;
        float playerMovePointX = boundPointX + ballController.vx * 20.0f;

        // Arrival point in the opponent's court of the ball hit (z-coordinate)
        float ballPosX = ballController.transform.position.x;
        float ballPosZ = ballController.transform.position.z;
        float disX = ballPosX - playerMovePointX;
        float arriveTimeZ = Mathf.Abs(disX / ballController.vx);

        float ballSpeedX = ballController.vx;
        float ballSpeedZ = ballController.vz;

        StartCoroutine(DelayMethod(delay, () =>
        {
            // Add 20 frames of travel distance to hit a little time after the bounce
            playerMovePoint.x = boundPointX + ballController.vx * 20.0f;

            playerMovePoint.z = ballPosZ + arriveTimeZ * ballController.vz;
            if (playerMovePoint.z < -32.0f) { playerMovePoint.z = -32.0f; }
            else if (32.0f < playerMovePoint.z) { playerMovePoint.z = 32.0f; }

            if (playerMovePoint.z >= player.transform.position.z) { playerController.takeBackRight = true; }
            else if (playerMovePoint.z < player.transform.position.z) { playerController.takeBackLeft = true; }
        }));

        manager.who = "Opponent";
    }

    private void Shot(float _powerRate, float _theta)
    {
        int num = UnityEngine.Random.Range(0, 3);
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

    private IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++) { yield return null; }
        action();
    }
}
