using System.Collections;
using UnityEngine;
using System;

public class OpponentAI : MonoBehaviour
{
    private int delay = 20;
    private float distance = 0.50f;

    [SerializeField]
    private GameObject player;
    private GameObject ball;
    private BallController ballController;

    [NonSerialized]
    public bool isArrivedX;
    [NonSerialized]
    public bool isArrivedZ;
    [NonSerialized]
    public bool returnBall;
    [NonSerialized]
    public Vector3 targetPosition;

    private string lastShooter;

    // Start is called before the first frame update
    void Start()
    {
        delay = OpponentReactionDelay.delay;
        distance = Distance.distance;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.inPlay) { return; }
        delay = OpponentReactionDelay.delay;
        distance = Distance.distance;

        if (CountObjectAmount("Ball") == 1 && !ball)
        {
            ball = GameObject.FindWithTag("Ball");
            ballController = ball.GetComponent<BallController>();
        }
        else if (CountObjectAmount("Ball") == 0)
        {
            ball = null;
            ballController = null;
        }
    }

    void LateUpdate()
    {
        DecideNextPosition();
        lastShooter = GameManager.lastShooter;
    }

    private void DecideNextPosition()
    {
        Vector3 nextTargetPosition;

        if (lastShooter != player.name && GameManager.lastShooter == player.name)
        {
            // The moment the player hits the ball back
            Vector3 ballArrivalPoint = CalculateBallArrivalPoint();
            if (GameManager.courtAreaBegin.x - 5.0f <= ballArrivalPoint.x && ballArrivalPoint.x <= GameManager.courtAreaEnd.x + 5.0f
                && GameManager.courtAreaBegin.z - 5.0f <= ballArrivalPoint.z && ballArrivalPoint.z <= GameManager.courtAreaEnd.z + 5.0f)
            {
                returnBall = true;
                nextTargetPosition = ballArrivalPoint + CalculateExtraDistance();
                UpdateTargetPosition(nextTargetPosition, delay);
            }
            else { returnBall = false; }
        }
        else if (lastShooter != name && GameManager.lastShooter == name)
        {
            // The moment the opponent hits the ball back
            nextTargetPosition = OptimizePosition();
            UpdateTargetPosition(nextTargetPosition, delay * 1.50f);
        }
    }

    private Vector3 CalculateBallArrivalPoint()
    {
        if (!ball) { return new Vector3(-52.0f, 0.0f, 0.0f); }
        Vector3 ballPosition = ball.transform.position;
        float ballRadius = ball.GetComponent<SphereCollider>().radius;
        float speedY = ballController.speedY;
        float gravity = ballController.gravity;
        float arrivalTime = (speedY + Mathf.Sqrt(speedY * speedY - 2.0f * gravity * (ballRadius - ballPosition.y))) / gravity;

        float arrivalPointX = ballController.speedX * arrivalTime + ballPosition.x;
        float arrivalPointZ = ballController.speedZ * arrivalTime + ballPosition.z;
        Vector3 arrivalPoint = new Vector3(arrivalPointX, 0.0f, arrivalPointZ);
        
        return arrivalPoint;
    }

    private Vector3 CalculateExtraDistance()
    {
        float extraDistanceX = 0.30f * ballController.speedX;
        float extraDistanceZ = 0.30f * ballController.speedZ;
        Vector3 extraDistance = new Vector3(extraDistanceX, 0.0f, extraDistanceZ);

        return extraDistance;
    }

    private Vector3 OptimizePosition()
    {
        Vector3 predictedBallArrivalPoint = CalculateBallArrivalPoint();
        Vector3 optimizedPosition;

        if (predictedBallArrivalPoint.z > 0) { optimizedPosition = new Vector3(-52.0f, 0.0f, -4.0f); }
        else { optimizedPosition = new Vector3(-52.0f, 0.0f, 4.0f); }

        return optimizedPosition;
    }

    public Vector3 DecideBallSpeed()
    {
        Vector3 ballSpeed;
        Vector3 playerPosition = player.transform.position;
        Vector3 defaltTargetPoint;
        if (player.transform.position.x < GameManager.courtAreaEnd.x / 2.0f)
        {
            if (playerPosition.z < 0.0f) { defaltTargetPoint = new Vector3(40.0f, 0.0f, 12.0f); }
            else { defaltTargetPoint = new Vector3(40.0f, 0.0f, -12.0f); }
        }
        else
        {
            if (playerPosition.z < 0.0f) { defaltTargetPoint = new Vector3(35.0f, 0.0f, 12.0f); }
            else { defaltTargetPoint = new Vector3(35.0f, 0.0f, -12.0f); }
        }

        Vector3 noise = GenerateNoise();
        ballSpeed.y = DecideUpwardSpeed();
        ballSpeed.x = DecideDepthSpeed(ballSpeed.y, defaltTargetPoint, noise.x);
        ballSpeed.z = DecideLateralSpeed(ballSpeed.y, defaltTargetPoint, noise.z);

        return ballSpeed;
    }

    private float DecideUpwardSpeed()
    {
        if (!isArrivedX && !isArrivedZ) { return UnityEngine.Random.Range(30.0f, 40.0f); }
        else if (isArrivedX && isArrivedZ)
        {
            if (player.transform.position.x < GameManager.courtAreaEnd.x / 2.0f) { return UnityEngine.Random.Range(45.0f, 50.0f); }
            else { return UnityEngine.Random.Range(20.0f, 30.0f); }
        }
        else
        {
            if (player.transform.position.x < GameManager.courtAreaEnd.x / 2.0f) { return UnityEngine.Random.Range(35.0f, 45.0f); }
            else { return UnityEngine.Random.Range(25.0f, 30.0f); }
        }
    }

    private float DecideDepthSpeed(float ballSpeedY, Vector3 targetPosition, float noise)
    {
        Vector3 ballPosition = ball.transform.position;
        float gravity = ballController.gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disX = targetPosition.x - ball.transform.position.x;
        return (disX / arrivalTime) * noise;
    }

    private float DecideLateralSpeed(float ballSpeedY, Vector3 targetPosition, float noise)
    {        
        int random = UnityEngine.Random.Range(-1, 3);
        int randomTarget;
        if (random < 0) { randomTarget = -1; }
        else { randomTarget = 1; }

        Vector3 ballPosition = ball.transform.position;
        float gravity = ballController.gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disZ = targetPosition.z * randomTarget * distance - ball.transform.position.z;
        return (disZ / arrivalTime) * noise;
    }

    private Vector3 GenerateNoise()
    {
        Vector3 noise = new Vector3(1.0f, 1.0f, 1.0f);

        if (!isArrivedX) { noise.x = UnityEngine.Random.Range(0.80f, 1.0f); }
        if (!isArrivedZ) { noise.z = UnityEngine.Random.Range(-0.50f, 1.0f); }

        float randomNoise = UnityEngine.Random.Range(0.0f, 1.0f);
        if (randomNoise < 0.20f && (isArrivedX || isArrivedZ))
        {
            noise.x = UnityEngine.Random.Range(0.90f, 1.0f);
            noise.z = UnityEngine.Random.Range(-0.50f, 1.0f);
        }

        return noise;
    }

    private void UpdateTargetPosition(Vector3 nextTargetPosition, float delay)
    {
        StartCoroutine(DelayMethod(delay, () =>
        {
            isArrivedX = false;
            isArrivedZ = false;
            targetPosition = nextTargetPosition;
        }));
    }

    private IEnumerator DelayMethod(float delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            if (!GameManager.inPlay) { yield break; }
            yield return null;
        }
        action();
    }

    private int CountObjectAmount(string tagName)
    {
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(tagName);
        return tagObjects.Length;
    }
}
