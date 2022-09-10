using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SecondPlayerController : MonoBehaviour
{
    [NonSerialized] public static float setPlayerSpeed = 0.50f;
    [NonSerialized] public static float playerBallSpeed = 1.0f;

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

    private CompetitionBallController ballController;

    public GameObject gameManager;
    private CompetitionGameManager manager;

    public GameObject proController;
    private CompetitionGameControllerManager controllerManager;

    private AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip serveSound;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameManager.GetComponent<CompetitionGameManager>();
        controllerManager = proController.GetComponent<CompetitionGameControllerManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.active && !manager.isTOS && !manager.Check("Ball"))
        {
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
            && !isHit && manager.active && !manager.isTOS && !manager.isSERVE && manager.who != "Player2")
        {
            if (normalShot) { ControlPlayer(other.gameObject, 0.40f); }
            else if (lobShot) { ControlPlayer(other.gameObject, 0.30f); }
            else if (fastShot) { ControlPlayer(other.gameObject, 0.50f); }
            else if (dropShot) { ControlPlayer(other.gameObject, 0.40f); }
        }

        // When in tos motion
        else if (other.gameObject.CompareTag("Ball") && !isHit && manager.active && manager.isTOS)
        {
            GameObject ball = other.gameObject;
            ballController = ball.GetComponent<CompetitionBallController>();

            if (Input.GetKeyDown(KeyCode.Space) || controllerManager.PressEastButton(1))
            {
                // After pressing the button, return it to the state where it cannot be pressed.
                controllerManager.isPressedEast[1] = false;

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

    private void ControlPlayer(GameObject _ball, float _vz)
    {
        GameObject ball = _ball;
        ballController = ball.GetComponent<CompetitionBallController>();
        ballController.t = 0.0f;
        isHit = true;

        if (ball.transform.position.z < gameObject.transform.position.z && ball.transform.position.y < 9.0f) { isRight = true; }
        else if (ball.transform.position.z >= gameObject.transform.position.z && ball.transform.position.y < 9.0f) { isRight = false; }
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
        if (!manager.isSERVE && ballController.transform.position.x - gameObject.transform.position.x <= 0.0f)
        {
            ballController.vz = _vz * (2.0f * Convert.ToInt32(isRight) - 1.0f)
                                                * (ballController.transform.position.x - gameObject.transform.position.x) / 12.0f;
        }
        else if (!manager.isSERVE && ballController.transform.position.x - gameObject.transform.position.x > 0.0f)
        {
            ballController.vz = _vz * (2.0f * Convert.ToInt32(isRight) - 1.0f)
                                                 * (ballController.transform.position.x - gameObject.transform.position.x) / 12.0f;
        }
        else if (!manager.isSERVE) { ballController.vz = 0.0f; }

        if (!manager.isOUT) manager.who = "Player2";
        manager.ballCount = 0;
    }

    private void Serve(float _power)
    {
        audioSource.PlayOneShot(serveSound);
        ballController.vx = _power * 1.8f;
        ballController.vy = -Mathf.Sin(Mathf.PI / 24);

        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player2";

        // If a game controller is connected
        if (controllerManager.gamepadCount == 0) { return; }

        if (controllerManager.leftStick[1].x > 0.10f)
        {
            ballController.vz = 0.50f * manager.whereServe - 0.20f * controllerManager.leftStick[1].x;
        }
        else if (controllerManager.leftStick[1].x < -0.10f)
        {
            ballController.vz = 0.50f * manager.whereServe - 0.20f * controllerManager.leftStick[1].x;
        }
        else { ballController.vz = 0.50f * manager.whereServe; }
    }

    private void NormalShot(float _power)
    {
        int num = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = _power * 1.1f;
        ballController.vy = Mathf.Sin(Mathf.PI / 6.0f);
        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player2";
        manager.rallyCount++;
    }

    private void LobShot(float _power)
    {
        int num = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = _power * 0.74f;
        ballController.vy = Mathf.Sin(Mathf.PI / 3.0f);
        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player2";
        manager.rallyCount++;
    }

    private void FastShot(float _power)
    {
        int num = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = _power * 1.85f;
        ballController.vy = Mathf.Sin(Mathf.PI / 11.0f);
        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player2";
        manager.rallyCount++;
    }

    private void DropShot(float _power)
    {
        int num = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.vx = _power * 0.80f;
        ballController.vy = Mathf.Sin(Mathf.PI / 6.0f);
        ballController.ballSpeed = playerBallSpeed;
        manager.who = "Player2";
        manager.rallyCount++;
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
            normalShot = Input.GetKeyDown(KeyCode.L) || controllerManager.PressEastButton(1);
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
            lobShot = Input.GetKeyDown(KeyCode.K) || controllerManager.PressSouthButton(1);
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
            dropShot = Input.GetKeyDown(KeyCode.J) || controllerManager.PressWestButton(1);
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
            fastShot = Input.GetKeyDown(KeyCode.I) || controllerManager.PressNorthButton(1);
        }
    }
}
