using UnityEngine;

public class FirstPlayerShotController : MonoBehaviour
{
    private float playerBallSpeed = 1.0f;
    private int lateralDirection;
    private bool isHit = false;
    private string lastShooter;

    private BallController ballController;
    private RacketAnimationController racketController;

    [SerializeField]
    private GameObject inputManager;
    private MultipleGamepadManager gamepad;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] hitSounds;
    [SerializeField]
    private AudioClip serveSound;

    // Start is called before the first frame update
    void Start()
    {
        gamepad = inputManager.GetComponent<MultipleGamepadManager>();
        racketController = GetComponentInChildren<RacketAnimationController>();
        audioSource = GetComponent<AudioSource>();

        playerBallSpeed = PlayerBallSpeed.playerBallSpeed;
    }

    void LateUpdate()
    {
        if (GameManager.foul != "NO FOUL" || !GameManager.inPlay)
        {
            lastShooter = null;
            return;
        }
        if (GameManager.lastShooter != name && lastShooter == name)
        {
            GameManager.lastShooter = lastShooter;
            lastShooter = null;
        }
        else { lastShooter = null; }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Ball")) { return; }
        if (isHit) { return; }

        GameObject ball = other.gameObject;
        ballController = ball.GetComponent<BallController>();

        playerBallSpeed = PlayerBallSpeed.playerBallSpeed;
        if (GameManager.inPlay && !GameManager.isToss && GameManager.isServeIn && GameManager.lastShooter != name)
        {
            if (ball.transform.position.z < transform.position.z) { lateralDirection = -1; }
            else if (ball.transform.position.z >= transform.position.z) { lateralDirection = 1; }

            float ballHight = ballController.transform.position.y;
            if (transform.position.x < GameManager.courtAreaEnd.x / 2.0f
                && ballHight > transform.localScale.y * 3.5f)
            {
                if (gamepad.isAnyButtonPressed[0]) { Smash(); }
            }
            else if (transform.position.x < GameManager.courtAreaEnd.x / 2.0f
                && ballHight > transform.localScale.y * 0.70f)
            {
                if (gamepad.isAnyButtonPressed[0]) { Volley(); }
            }
            else
            {
                if (gamepad.isPressedEast[0]) { NormalShot(); }
                else if (gamepad.isPressedSouth[0]) { LobShot(); }
                else if (gamepad.isPressedNorth[0]) { FastShot(); }
                else if (gamepad.isPressedWest[0]) { DropShot(); }
            }
        }
        else if (GameManager.inPlay && GameManager.isToss)
        {
            if (gamepad.isPressedEast[0]) { Serve(); }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            isHit = false;
        }
    }

    private void Serve()
    {
        isHit = true;
        racketController.AnimateServe();
        audioSource.PlayOneShot(serveSound);

        float lateralSpeed = -25.0f * GameManager.servePosition;
        if (gamepad.isConnected[0])
        {
            float leftStickX = gamepad.leftStickValue[0].x;
            if (leftStickX > 0.10f) { lateralSpeed = -25.0f * GameManager.servePosition + 6.0f * leftStickX; }
            else if (leftStickX < -0.10f) { lateralSpeed = -25.0f * GameManager.servePosition + 6.0f * leftStickX; }
        }

        ballController.time = 0.0f;

        ballController.speedX = -100.0f;
        ballController.speedY = -6.0f;
        ballController.speedZ = lateralSpeed;

        ballController.ballSpeed = playerBallSpeed;
        GameManager.isToss = false;
        if (GameManager.foul == "NO FOUL") { lastShooter = name;}
    }

    private void NormalShot()
    {
        isHit = true;
        if (lateralDirection == 1) { racketController.AnimateFore(); }
        else if (lateralDirection == -1) { racketController.AnimateBack(); }
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -65.0f;
        ballController.speedY = 25.0f;
        ballController.speedZ = 2.0f * lateralSpeedMagnification;

        ballController.ballSpeed = playerBallSpeed;
        GameManager.ballBoundCount = 0;
        GameManager.rallyCount++;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }

    private void LobShot()
    {
        isHit = true;
        if (lateralDirection == 1) { racketController.AnimateFore(); }
        else if (lateralDirection == -1) { racketController.AnimateBack(); }
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -45.0f;
        ballController.speedY = 45.0f;
        ballController.speedZ = 1.25f * lateralSpeedMagnification;

        ballController.ballSpeed = playerBallSpeed;
        GameManager.ballBoundCount = 0;
        GameManager.rallyCount++;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }

    private void FastShot()
    {
        isHit = true;
        if (lateralDirection == 1) { racketController.AnimateFore(); }
        else if (lateralDirection == -1) { racketController.AnimateBack(); }
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -90.0f;
        ballController.speedY = 17.0f;
        ballController.speedZ = 2.5f * lateralSpeedMagnification;

        ballController.ballSpeed = playerBallSpeed;
        GameManager.ballBoundCount = 0;
        GameManager.rallyCount++;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }

    private void DropShot()
    {
        isHit = true;
        if (lateralDirection == 1) { racketController.AnimateFore(); }
        else if (lateralDirection == -1) { racketController.AnimateBack(); }
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -48.0f;
        ballController.speedY = 25.0f;
        ballController.speedZ = 2.0f * lateralSpeedMagnification;

        ballController.ballSpeed = playerBallSpeed;
        GameManager.ballBoundCount = 0;
        GameManager.rallyCount++;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }

    private void Smash()
    {
        isHit = true;
        if (lateralDirection == 1) { racketController.AnimateServe(); }
        else if (lateralDirection == -1) { racketController.AnimateServe(); }
        audioSource.PlayOneShot(serveSound);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -120.0f;
        ballController.speedY = -20.0f;
        ballController.speedZ = 4.0f * lateralSpeedMagnification;

        ballController.ballSpeed = playerBallSpeed;
        GameManager.ballBoundCount = 0;
        GameManager.rallyCount++;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }

    private void Volley()
    {
        isHit = true;
        if (lateralDirection == 1) { racketController.AnimateFore(); }
        else if (lateralDirection == -1) { racketController.AnimateBack(); }
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -55.0f;
        ballController.speedY = 5.0f;
        ballController.speedZ = 2.0f * lateralSpeedMagnification;

        ballController.ballSpeed = playerBallSpeed;
        GameManager.ballBoundCount = 0;
        GameManager.rallyCount++;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }
}
