using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MPlayerController : MonoBehaviour
{
    [NonSerialized] public float speedX = 0.0f;
    [NonSerialized] public float speedZ = 0.0f;

    public static float setPlayerSpeed = 0.50f;
    public static float playerBallSpeed = 1.0f;

    [NonSerialized] public Vector3 enemyMovePoint;

    [NonSerialized] public static int delay = 20;
    [NonSerialized] public float power = 1.0f;

    [NonSerialized] public bool isHit = false;
    [NonSerialized] public bool isServe = false;
    [NonSerialized] public bool isFore = false;
    [NonSerialized] public bool takeBackRight = false;
    [NonSerialized] public bool takeBackLeft = false;

    private bool normalShot;
    private bool fastShot;
    private bool lobShot;
    private bool dropShot;
    private int normalFlames;
    private int fastFlames;
    private int lobFlames;
    private int dropFlames;

    private bool fore;
    private bool back;
    private int foreFlames;
    private int backFlames;

    private MBallController ballController;

    public GameObject enemy;
    private MEnemyController enemyController;

    public GameObject gameManager;
    private MGameManager manager;

    public GameObject joyCon;
    private JoyConManager joyConManager;

    private AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip serveSound;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = enemy.GetComponent<MEnemyController>();
        enemyController.playerMovePoint.x = transform.position.x;
        enemyController.playerMovePoint.z = transform.position.z;

        manager = gameManager.GetComponent<MGameManager>();
        joyConManager = joyCon.GetComponent<JoyConManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.active && !manager.isTOS && !Check("Ball"))
        {
            // Move automatically
            AutoMove();

            // Extend the grace frame when hitting the ball back
            ExtendFlameFore();
            ExtendFlameBack();

            ExtendFlameEast();
            ExtendFlameSouth();
            ExtendFlameWest();
            ExtendFlameNorth();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") 
            && !isHit && manager.active && !manager.isTOS && !manager.isSERVE && manager.who != "Player")
        {
            if (normalShot) { ControlPlayer(other.gameObject, 0.40f, 25.0f); }
            else if (lobShot) { ControlPlayer(other.gameObject, 0.30f, 20.0f); }
            else if (dropShot) { ControlPlayer(other.gameObject, 0.40f, 30.0f); }
            else if (fastShot || fore || back) { ControlPlayer(other.gameObject, 0.50f, 15.0f); }
        }

        // When in tos motion
        else if (other.gameObject.CompareTag("Ball") && !isHit && manager.active && manager.isTOS)
        {
            GameObject ball = other.gameObject;
            ballController = ball.GetComponent<MBallController>();

            if (joyConManager.swing != null || Input.GetKeyDown(KeyCode.Space))
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

    private void ControlPlayer(GameObject _ball, float _vz, float _frame)
    {
        GameObject ball = _ball;
        ballController = ball.GetComponent<MBallController>();
        ballController.t = 0.0f;
        isHit = true;

        if (enemyController.playerMovePoint.z < transform.position.z) { isFore = false; }
        else if (enemyController.playerMovePoint.z >= transform.position.z) { isFore = true; }

        if (fastShot)
        {
            fastShot = false;
            if (!manager.isSERVE) { FastShot(power); }
        }
        else if (lobShot)
        {
            lobShot = false;
            if (!manager.isSERVE) { LobShot(power); }
        }
        else if (dropShot)
        {
            dropShot = false;
            if (!manager.isSERVE) { DropShot(power); }
        }
        else if (normalShot || fore || back)
        {
            normalShot = false;
            if (!manager.isSERVE) { NormalShot(power); }
        }

        // Determines how fast it moves sideways
        if (!manager.isSERVE && ballController.transform.position.x - gameObject.transform.position.x >= 0.0f)
        {
            ballController.vz = _vz * (2 * Convert.ToInt32(isFore) - 1)
                                                * (ballController.transform.position.x - gameObject.transform.position.x) / 12.0f;
        }
        else if (!manager.isSERVE && ballController.transform.position.x - gameObject.transform.position.x < 0.0f)
        {
            ballController.vz = _vz * (2 * Convert.ToInt32(isFore) - 1)
                                                 * (ballController.transform.position.x - gameObject.transform.position.x) / 12.0f;
        }
        else if (!manager.isSERVE) { ballController.vz = 0.0f; }

        // Arrival point in the opponent's court of the ball hit (x-coordinate)
        float g = 2.5f * ballController.deltaTime;
        float v0 = -ballController.vy;
        float boundTime = -v0 / g + Mathf.Sqrt(v0 * v0 + 2.0f * g * ballController.transform.position.y) / g;
        float boundPointX = ballController.vx * boundTime + ballController.transform.position.x;
        float enemyMovePointX = boundPointX + ballController.vx * _frame;

        // Arrival point in the opponent's court of the ball hit (z-coordinate)
        float ballPosX = ballController.transform.position.x;
        float ballPosZ = ballController.transform.position.z;
        float disX = ballPosX - enemyMovePointX;
        float arriveTimeZ = Mathf.Abs(disX / ballController.vx);

        float ballSpeedX = ballController.vx;
        float ballSpeedZ = ballController.vz;

        StartCoroutine(DelayMethod(delay, () =>
        {
            // Add "_frame" distance traveled to hit a little time after bounce.
            enemyMovePoint.x = boundPointX + ballSpeedX * _frame;

            enemyMovePoint.z = ballPosZ + arriveTimeZ * ballSpeedZ;
            if (enemyMovePoint.z < -32.0f) { enemyMovePoint.z = -32.0f; }
            else if (32.0f < enemyMovePoint.z) { enemyMovePoint.z = 32.0f; }

            if (enemyMovePoint.z >= enemy.transform.position.z) { enemyController.takeBackLeft = true; }
            else if (enemyMovePoint.z < enemy.transform.position.z) { enemyController.takeBackRight = true; }
        }));

        if (!manager.isOUT) manager.who = "Player";
        manager.ballCount = 0;
    }

    private void Serve(float _power)
    {
        audioSource.PlayOneShot(serveSound);
        ballController.vx = -_power * 1.8f;
        ballController.vy = -Mathf.Sin(Mathf.PI / 24.0f);
        ballController.vz = -0.50f * manager.whereServe * _power;

        // Arrival point in the opponent's court of the ball hit (x-coordinate)
        float g = 2.5f * ballController.deltaTime;
        float v0 = -ballController.vy;
        float boundTime = -v0 / g + Mathf.Sqrt(v0 * v0 + 2.0f * g * ballController.transform.position.y) / g;
        float boundPointX = ballController.vx * boundTime + ballController.transform.position.x;
        float enemyMovePointX = boundPointX + ballController.vx * 20.0f;

        // Arrival point in the opponent's court of the ball hit (z-coordinate)
        float ballPosX = ballController.transform.position.x;
        float ballPosZ = ballController.transform.position.z;
        float disX = ballPosX - enemyMovePointX;
        float arriveTimeZ = Mathf.Abs(disX / ballController.vx);

        float ballSpeedX = ballController.vx;
        float ballSpeedZ = ballController.vz;

        StartCoroutine(DelayMethod(delay, () =>
        {
            // Add 20 frames of distance to hit a little time after the bounce
            enemyMovePoint.x = boundPointX + ballSpeedX * 20.0f;

            enemyMovePoint.z = ballPosZ + arriveTimeZ * ballSpeedZ;
            if (enemyMovePoint.z < -32.0f) { enemyMovePoint.z = -32.0f; }
            else if (32.0f < enemyMovePoint.z) { enemyMovePoint.z = 32.0f; }

            if (enemyMovePoint.z >= enemy.transform.position.z) { enemyController.takeBackLeft = true; }
            else if (enemyMovePoint.z < enemy.transform.position.z) { enemyController.takeBackRight = true; }
        }));

        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player";
    }

    private void NormalShot(float _power)
    {
        int num = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = -_power * 1.1f;
        ballController.vy = Mathf.Sin(Mathf.PI / 6.0f);
        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player";
        manager.rallyCount++;
    }

    private void LobShot(float _power)
    {
        int num = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = -_power * 0.74f;
        ballController.vy = Mathf.Sin(Mathf.PI / 3.0f);
        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player";
        manager.rallyCount++;
    }

    private void FastShot(float _power)
    {
        int num = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = -_power * 1.75f;
        ballController.vy = Mathf.Sin(Mathf.PI / 10.0f);
        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player";
        manager.rallyCount++;
    }

    private void DropShot(float _power)
    {
        int num = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = -_power * 0.80f;
        ballController.vy = Mathf.Sin(Mathf.PI / 6.0f);
        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player";
        manager.rallyCount++;
    }

    private void AutoMove()
    {
        // Controls movement in the x-axis direction
        if (!(enemyController.playerMovePoint.x - setPlayerSpeed / 2.0f <= transform.position.x
              && transform.position.x <= enemyController.playerMovePoint.x + setPlayerSpeed / 2.0f))
        {
            if (transform.position.x >= enemyController.playerMovePoint.x) { speedX = -setPlayerSpeed / 2.0f; }
            else if (transform.position.x < enemyController.playerMovePoint.x) { speedX = setPlayerSpeed; }
        }
        else { speedX = 0.0f; }

        if (2.0f < transform.position.x + speedX && transform.position.x + speedX < 78.0f)
        {
            transform.Translate(speedX, 0.0f, 0.0f, Space.World);
        }

        // Controls movement in the z-axis direction
        if (!(enemyController.playerMovePoint.z - 5.0f <= transform.position.z
              && transform.position.z <= enemyController.playerMovePoint.z + 5.0f))
        {
            if (transform.position.z >= enemyController.playerMovePoint.z) { speedZ = -setPlayerSpeed; }
            else if (transform.position.z < enemyController.playerMovePoint.z) { speedZ = setPlayerSpeed; }
        }
        else { speedZ = 0.0f; }

        if (-32.0f < transform.position.z + speedZ && transform.position.z + speedZ < 32.0f)
        {
            transform.Translate(0.0f, 0.0f, speedZ, Space.World);
        }
    }

    private bool Check(string _tagName)
    {
        GameObject[] _tagObjects = GameObject.FindGameObjectsWithTag(_tagName);
        if (_tagObjects.Length == 0) { return true; }
        else { return false; }
    }

    private void ExtendFlameFore()
    {
        if (fore)
        {
            back = false;

            foreFlames--;
            if (foreFlames < 0) { fore = false; }
        }
        else if (!fore)
        {
            foreFlames = 60;
            if (joyConManager.fore) { fore = true; }
        }
    }

    private void ExtendFlameBack()
    {
        if (back)
        {
            fore = false;

            backFlames--;
            if (backFlames < 0) { back = false; }
        }
        else if (!back)
        {
            backFlames = 60;
            if (joyConManager.back) { back = true; }
        }
    }

    private void ExtendFlameEast()
    {
        if (normalShot)
        {
            lobShot = false;
            fastShot = false;
            dropShot = false;

            normalFlames--;
            if (normalFlames < 0) { normalShot = false; }
        }
        else if (!normalShot)
        {
            normalFlames = 60;
            normalShot = (fore || back) && joyConManager.isPressedA;
        }
    }

    private void ExtendFlameSouth()
    {
        if (lobShot)
        {
            normalShot = false;
            fastShot = false;
            dropShot = false;

            lobFlames--;
            if (lobFlames < 0) { lobShot = false; }
        }
        else if (!lobShot)
        {
            lobFlames = 60;
            lobShot = (fore || back) && joyConManager.isPressedB;
        }
    }

    private void ExtendFlameWest()
    {
        if (dropShot)
        {
            normalShot = false;
            fastShot = false;
            lobShot = false;

            dropFlames--;
            if (dropFlames < 0) { dropShot = false; }
        }
        else if (!dropShot)
        {
            dropFlames = 60;
            dropShot = (fore || back) && joyConManager.isPressedY;
        }
    }

    private void ExtendFlameNorth()
    {
        if (fastShot)
        {
            normalShot = false;
            lobShot = false;
            dropShot = false;

            fastFlames--;
            if (fastFlames < 0) { fastShot = false; }
        }
        else if (!fastShot)
        {
            fastFlames = 60;
            fastShot = (fore || back) && joyConManager.isPressedX;
        }
    }

    private IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++) { yield return null; }
        action();
    }
}
