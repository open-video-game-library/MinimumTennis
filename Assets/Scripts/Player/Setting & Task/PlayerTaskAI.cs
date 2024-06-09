using System.Collections;
using UnityEngine;
using System;

public class PlayerTaskAI : MonoBehaviour
{
    // Editable Parameters
    private float delay = 0.30f;
    private float distance = 1.0f;

    // 対戦相手のプレイヤ
    private GameObject opponentPlayer;

    // 自由に移動できたり、ボールを自由に打ったりできるかどうか
    // RallyingモードとSettingモードではtrueになる
    [NonSerialized]
    public bool action = true;

    // ボール
    private GameObject ballObject;
    private TaskBallController ball;

    // 移動時に用いるパラメータ
    private bool isArrivedX = true;
    private bool isArrivedZ = true;
    private Vector3 targetPosition;

    // 返球時に用いるパラメータ
    private bool hitBall;
    private string lastShooter;

    // 自身が1Pか2Pかを格納
    [NonSerialized]
    public Players player;

    // 自動操作用パラメータ
    [NonSerialized]
    public bool autoMove;
    [NonSerialized]
    public bool autoShot;

    [NonSerialized]
    public float x;
    [NonSerialized]
    public float z;

    [NonSerialized]
    public bool shot;
    [NonSerialized]
    public Vector3 shotPower;

    [NonSerialized]
    public bool toss;

    [NonSerialized]
    public bool serve;

    // テイクバック時に用いるパラメータ
    [NonSerialized]
    public float autoMoveLateralDirection;
    [NonSerialized]
    public bool takebackBack;
    [NonSerialized]
    public bool takebackFore;

    // Start is called before the first frame update
    void Start()
    {
        delay = Parameters.reactionDelay[(int)player];
        distance = Parameters.distance[(int)player];

        if (player == Players.p1) { opponentPlayer = CharacterGenerator.GetCharacter((int)Players.p2); }
        else if (player == Players.p2) { opponentPlayer = CharacterGenerator.GetCharacter((int)Players.p1); }

        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        delay = Parameters.reactionDelay[(int)player];
        distance = Parameters.distance[(int)player];

        // 移動のみが自動化されているとき
        if (autoMove && !autoShot)
        {
            // 制御可能でないとき、テイクバック状態を解除する
            if (!TaskData.controllable) { ResetTakeback(); }
        }
        else
        {
            // 移動が自動化されていない場合は、テイクバック状態を解除する
            ResetTakeback();
        }
    }

    void LateUpdate()
    {
        DecideNextPosition();
        lastShooter = TaskData.lastShooter;
    }

    private void FixedUpdate()
    {
        if (action && (autoMove || autoShot))
        {
            // ボールを探し、変数に格納する
            if (CountObjectAmount("Ball") == 1 && !ballObject)
            {
                ballObject = GameObject.FindWithTag("Ball");
                ball = ballObject.GetComponent<TaskBallController>();
            }
            else if (CountObjectAmount("Ball") > 1)
            {
                // ボールが複数存在する場合は、最新のボールを変数に格納する
                GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Ball");
                ballObject = ballObjects[ballObjects.Length - 1];
                ball = ballObject.GetComponent<TaskBallController>();
            }
            else if (CountObjectAmount("Ball") == 0)
            {
                ballObject = null;
                ball = null;
            }
        }

        if (autoMove)
        {
            // 自動で移動する
            if (TaskData.controllable) { AutoMove(targetPosition); }
            else { targetPosition = transform.position; }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Ball")) { return; }

        if (autoShot)
        {
            // 自動でショットを打つ
            AutoShot();
        }
    }

    private void AutoMove(Vector3 ballArrivalPoint)
    {
        if (!action) { return; }

        float arrivalRange = 4.0f;
        Vector3 arrivalPointInPlayerLocal = transform.InverseTransformPoint(ballArrivalPoint);

        if (!isArrivedX && !(ballArrivalPoint.x - arrivalRange <= transform.position.x
            && transform.position.x <= ballArrivalPoint.x + arrivalRange))
        {
            // 右方向への移動
            if (arrivalPointInPlayerLocal.x > 0) { x = 1.0f; }
            // 左方向への移動
            else { x = -1.0f; }
        }
        else
        {
            isArrivedX = true;
            x = 0.0f;
        }

        if (!isArrivedZ && !(ballArrivalPoint.z - arrivalRange <= transform.position.z
            && transform.position.z <= ballArrivalPoint.z + arrivalRange))
        {
            // 前方向への移動
            if (arrivalPointInPlayerLocal.z > 0) { z = 1.0f; }
            // 後方向への移動
            else { z = -1.0f; }
        }
        else
        {
            isArrivedZ = true;
            z = 0.0f;
        }
    }

    private void AutoShot()
    {
        if (!action) { return; }

        shot = hitBall;

        if (hitBall)
        {
            shotPower = DecideBallSpeed();
            hitBall = false;
        }
    }

    private void DecideNextPosition()
    {
        if (!opponentPlayer || !ball) { return; }

        Vector3 nextTargetPosition;

        // 相手プレイヤがボールを打ち返した瞬間
        if (lastShooter != opponentPlayer.name && TaskData.lastShooter == opponentPlayer.name)
        {
            Vector3 ballArrivalPoint = CalculateBallArrivalPoint();

            if (TaskData.playersCourtArea[(int)player].CheckInside(ballArrivalPoint.x, ballArrivalPoint.z, 5.0f))
            {
                hitBall = true;
                nextTargetPosition = ballArrivalPoint + CalculateExtraDistance();
                UpdateTargetPosition(nextTargetPosition, delay);
            }
            else { hitBall = false; }

            // 移動のみが自動化されている場合、テイクバックする
            if (autoMove && !autoShot && hitBall) { DecideTakeback(ballArrivalPoint, delay); }
        }
        // 自分がボールを打ち返した瞬間
        else if (lastShooter != name && TaskData.lastShooter == name)
        {
            nextTargetPosition = OptimizePosition();
            UpdateTargetPosition(nextTargetPosition, delay * 1.50f);

            // 移動のみが自動化されている場合、テイクバック状態を解除する
            if (autoMove && !autoShot) { ResetTakeback(); }
        }
    }

    private Vector3 CalculateBallArrivalPoint()
    {
        Vector3 ballPosition = ball.transform.position;
        float ballRadius = ball.GetComponent<SphereCollider>().radius;
        float speedY = ball.SpeedY;
        float gravity = ball.Gravity;
        float arrivalTime = (speedY + Mathf.Sqrt(speedY * speedY + 2.0f * gravity * (ballPosition.y - ballRadius))) / gravity;

        float arrivalPointX = ball.SpeedX * arrivalTime + ballPosition.x;
        float arrivalPointZ = ball.SpeedZ * arrivalTime + ballPosition.z;
        Vector3 arrivalPoint = new Vector3(arrivalPointX, 0.0f, arrivalPointZ);

        return arrivalPoint;
    }

    private Vector3 CalculateExtraDistance()
    {
        float extraDistanceX = 0.30f * ball.SpeedX;
        float extraDistanceZ = 0.30f * ball.SpeedZ;
        Vector3 extraDistance = new Vector3(extraDistanceX, 0.0f, extraDistanceZ);

        return extraDistance;
    }

    private Vector3 OptimizePosition()
    {
        Vector3 predictedBallArrivalPoint = CalculateBallArrivalPoint();
        Vector3 optimizedPosition;

        if (predictedBallArrivalPoint.x > 0.0f) { optimizedPosition = new Vector3(-4.0f, 0.0f, -52.0f * transform.forward.z); }
        else { optimizedPosition = new Vector3(4.0f, 0.0f, -52.0f * transform.forward.z); }

        return optimizedPosition;
    }

    private Vector3 DecideBallSpeed()
    {
        Vector3 ballSpeed;
        Vector3 playerPosition = opponentPlayer.transform.position;
        Vector3 defaltTargetPoint;
        float randomNoise = UnityEngine.Random.Range(-3.0f, 3.0f);

        // 相手プレイヤーが前衛ポジションのとき
        if (TaskData.courtArea.zNegativeLimit / 2.0f < opponentPlayer.transform.position.z
            && opponentPlayer.transform.position.z < TaskData.courtArea.zPositiveLimit / 2.0f)
        {
            // Rallyingモード
            if (Parameters.taskType == TaskType.rallying)
            {
                if (playerPosition.x < 0.0f) { defaltTargetPoint = new Vector3(5.0f + randomNoise, 0.0f, 30.0f * transform.forward.z); }
                else { defaltTargetPoint = new Vector3(-5.0f + randomNoise, 0.0f, 30.0f * transform.forward.z); }
            }
            // Settingモード
            else
            {
                if (playerPosition.x < 0.0f) { defaltTargetPoint = new Vector3(12.0f, 0.0f, 40.0f * transform.forward.z); }
                else { defaltTargetPoint = new Vector3(-12.0f, 0.0f, 40.0f * transform.forward.z); }
            }
        }
        // 相手プレイヤーが後衛ポジションのとき
        else
        {
            // Rallyingモード
            if (Parameters.taskType == TaskType.rallying)
            {
                if (playerPosition.x < 0.0f) { defaltTargetPoint = new Vector3(5.0f + randomNoise, 0.0f, 25.0f * transform.forward.z); }
                else { defaltTargetPoint = new Vector3(-5.0f + randomNoise, 0.0f, 25.0f * transform.forward.z); }
            }
            // Settingモード
            else
            {
                if (playerPosition.x < 0.0f) { defaltTargetPoint = new Vector3(12.0f, 0.0f, 35.0f * transform.forward.z); }
                else { defaltTargetPoint = new Vector3(-12.0f, 0.0f, 35.0f * transform.forward.z); }
            }
        }

        Vector3 noise = Vector3.one;
        // Settingモード
        if (Parameters.taskType == TaskType.setting) { noise = GenerateNoise(); }

        ballSpeed.y = DecideUpwardSpeed();
        ballSpeed.x = DecideLateralSpeed(ballSpeed.y, defaltTargetPoint, noise.x);
        ballSpeed.z = DecideDepthSpeed(ballSpeed.y, defaltTargetPoint, noise.z);

        return ballSpeed;
    }

    private float DecideUpwardSpeed()
    {
        if (!isArrivedX && !isArrivedZ) { return UnityEngine.Random.Range(30.0f, 40.0f); }
        else if (isArrivedX && isArrivedZ)
        {
            // Rallyingモード
            if (Parameters.taskType == TaskType.rallying) { return UnityEngine.Random.Range(20.0f, 30.0f); }
            // Settingモード
            else
            {
                // プレイヤーが前衛ポジションのとき
                if (TaskData.courtArea.zNegativeLimit / 2.0f < opponentPlayer.transform.position.z
                && opponentPlayer.transform.position.z < TaskData.courtArea.zPositiveLimit / 2.0f) { return UnityEngine.Random.Range(45.0f, 50.0f); }
                // プレイヤーが後衛ポジションのとき
                else { return UnityEngine.Random.Range(20.0f, 30.0f); }
            }
        }
        else
        {
            // Rallyingモード
            if (Parameters.taskType == TaskType.rallying) { return UnityEngine.Random.Range(25.0f, 30.0f); }
            // Settingモード
            else
            {
                // プレイヤーが前衛ポジションのとき
                if (TaskData.courtArea.zNegativeLimit / 2.0f < opponentPlayer.transform.position.z
                && opponentPlayer.transform.position.z < TaskData.courtArea.zPositiveLimit / 2.0f) { return UnityEngine.Random.Range(35.0f, 45.0f); }
                // プレイヤーが後衛ポジションのとき
                else { return UnityEngine.Random.Range(25.0f, 30.0f); }
            }
        }
    }

    private float DecideLateralSpeed(float ballSpeedY, Vector3 targetPosition, float noise)
    {
        int random = UnityEngine.Random.Range(-1, 3);
        int randomTarget = random < 0 ? -1 : 1;

        Vector3 ballPosition = ball.transform.position;
        float gravity = ball.Gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disX;
        // Rallyingモード
        if (Parameters.taskType == TaskType.rallying) { disX = targetPosition.x * randomTarget - ball.transform.position.x; }
        // Settingモード
        else { disX = targetPosition.x * randomTarget * distance - ball.transform.position.x; }

        return (disX / arrivalTime) * noise;
    }

    private float DecideDepthSpeed(float ballSpeedY, Vector3 targetPosition, float noise)
    {
        Vector3 ballPosition = ball.transform.position;
        float gravity = ball.Gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disZ = targetPosition.z - ball.transform.position.z;

        return (disZ / arrivalTime) * noise;
    }

    private Vector3 GenerateNoise()
    {
        Vector3 noise = new Vector3(1.0f, 1.0f, 1.0f);

        if (!isArrivedX) { noise.x = UnityEngine.Random.Range(-0.50f, 1.0f); }
        if (!isArrivedZ) { noise.z = UnityEngine.Random.Range(0.80f, 1.0f); }

        float randomNoise = UnityEngine.Random.Range(0.0f, 1.0f);
        if (randomNoise < 0.20f && (isArrivedX || isArrivedZ))
        {
            noise.x = UnityEngine.Random.Range(-0.50f, 1.0f);
            noise.z = UnityEngine.Random.Range(0.90f, 1.0f);
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

    private IEnumerator DelayMethod(float delaySeconds, Action action)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < delaySeconds)
        {
            if (!TaskData.pause) { elapsedTime += Time.deltaTime; }
            if (!TaskData.controllable) { yield break; }
            yield return null;
        }

        action();
    }

    private int CountObjectAmount(string tagName)
    {
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(tagName);
        return tagObjects.Length;
    }

    private void DecideTakeback(Vector3 ballArrivalPoint, float delay)
    {
        StartCoroutine(DelayMethod(delay, () =>
        {
            Vector3 objectPositionInPlayerLocal = transform.InverseTransformPoint(ballArrivalPoint);

            // 右側で打ち返す
            if (objectPositionInPlayerLocal.x > 0.0f)
            {
                autoMoveLateralDirection = -1;
                takebackFore = Parameters.charactersDominantHand[0] == DominantHand.right;
                takebackBack = Parameters.charactersDominantHand[0] != DominantHand.right;
            }
            // 左側で打ち返す
            else
            {
                autoMoveLateralDirection = 1;
                takebackFore = Parameters.charactersDominantHand[0] != DominantHand.right;
                takebackBack = Parameters.charactersDominantHand[0] == DominantHand.right;
            }
        }));
    }

    private void ResetTakeback()
    {
        autoMoveLateralDirection = 0.0f;
        takebackBack = false;
        takebackFore = false;
    }
}
