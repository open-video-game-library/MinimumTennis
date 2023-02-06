using UnityEngine;

public class PlayerMotionShotController : MonoBehaviour
{
    private float playerBallSpeed = 1.0f;
    private int lateralDirection;
    private bool isHit = false;
    private string lastShooter;

    private PlayerAutoControllAI ai;
    private BallController ballController;
    private RacketMotionAnimationController racketController;

    [SerializeField]
    private GameObject inputManager;
    private JoyconInputManager joycon;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] hitSounds;
    [SerializeField]
    private AudioClip serveSound;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<PlayerAutoControllAI>();
        racketController = GetComponentInChildren<RacketMotionAnimationController>();
        joycon = inputManager.GetComponent<JoyconInputManager>();
        audioSource = GetComponent<AudioSource>();

        playerBallSpeed = PlayerBallSpeed.playerBallSpeed;
    }

    void Update()
    {
        if (!GameManager.inPlay) { racketController.ResetTakeback(); }
        else
        {
            if (ai.takebackLeft) { racketController.AnimateBackTakeback(); }
            else if (ai.takebackRight) { racketController.AnimateForeTakeback(); }
            else { racketController.ResetTakeback(); }
        }
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
            lateralDirection = ai.lateralDirection;

            if (joycon.isPressedSouth && joycon.anySwing) { LobShot(); }
            else if (joycon.isPressedWest && joycon.anySwing) { DropShot(); }
            else if (joycon.isPressedNorth && joycon.anySwing) { FastShot(); }
            else if ((joycon.isPressedEast && joycon.anySwing) || joycon.anySwing) { NormalShot(); }
        }
        else if (GameManager.inPlay && GameManager.isToss)
        {
            if (joycon.anySwing) { Serve(); }
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
        if (joycon.isPressedWest) { lateralSpeed = -25.0f * GameManager.servePosition - 6.0f; }
        else if (joycon.isPressedEast) { lateralSpeed = -25.0f * GameManager.servePosition + 6.0f; }

        ballController.time = 0.0f;
        ballController.speedX = -100.0f;
        ballController.speedY = -6.0f;
        ballController.speedZ = lateralSpeed;

        ballController.ballSpeed = playerBallSpeed;
        GameManager.isToss = false;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }

    private void NormalShot()
    {
        isHit = true;
        if (lateralDirection == 1) { racketController.AnimateFore(); }
        else if (lateralDirection == -1) { racketController.AnimateBack(); }
        racketController.ResetTakeback();
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -65.0f;
        ballController.speedY = 25.0f;
        ballController.speedZ = 2.0f * lateralSpeedMagnification;
        AdjustBallSpeed(ballController);

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
        racketController.ResetTakeback();
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -45.0f;
        ballController.speedY = 45.0f;
        ballController.speedZ = 1.25f * lateralSpeedMagnification;
        AdjustBallSpeed(ballController);

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
        racketController.ResetTakeback();
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -100.0f;
        ballController.speedY = 13.0f;
        ballController.speedZ = 2.5f * lateralSpeedMagnification;
        AdjustBallSpeed(ballController);

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
        racketController.ResetTakeback();
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballController.transform.position.x - transform.position.x);

        ballController.time = 0.0f;

        ballController.speedX = -48.0f;
        ballController.speedY = 25.0f;
        ballController.speedZ = 2.0f * lateralSpeedMagnification;
        AdjustBallSpeed(ballController);

        ballController.ballSpeed = playerBallSpeed;
        GameManager.ballBoundCount = 0;
        GameManager.rallyCount++;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }

    private void AdjustBallSpeed(BallController ballController)
    {
        Vector3 ballPosition = ballController.transform.position;

        float speedY = ballController.speedY;
        float gravity = ballController.gravity;
        float arrivalTime = (speedY + Mathf.Sqrt(speedY * speedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float arrivalPointX = ballController.speedX * arrivalTime + ballPosition.x;
        float arrivalPointZ = ballController.speedZ * arrivalTime + ballPosition.z;

        Vector3 areaBegin = GameManager.courtAreaBegin;
        Vector3 areaEnd = GameManager.courtAreaEnd;

        float margin = 5.0f;
        if (areaBegin.x - 10.0f < arrivalPointX && arrivalPointX < areaBegin.x)
        {
            areaBegin.x += margin;
            ballController.speedX = (areaBegin.x - ballPosition.x) / arrivalTime;
        }

        if (areaEnd.z < arrivalPointZ && arrivalPointZ < areaEnd.z + 10.0f)
        {
            areaEnd.z -= margin;
            ballController.speedZ = (areaEnd.z - ballPosition.z) / arrivalTime;
        }
        else if (areaBegin.z - 10.0f < arrivalPointZ && arrivalPointZ < areaBegin.z)
        {
            areaBegin.z += margin;
            ballController.speedZ = (areaBegin.z - ballPosition.z) / arrivalTime;
        }
    }
}
