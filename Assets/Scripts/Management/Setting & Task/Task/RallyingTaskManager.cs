using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class RallyingTaskManager : MonoBehaviour, ITaskManager
{
    public static readonly int timeLimit = 90;
    public static int rallyCount;
    public static int maxRallyCount;

    private Shot ballServePlayer;

    private readonly float ballServeSpan = 3.0f;
    private float ballServeCount;
    private bool isBallServed;

    private bool miss;
    private bool prepareOnce;

    [SerializeField]
    private Image resetPanel;
    private float panelAlpha;

    private float remainingTime;

    private Animator animator;

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text highScoreText;
    [SerializeField]
    private GameObject timeTextObject;
    private TMP_Text timeText;

    [SerializeField]
    private CameraResetManager cameraResetManager;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = SystemParameters.fps;

        InitializeTask();
        rallyCount = 0;
        maxRallyCount = 0;

        // ボールをサーブするプレイヤをcharacter2に設定
        ballServePlayer = TaskData.character2.GetComponent<Shot>();

        animator = GetComponentInParent<Animator>();

        timeText = timeTextObject.GetComponent<TMP_Text>();

        scoreText.text = rallyCount.ToString();
        highScoreText.text = "High Score: " + maxRallyCount.ToString();

        timeTextObject.SetActive(true);
        remainingTime = timeLimit;
        timeText.text = FormatTime(remainingTime);

        resetPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        panelAlpha = resetPanel.color.a;
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

        Parameters.taskType = TaskType.rallying;
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
        if (prepareOnce) { return; }
        prepareOnce = true;
        StartCoroutine(SwitchPlayingAfterDelay());
    }

    private void ManagePlayingState()
    {
        // 制御不能状態もしくはポーズ画面が開かれているとき
        if (!TaskData.controllable || TaskData.pause)
        {
            ballServeCount = 0.0f;
            return;
        }

        // スコア情報を更新
        rallyCount = TaskData.rallyCount;
        scoreText.text = rallyCount.ToString();
        if (rallyCount > maxRallyCount)
        {
            // 最高スコアを更新したとき
            maxRallyCount = rallyCount;
            highScoreText.text = "High Score: " + maxRallyCount.ToString();
        }

        // 制限時間を過ぎたとき
        if (remainingTime <= 0.0f)
        {
            remainingTime = 0.0f;
            timeText.text = FormatTime(remainingTime);

            // TaskResultManagerクラスへスコアを渡す
            TaskResultManager.taskScore = maxRallyCount;

            SwitchTaskState(TaskState.End);
        }
        else if (isBallServed && !miss) 
        {
            // 残り時間を減らす
            remainingTime -= Time.deltaTime;
            timeText.text = FormatTime(remainingTime);
        }

        // 反則が無いか監視する
        if (JudgeFoul())
        {
            if (remainingTime > 0.0f)
            {
                if (!miss)
                {
                    miss = true;
                    StartCoroutine(SwitchPrepareAfterDelay());
                }
            }
            else { SwitchTaskState(TaskState.End); }
        }

        // 対戦相手が自動でボールをサーブするまでカウント
        if (!isBallServed)
        {
            ballServeCount += Time.deltaTime;
            if (ballServeCount >= ballServeSpan)
            {
                ballServePlayer.ServeBall();
                ballServeCount = 0.0f;
                isBallServed = true;

                if (TaskData.foul == FoulState.NoFoul) { TaskData.lastShooter = ballServePlayer.gameObject.name; }
            }
        }
    }

    private void ManageEndState()
    {
        animator.SetTrigger("Finish");
    }

    IEnumerator SwitchPrepareAfterDelay()
    {
        // Foulを検出してからPrepare状態に移行するまでの処理

        yield return new WaitForSeconds(1f); // 1秒待機
        SwitchTaskState(TaskState.Prepare);
    }

    IEnumerator SwitchPlayingAfterDelay()
    {
        // Prepare状態になってからPlaying状態に移行するまでの処理

        while (panelAlpha < 1.0f)
        {
            panelAlpha += 0.050f;
            resetPanel.color = new Color(0.0f, 0.0f, 0.0f, panelAlpha);
            yield return new WaitForSeconds(0.010f); // 速度調整
        }

        cameraResetManager.Teleport(new Vector3(0.0f, 30.0f, -80.0f));

        TaskData.character1.transform.position = TaskData.character1DefalutPosition;
        TaskData.character2.transform.position = TaskData.character2DefalutPosition;

        TaskData.foul = FoulState.NoFoul;
        TaskData.lastShooter = null;
        TaskData.isOut = false;
        TaskData.isNet = false;
        TaskData.ballBoundCount = 0;
        TaskData.ballAmount = 0;

        TaskData.rallyCount = 0;
        rallyCount = TaskData.rallyCount;
        scoreText.text = rallyCount.ToString();

        while (0.0f < panelAlpha)
        {
            panelAlpha -= 0.050f;
            resetPanel.color = new Color(0.0f, 0.0f, 0.0f, panelAlpha);
            yield return new WaitForSeconds(0.010f); // 速度調整
        }

        resetPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        panelAlpha = resetPanel.color.a;

        SwitchTaskState(TaskState.Playing);
        isBallServed = false;

        miss = false;
        prepareOnce = false;
    }

    private string FormatTime(float totalSeconds)
    {
        // floatからintへ変換
        int intSeconds = (int)totalSeconds;
        // 分を取得
        int minutes = intSeconds / 60;
        // 秒を取得
        int remainingSeconds = intSeconds % 60;
        // ミリ秒を取得
        int milliseconds = (int)((totalSeconds - intSeconds) * 100);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, remainingSeconds, milliseconds);
    }

    private bool JudgeFoul()
    {
        if (TaskData.isNet) { TaskData.foul = FoulState.Net; }
        else if (TaskData.isOut) { TaskData.foul = FoulState.Out; }
        else if (TaskData.ballBoundCount >= 2) { TaskData.foul = FoulState.TwoBounds; }

        return TaskData.foul != FoulState.NoFoul;
    }

    public Vector3 DecideArrivalPosition(TaskBallController ball)
    {
        Vector3 defaltTargetPoint = new Vector3(0.0f, 0.0f, -20.0f);

        return defaltTargetPoint;
    }

    public float DecideLateralSpeed(GameObject ballOjbect, TaskBallController ball, float ballSpeedY, Vector3 targetPosition)
    {
        Vector3 ballPosition = ballOjbect.transform.position;
        float gravity = ball.Gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disX = targetPosition.x - ballOjbect.transform.position.x;
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
