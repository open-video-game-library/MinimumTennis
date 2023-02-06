using UnityEngine;

public class OpponentShotController : MonoBehaviour
{
    private float opponentBallSpeed = 1.0f;
    private int lateralDirection;
    private bool isHit = false;
    private string lastShooter;

    [SerializeField]
    private GameObject player;
    private OpponentAI ai;
    private BallController ballController;
    private RacketAnimationController racketController;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] hitSounds;
    [SerializeField]
    private AudioClip serveSound;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<OpponentAI>();
        racketController = GetComponentInChildren<RacketAnimationController>();
        audioSource = GetComponent<AudioSource>();

        opponentBallSpeed = OpponentBallSpeed.opponentBallSpeed;
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

        opponentBallSpeed = OpponentBallSpeed.opponentBallSpeed;
        if (GameManager.inPlay && !GameManager.isToss && GameManager.isServeIn && GameManager.lastShooter != name)
        {
            if (ball.transform.position.z < transform.position.z) { lateralDirection = 1; }
            else if (ball.transform.position.z >= transform.position.z) { lateralDirection = -1; }

            if (ai.returnBall) { Shot(); }
        }
        else if (GameManager.inPlay && GameManager.isToss)
        {
            if (ball.transform.position.y >= 14.0f) { Serve(); }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball")) { isHit = false; }
    }

    private void Serve()
    {
        isHit = true;
        racketController.AnimateServe();
        audioSource.PlayOneShot(serveSound);

        int random = Random.Range(-1, 2);
        float lateralSpeed = 25.0f * GameManager.servePosition + 6.0f * random;

        ballController.time = 0.0f;
        ballController.speedX = 100.0f;
        ballController.speedY = -6.0f;
        ballController.speedZ = lateralSpeed;

        ballController.ballSpeed = opponentBallSpeed;
        GameManager.isToss = false;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }

    private void Shot()
    {
        isHit = true;
        if (lateralDirection == 1) { racketController.AnimateFore(); }
        else if (lateralDirection == -1) { racketController.AnimateBack(); }
        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ballController.time = 0.0f;

        Vector3 ballSpeed = ai.DecideBallSpeed();
        ballController.speedX = ballSpeed.x;
        ballController.speedY = ballSpeed.y;
        ballController.speedZ = ballSpeed.z;

        ballController.ballSpeed = opponentBallSpeed;
        GameManager.ballBoundCount = 0;
        GameManager.rallyCount++;
        if (GameManager.foul == "NO FOUL") { lastShooter = name; }
    }
}
