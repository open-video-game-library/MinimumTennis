using UnityEngine;
using TMPro;

public class HittingTaskManager : MonoBehaviour, ITaskManager
{
    public static readonly int taskCount = 30;
    public static int shotCount;
    public static int returnCount;

    private Shot ballServePlayer;
    private PlayerTaskAI ballServePlayerAI;

    private readonly float ballServeSpan = 3.0f;
    private float ballServeCount;

    private Animator animator;

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text remainingText;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = SystemParameters.fps;

        InitializeTask();
        shotCount = 0;
        returnCount = 0;

        // ボールをサーブするプレイヤをcharacter2に設定
        ballServePlayer = TaskData.character2.GetComponent<Shot>();
        ballServePlayerAI = TaskData.character2.GetComponent<PlayerTaskAI>();
        // ボールをサーブするプレイヤは、自由に移動・返球ができない
        ballServePlayerAI.action = false;

        animator = GetComponentInParent<Animator>();

        scoreText.text = returnCount.ToString();
        remainingText.text = "Remaining: " + taskCount.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (TaskData.taskState)
        {
            case TaskState.Start:
                ManageStartState();
                break;
            case TaskState.Prepare:
                ManagePrepareState();
                break;
            case TaskState.Playing:
                ManagePlayingState();
                break;
            case TaskState.End:
                ManageEndState();
                break;
        }
    }

    public void InitializeTask()
    {
        // Set this object's interface as task manager
        TaskData.taskManagerInterface = gameObject.GetComponent<ITaskManager>();

        // Set default parameter to "TaskData"
        TaskData.character1 = CharacterGenerator.GetCharacter(0);
        TaskData.character2 = CharacterGenerator.GetCharacter(1);

        // Set character to initial position
        TaskData.character1.transform.position = TaskData.character1DefalutPosition;
        TaskData.character2.transform.position = TaskData.character2DefalutPosition;

        Parameters.taskType = TaskType.hitting;
        TaskData.taskState = TaskState.Start;

        TaskData.foul = FoulState.NoFoul;
        TaskData.lastShooter = null;
        TaskData.isOut = false;
        TaskData.isNet = false;
        TaskData.rallyCount = 0;
        TaskData.ballBoundCount = 0;
        TaskData.ballAmount = 0;

        TaskData.controllable = false;
        TaskData.pause = false;
    }

    public void SwitchTaskState(TaskState nextState)
    {
        TaskData.taskState = nextState;
    }

    private void ManageStartState()
    {

    }

    private void ManagePrepareState()
    {

    }

    private void ManagePlayingState()
    {
        // 制御不能状態もしくはポーズ画面が開かれているとき
        if (!TaskData.controllable || TaskData.pause)
        {
            ballServeCount = 0.0f;
            return;
        }
        // タスクの試行回数が既定回数まで達したとき
        if (taskCount - shotCount <= 0)
        {
            if (CountBall() != 0) { return; }

            // TaskResultManagerクラスへスコアを渡す
            TaskResultManager.taskScore = returnCount;
            SwitchTaskState(TaskState.End);
        }

        ballServeCount += Time.deltaTime;
        if (ballServeCount >= ballServeSpan)
        {
            shotCount++;
            ballServePlayer.ServeBall();
            ballServeCount = 0.0f;

            if (TaskData.foul == FoulState.NoFoul) { TaskData.lastShooter = ballServePlayer.gameObject.name; }
        }

        scoreText.text = returnCount.ToString();
        remainingText.text = "Remaining: " + (taskCount - shotCount).ToString();
    }

    private void ManageEndState()
    {
        animator.SetTrigger("Finish");
    }

    private int CountBall()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        int ballCount = balls.Length;

        return ballCount;
    }

    public Vector3 DecideArrivalPosition(TaskBallController ball)
    {
        Vector3 defaltTargetPoint;
        if (shotCount % 2 == 1) { defaltTargetPoint = new Vector3(2.0f, 0.0f, -20.0f); }
        else { defaltTargetPoint = new Vector3(-2.0f, 0.0f, -20.0f); }

        ball.MarkArrivalPoint(defaltTargetPoint);

        return defaltTargetPoint;
    }

    public float DecideLateralSpeed(GameObject ballObject, TaskBallController ball, float ballSpeedY, Vector3 targetPosition)
    {
        Vector3 ballPosition = ballObject.transform.position;
        float gravity = ball.Gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disX = targetPosition.x - ballObject.transform.position.x;
        return disX / arrivalTime;
    }

    public float DecideDepthSpeed(GameObject ballObject, TaskBallController ball, float ballSpeedY, Vector3 targetPosition)
    {
        Vector3 ballPosition = ballObject.transform.position;
        float gravity = ball.Gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disZ = ballObject.transform.position.z - targetPosition.z;
        return disZ / arrivalTime;
    }
}
