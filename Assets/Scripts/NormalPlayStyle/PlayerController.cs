using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [NonSerialized] public float speedX = 0.0f;
    [NonSerialized] public float speedZ = 0.0f;

    [NonSerialized] public static float setPlayerSpeed = 0.50f;
    [NonSerialized] public static float playerBallSpeed = 1.0f;

    [NonSerialized] public Vector3 enemyMovePoint;

    [NonSerialized] public static int delay = 20;
    [NonSerialized] public float power = 1.0f;

    [NonSerialized] public bool isHit = false;
    [NonSerialized] public bool isSmash = false;
    [NonSerialized] public bool isRight = false;

    private bool normalShot;
    private bool fastShot;
    private bool lobShot;
    private bool dropShot;
    private int normalFrames;
    private int fastFrames;
    private int lobFrames;
    private int dropFrames;

    private BallController ballController;

    public GameObject gameManager;
    private GameManager manager;

    public GameObject proController;
    private ProControllerManager controllerManager;

    private AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip serveSound;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameManager.GetComponent<GameManager>();
        controllerManager = proController.GetComponent<ProControllerManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.active && !manager.isTOS && !Check("Ball"))
        {
            // Move players within a defined area
            Move(0.0f, 34.0f, 80.0f, -34.0f, 2.0f);

            // Extend the grace frame when hitting the ball back
            ExtendFrameEast();
            ExtendFrameSouth();
            ExtendFrameWest();
            ExtendFrameNorth();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") 
            && !isHit && manager.active && !manager.isTOS && !manager.isSERVE && manager.who != "Player")
        {
            if (normalShot) { ControlPlayer(other.gameObject, 0.40f, 25.0f); }
            else if (lobShot) { ControlPlayer(other.gameObject, 0.30f, 20.0f); }
            else if (fastShot) { ControlPlayer(other.gameObject, 0.50f, 15.0f); }
            else if (dropShot) { ControlPlayer(other.gameObject, 0.40f, 30.0f); }
        }

        // When in tos motion
        else if (other.gameObject.CompareTag("Ball") && !isHit && manager.active && manager.isTOS)
        {
            GameObject ball = other.gameObject;
            ballController = ball.GetComponent<BallController>();

            if (Input.GetKeyDown(KeyCode.Space) || controllerManager.PressEastButton())
            {
                // After pressing the button, return to the state where it cannot be pressed.
                controllerManager.isPressedEast = false;

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

    private void ControlPlayer(GameObject _ball, float _vz, float _frame)
    {
        GameObject ball = _ball;
        ballController = ball.GetComponent<BallController>();
        ballController.t = 0.0f;
        isHit = true;

        if (ball.transform.position.z < gameObject.transform.position.z && ball.transform.position.y < 9.0f) { isRight = false; }
        else if (ball.transform.position.z >= gameObject.transform.position.z && ball.transform.position.y < 9.0f) { isRight = true; }
        else if (ball.transform.position.y >= 9.0f) { isSmash = true; }

        if (normalShot) 
        { 
            normalShot = false;
            if (!manager.isSERVE) { NormalShot(power); }
        }
        else if (lobShot)
        {
            lobShot = false;
            if (!manager.isSERVE) { LobShot(power); }
        }
        else if (fastShot)
        {
            fastShot = false;
            if (!manager.isSERVE) { FastShot(power); }
        }
        else if (dropShot)
        {
            dropShot = false;
            if (!manager.isSERVE) { DropShot(power); }
        }

        // Determines how fast it moves sideways
        if (!manager.isSERVE && ballController.transform.position.x - gameObject.transform.position.x >= 0.0f)
        {
            ballController.vz = _vz * (2 * Convert.ToInt32(isRight) - 1)
                                                * (ballController.transform.position.x - gameObject.transform.position.x) / 12.0f;
        }
        else if (!manager.isSERVE && ballController.transform.position.x - gameObject.transform.position.x < 0.0f)
        {
            ballController.vz = _vz * (2 * Convert.ToInt32(isRight) - 1)
                                                 *(ballController.transform.position.x - gameObject.transform.position.x) / 12.0f;
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
        }));

        if (!manager.isOUT) manager.who = "Player";
        manager.ballCount = 0;
    }

    private void Serve(float _power)
    {
        audioSource.PlayOneShot(serveSound);
        ballController.vx = -_power * 1.8f;
        ballController.vy = -Mathf.Sin(Mathf.PI / 24.0f);

        if (Input.GetKey(KeyCode.A)) { ballController.vz = (-0.50f * manager.whereServe - 0.20f) * _power; }
        else if (Input.GetKey(KeyCode.D)) { ballController.vz = (-0.50f * manager.whereServe + 0.20f) * _power; }
        else { ballController.vz = -0.50f * manager.whereServe * _power; }

        // If a game controller is connected
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftStick.ReadValue().x > 0.10f)
            {
                ballController.vz = -0.50f * manager.whereServe + 0.20f * Gamepad.current.leftStick.ReadValue().x;
            }
            else if (Gamepad.current.leftStick.ReadValue().x < -0.10f)
            {
                ballController.vz = -0.50f * manager.whereServe + 0.20f * Gamepad.current.leftStick.ReadValue().x;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) { /* Nothing to do */ }
            else { ballController.vz = -0.50f * manager.whereServe; }
        }

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
            // Add "_frame" distance traveled to hit a little time after bounce.
            enemyMovePoint.x = boundPointX + ballSpeedX * 20.0f;

            enemyMovePoint.z = ballPosZ + arriveTimeZ * ballSpeedZ;
            if (enemyMovePoint.z < -32.0f) { enemyMovePoint.z = -32.0f; }
            else if (32.0f < enemyMovePoint.z) { enemyMovePoint.z = 32.0f; }
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

        ballController.vx = -_power * 1.85f;
        ballController.vy = Mathf.Sin(Mathf.PI / 11.0f);
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

    private void Move(float begin_x, float begin_z, float end_x, float end_z, float margin)
    {
        speedX *= 0.90f;
        speedZ *= 0.90f;

        if (Input.GetKey(KeyCode.D))
        {
            // move left
            speedZ = setPlayerSpeed; 
            if (transform.position.z + speedZ < begin_z - margin) { transform.Translate(0.0f, 0.0f, speedZ); }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // move right
            speedZ = -setPlayerSpeed; 
            if (transform.position.z + speedZ > end_z + margin) { transform.Translate(0.0f, 0.0f, speedZ); }
        }

        if (Input.GetKey(KeyCode.W))
        {
            // move front
            speedX = -setPlayerSpeed; 
            if (transform.position.x + speedX > begin_x + margin) { transform.Translate(speedX, 0.0f, 0.0f); }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // move back
            speedX = setPlayerSpeed / 2.0f; 
            if (transform.position.x + speedX < end_x - margin) { transform.Translate(speedX, 0.0f, 0.0f); }
        }

        // If a game controller is connected
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftStick.ReadValue().x > 0.10f)
            {
                // move left
                speedZ = setPlayerSpeed; 
                if (transform.position.z + speedZ * Gamepad.current.leftStick.ReadValue().x < begin_z - margin)
                {
                    transform.Translate(0.0f, 0.0f, speedZ * Gamepad.current.leftStick.ReadValue().x);
                }
            }
            else if (Gamepad.current.leftStick.ReadValue().x < -0.10f)
            {
                // move right
                speedZ = setPlayerSpeed; 
                if (transform.position.z + speedZ * Gamepad.current.leftStick.ReadValue().x > end_z + margin)
                {
                    transform.Translate(0, 0, speedZ * Gamepad.current.leftStick.ReadValue().x);
                }
            }

            if (Gamepad.current.leftStick.ReadValue().y > 0.10f)
            {
                // move front
                speedX = setPlayerSpeed; 
                if (transform.position.x + speedX * Gamepad.current.leftStick.ReadValue().y < end_x - margin)
                {
                    transform.Translate(-speedX * Gamepad.current.leftStick.ReadValue().y, 0.0f, 0.0f);
                }
            }
            else if (Gamepad.current.leftStick.ReadValue().y < -0.10f)
            {
                // move back
                speedX = setPlayerSpeed / 2.0f; 
                if (transform.position.x + speedX * Gamepad.current.leftStick.ReadValue().y > begin_x + margin)
                {
                    transform.Translate(-speedX * Gamepad.current.leftStick.ReadValue().y, 0.0f, 0.0f);
                }
            }
        }
    }

    private bool Check(string _tagName)
    {
        GameObject[] _tagObjects = GameObject.FindGameObjectsWithTag(_tagName);
        if (_tagObjects.Length == 0) { return true; }
        else { return false; }
    }

    private void ExtendFrameEast()
    {
        if (normalShot)
        {
            lobShot = false;
            fastShot = false;
            dropShot = false;

            normalFrames--;
            if (normalFrames < 0) { normalShot = false; }
        }
        else if (!normalShot)
        {
            normalFrames = 30;
            normalShot = Input.GetKeyDown(KeyCode.L) || controllerManager.PressEastButton();
        }
    }

    private void ExtendFrameSouth()
    {
        if (lobShot)
        {
            normalShot = false;
            fastShot = false;
            dropShot = false;

            lobFrames--;
            if (lobFrames < 0) { lobShot = false; }
        }
        else if (!lobShot)
        {
            lobFrames = 30;
            lobShot = Input.GetKeyDown(KeyCode.K) || controllerManager.PressSouthButton();
        }
    }

    private void ExtendFrameWest()
    {
        if (dropShot)
        {
            normalShot = false;
            fastShot = false;
            lobShot = false;

            dropFrames--;
            if (dropFrames < 0) { dropShot = false; }
        }
        else if (!dropShot)
        {
            dropFrames = 30;
            dropShot = Input.GetKeyDown(KeyCode.J) || controllerManager.PressWestButton();
        }
    }

    private void ExtendFrameNorth()
    {
        if (fastShot)
        {
            normalShot = false;
            lobShot = false;
            dropShot = false;

            fastFrames--;
            if (fastFrames < 0) { fastShot = false; }
        }
        else if (!fastShot)
        {
            fastFrames = 30;
            fastShot = Input.GetKeyDown(KeyCode.I) || controllerManager.PressNorthButton();
        }
    }

    private IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++) { yield return null; }
        action();
    }
}
