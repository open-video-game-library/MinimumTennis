using System.Collections;
using UnityEngine;
using System;

public class PlayerAutoControllAI : MonoBehaviour
{
    private int delay = 20;

    [SerializeField]
    private GameObject opponent;
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
    [NonSerialized]
    public int lateralDirection;
    [NonSerialized]
    public bool takebackLeft;
    [NonSerialized]
    public bool takebackRight;

    private string lastShooter;

    // Start is called before the first frame update
    void Start()
    {
        delay = PlayerReactionDelay.delay;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.inPlay)
        {
            ResetDecidedTakeback();
            return;
        }
        delay = PlayerReactionDelay.delay;

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
        
        if (lastShooter != opponent.name && GameManager.lastShooter == opponent.name)
        {
            // 対戦相手がボールを打ち返した瞬間
            Vector3 ballArrivalPoint = CalculateBallArrivalPoint();
            if (GameManager.courtAreaBegin.x - 5.0f <= ballArrivalPoint.x && ballArrivalPoint.x <= GameManager.courtAreaEnd.x + 5.0f
                && GameManager.courtAreaBegin.z - 5.0f <= ballArrivalPoint.z && ballArrivalPoint.z <= GameManager.courtAreaEnd.z + 5.0f)
            {
                returnBall = true;
                nextTargetPosition = ballArrivalPoint + CalculateExtraDistance();
                UpdateTargetPosition(nextTargetPosition, delay);
            }
            else { returnBall = false; }
            DecideTakeback();
        }
        else if (lastShooter != name && GameManager.lastShooter == name)
        {
            // 自分がボールを打ち返した瞬間
            nextTargetPosition = OptimizePosition();
            UpdateTargetPosition(nextTargetPosition, delay * 1.50f);
            ResetDecidedTakeback();
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

        if (predictedBallArrivalPoint.z > 0) { optimizedPosition = new Vector3(52.0f, 0.0f, 4.0f); }
        else { optimizedPosition = new Vector3(52.0f, 0.0f, -4.0f); }

        return optimizedPosition;
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

    private void DecideTakeback()
    {
        if (!returnBall) { return; }

        if (transform.position.z < CalculateBallArrivalPoint().z)
        {
            // フォアで打ち返す
            lateralDirection = 1;
            takebackLeft = false;
            takebackRight = true;
        }
        else
        {
            // バックで打ち返す
            lateralDirection = -1;
            takebackLeft = true;
            takebackRight = false;
        }
    }

    private void ResetDecidedTakeback()
    {
        takebackLeft = false;
        takebackRight = false;
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
