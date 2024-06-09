using UnityEngine;

public enum ReplayState
{
    Idle,
    Recording,
    Recorded,
    Replay,
    End
}

public class ReplayObjectManager : MonoBehaviour
{
    // 以下のreplayObjectControllersとreplayBallControllerにInterface適用をできる

    [System.NonSerialized]
    public ReplayObjectController[] replayObjectControllers = new ReplayObjectController[2];

    [System.NonSerialized]
    public ReplayBallController replayBallController;

    private readonly float replaySpeed = 0.50f;
    private readonly float replayTime = 5.0f;

    private float worldTime;

    private float localTime;
    private float startTime;
    private float endTime;

    [System.NonSerialized]
    public ReplayState state;

    // Start is called before the first frame update
    void Start()
    {
        worldTime = 0.0f;
        state = ReplayState.Idle;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        worldTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case ReplayState.Idle:
                break;
            case ReplayState.Recording:
                Record();
                break;
            case ReplayState.Recorded:
                break;
            case ReplayState.Replay:
                Replay();
                break;
        }
    }

    public void StartRecord()
    {
        startTime = worldTime;
        Time.timeScale = 1.0f;

        for (int i = 0; i < replayObjectControllers.Length; i++)
        {
            if (replayObjectControllers[i].animationRecorder)
            {
                // アニメーションの記録を開始する
                replayObjectControllers[i].animationRecorder.StartRecord();
            }
        }

        // 状態を遷移
        state = ReplayState.Recording;
        Debug.Log("記録スタート：記録開始時刻は" + worldTime.ToString("N2") + "秒時点から");
    }

    private void Record()
    {
        // リプレイデータの記録中に呼ばれる関数
        for (int i = 0; i < replayObjectControllers.Length; i++)
        {
            // プレイヤの位置と回転を記録する
            if (replayObjectControllers[i]) { replayObjectControllers[i].RecordTransform(worldTime); }
        }

        // ボールの位置と回転を記録する
        replayBallController.RecordTransform(worldTime);
    }

    public void StopRecord()
    {
        endTime = worldTime;

        for (int i = 0; i < replayObjectControllers.Length; i++)
        {
            if (!replayObjectControllers[i].enabled) { replayObjectControllers[i].gameObject.SetActive(true); }
            if (replayObjectControllers[i].animationRecorder) { replayObjectControllers[i].transform.parent = null; }
            if (replayObjectControllers[i].animationRecorder) { replayObjectControllers[i].animationRecorder.StopRecord(); }
        }

        // 状態を遷移
        state = ReplayState.Recorded;
        Debug.Log("記録ストップ：記録時間は" + (endTime - startTime).ToString("N2") + "秒");
    }

    public void StartReplay()
    {
        // リプレイ時の再生速度を変える
        Time.timeScale = replaySpeed;

        if (endTime - replayBallController.BackToExistTime(endTime, startTime) >= replayTime) { localTime = endTime - replayTime; }
        else { localTime = replayBallController.BackToExistTime(endTime, startTime); }

        for (int i = 0; i < replayObjectControllers.Length; i++)
        {
            // アニメーションを記録している場合は、アニメーションのリプレイの再生を開始する
            if (replayObjectControllers[i].animationRecorder) { replayObjectControllers[i].animationRecorder.StartPlayback(endTime - localTime); }
        }

        // 状態を遷移
        state = ReplayState.Replay;
        Debug.Log("リプレイスタート：開始時間は" + localTime.ToString("N2") + "秒時点から");
    }

    // リプレイデータの再生中に呼ばれる関数
    private void Replay()
    {
        for (int i = 0; i < replayObjectControllers.Length; i++)
        {
            // プレイヤオブジェクトのリプレイ再生
            replayObjectControllers[i].Replay(localTime);
        }

        // ボールオブジェクトのリプレイ再生
        replayBallController.Replay(localTime);

        localTime += Time.deltaTime;

        if (endTime - startTime >= replayTime) { if (localTime > Mathf.Min(endTime - startTime, replayTime) + endTime - replayTime) { CancelReplay(); } }
        else { if (localTime > Mathf.Min(endTime - startTime, replayTime) + startTime) { CancelReplay(); } }
    }

    public void CancelReplay()
    {
        for (int i = 0; i < replayObjectControllers.Length; i++)
        {
            if (replayObjectControllers[i].animationRecorder)
            {
                // アニメーションを記録している場合は、アニメーションのリプレイを停止する
                replayObjectControllers[i].animationRecorder.StopPlayback();
            }
        }

        // リプレイ時の再生速度から元に戻す
        Time.timeScale = 1.0f;

        // 状態を遷移
        state = ReplayState.End;
        Debug.Log("リプレイ終了");
    }
}
