using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ParameterSettingManager : MonoBehaviour, ITaskManager
{
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

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text highScoreText;

    [SerializeField]
    private CameraResetManager cameraResetManager;

    [SerializeField]
    private GameObject[] parameterSliders;
    private IResetParameter[] changeParameters;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = SystemParameters.fps;

        InitializeTask();
        rallyCount = 0;
        maxRallyCount = 0;

        // ボールをサーブするプレイヤをcharacter2に設定
        ballServePlayer = TaskData.character2.GetComponent<Shot>();

        scoreText.text = rallyCount.ToString();
        highScoreText.text = "High Score: " + maxRallyCount.ToString();

        resetPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        panelAlpha = resetPanel.color.a;

        changeParameters = new IResetParameter[parameterSliders.Length];
        for (int i = 0; i < parameterSliders.Length; i++) { changeParameters[i] = parameterSliders[i].GetComponent<IResetParameter>(); }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TaskData.controllable = TaskData.taskState == TaskState.Playing;

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

        Parameters.taskType = TaskType.setting;
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
        SwitchTaskState(TaskState.Playing);
    }

    private void ManagePrepareState()
    {
        if (prepareOnce) { return; }
        prepareOnce = true;
        StartCoroutine(SwitchPlayingAfterDelay());
    }

    private void ManagePlayingState()
    {
        // 制御不能状態もしくはパラメータ編集画面が開かれているとき
        if (!TaskData.controllable || TaskData.pause)
        {
            ballServeCount = 0.0f;

            // 反則が無いか監視する
            if (JudgeFoul() && !miss)
            {
                miss = true;
                StartCoroutine(SwitchPrepareAfterDelay());
            }

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

        // 反則が無いか監視する
        if (JudgeFoul() && !miss)
        {
            miss = true;
            StartCoroutine(SwitchPrepareAfterDelay());
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

    public void ResetParameters()
    {
        // Interface経由でパラメータを初期化
        for (int i = 0; i < parameterSliders.Length; i++) { changeParameters[i].ResetParameter(); }
    }

    public void BackHome()
    {
        InitializeTask();
        SceneManager.LoadScene("HomeScene");
    }

    private bool JudgeFoul()
    {
        if (TaskData.isNet) { TaskData.foul = FoulState.Net; }
        else if (TaskData.isOut) { TaskData.foul = FoulState.Out; }
        else if (TaskData.ballBoundCount >= 2) { TaskData.foul = FoulState.TwoBounds; }

        return TaskData.foul != FoulState.NoFoul;
    }

    public Vector3 DecideArrivalPosition(TaskBallController ballController)
    {
        Vector3 defaltTargetPoint = new Vector3(0.0f, 0.0f, -20.0f);

        return defaltTargetPoint;
    }

    public float DecideLateralSpeed(GameObject ball, TaskBallController ballController, float ballSpeedY, Vector3 targetPosition)
    {
        Vector3 ballPosition = ball.transform.position;
        float gravity = ballController.Gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disX = targetPosition.x - ball.transform.position.x;
        return disX / arrivalTime;
    }

    public float DecideDepthSpeed(GameObject ball, TaskBallController ballController, float ballSpeedY, Vector3 targetPosition)
    {
        Vector3 ballPosition = ball.transform.position;
        float gravity = ballController.Gravity;
        float arrivalTime = (ballSpeedY + Mathf.Sqrt(ballSpeedY * ballSpeedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float disZ = ball.transform.position.z - targetPosition.z;
        return disZ / arrivalTime;
    }
}
