using UnityEngine;

public class TaskBallController : MonoBehaviour, IBallController
{
    public float Gravity { get; set; }

    public float SpeedX { get; set; }

    public float SpeedY { get; set; }

    public float SpeedZ { get; set; }

    public float Time { get; set; }

    public float BallSpeed { get; set; }

    private GameObject courtCanvas;
    [SerializeField]
    private GameObject arrivalPointPrefab;
    private GameObject arrivalPointMark;

    private float moveSpeedX;
    private float moveSpeedY;
    private float moveSpeedZ;

    private bool isCourtIn;

    private int ballBoundCount;

    [System.NonSerialized]
    public bool canHit;
    [System.NonSerialized]
    public bool isHitByPlayer;

    private bool isBoundedPlayerBall;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip boundSound;

    private void Awake()
    {
        Gravity = 49.0f;
        Time = 0.0f;
        BallSpeed = 1.0f;

        courtCanvas = GameObject.Find("CourtCanvas");
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TaskData.taskState != TaskState.Playing) { Destroy(gameObject); }

        if (Parameters.taskType == TaskType.moving || Parameters.taskType == TaskType.hitting) { canHit = ballBoundCount == 1; }
        else if (Parameters.taskType == TaskType.rallying || Parameters.taskType == TaskType.setting) { canHit = ballBoundCount < 2; }

        // 着弾点のマークがある場合
        if (ballBoundCount > 0 && arrivalPointMark) { Destroy(arrivalPointMark); }

        isCourtIn = TaskData.courtArea.CheckInside(transform.position.x, transform.position.z);

        moveSpeedX = SpeedX;
        moveSpeedY = SpeedY - Gravity * Time;
        moveSpeedZ = SpeedZ;

        transform.Translate(
            moveSpeedX * BallSpeed * UnityEngine.Time.deltaTime,
            moveSpeedY * BallSpeed * UnityEngine.Time.deltaTime,
            moveSpeedZ * BallSpeed * UnityEngine.Time.deltaTime,
            Space.World
            );
        Time += UnityEngine.Time.deltaTime * BallSpeed;

        if (PredictNextFrameHight() < 0.20f)
        {
            // バウンドした状態や位置によって、ネットやアウトなどの判定を下す
            JudgeBound();

            // バウンド処理
            Bound();
        }

        if (transform.position.y < -10.0f)
        {
            if (Parameters.taskType == TaskType.rallying || Parameters.taskType == TaskType.setting)
            {
                if (TaskData.ballBoundCount == 0 && TaskData.controllable) { TaskData.isOut = true; }
                CountBallBound();
            }
            Destroy(gameObject);
        }

        if ((Parameters.taskType == TaskType.rallying || Parameters.taskType == TaskType.setting) && TaskData.ballBoundCount > 3) { Destroy(gameObject); }
        else if (ballBoundCount > 3) { Destroy(gameObject); }
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Court") && isCourtIn)
        {
            string whoseCourt = null;
            if (transform.position.z < 0.0f) { whoseCourt = TaskData.character1.name; }
            else if (transform.position.z > 0.0f) { whoseCourt = TaskData.character2.name; }

            if (TaskData.controllable) { if (TaskData.lastShooter == whoseCourt) { TaskData.isNet = true; } }
        }
        else if (collision.gameObject.CompareTag("Court") && !isCourtIn) { if (TaskData.ballBoundCount == 0 && TaskData.controllable) { TaskData.isOut = true; } }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (TaskData.ballBoundCount == 0 && TaskData.controllable) { TaskData.isOut = true; }
            CountBallBound();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Net"))
        {
            if (isHitByPlayer) { isBoundedPlayerBall = true; }
            if (TaskData.controllable) { TaskData.isNet = true; }
            SpeedZ = -0.10f * moveSpeedZ;
        }
    }

    private void JudgeBound()
    {
        string whoseCourt = null;
        if (transform.position.z < 0.0f) { whoseCourt = TaskData.character1.name; }
        else if (transform.position.z > 0.0f) { whoseCourt = TaskData.character2.name; }

        if (Parameters.taskType == TaskType.rallying || Parameters.taskType == TaskType.setting)
        {
            if (isCourtIn) { if (TaskData.controllable && TaskData.lastShooter == whoseCourt) { TaskData.isNet = true; } }
            else { if (TaskData.ballBoundCount == 0 && TaskData.controllable) { TaskData.isOut = true; } }
        }
        else
        {
            if (isCourtIn && isHitByPlayer && whoseCourt == TaskData.character2.name && !isBoundedPlayerBall)
            {
                isBoundedPlayerBall = true;
                if (Parameters.taskType == TaskType.moving)
                {
                    MovingTaskManager.returnCount++;
                    TaskAudioManager.PlaySuccessSound();
                }
                else if (Parameters.taskType == TaskType.hitting && TargetAreaManager.CheckInTargetArea(transform.position))
                {
                    HittingTaskManager.returnCount++;
                    TaskAudioManager.PlaySuccessSound();
                }
            }
        }
    }

    private void Bound()
    {
        SpeedY = -0.70f * moveSpeedY;
        Time = 0.0f;
        CountBallBound();

        audioSource.PlayOneShot(boundSound);
    }

    private float PredictNextFrameHight()
    {
        float nextHight;
        float currentHight = transform.position.y;
        float nextSpeed = SpeedY - Gravity * (Time + UnityEngine.Time.deltaTime);

        nextHight = currentHight + nextSpeed * BallSpeed * UnityEngine.Time.deltaTime;

        return nextHight;
    }

    private void CountBallBound()
    {
        if (Parameters.taskType == TaskType.rallying || Parameters.taskType == TaskType.setting) { TaskData.ballBoundCount++; }
        else { ballBoundCount++; }
    }

    public void MarkArrivalPoint(Vector3 arrivalPoint)
    {
        arrivalPointMark = Instantiate(arrivalPointPrefab);
        arrivalPointMark.transform.position = arrivalPoint;
        arrivalPointMark.transform.SetParent(courtCanvas.transform);
        arrivalPointMark.transform.localPosition = new Vector3(arrivalPointMark.transform.localPosition.x, arrivalPointMark.transform.localPosition.y, 0.0f);
    }
}
