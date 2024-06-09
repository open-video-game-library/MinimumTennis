using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReplayManager : MonoBehaviour
{
    // リプレイのシステムを制御するオブジェクト
    [SerializeField]
    private GameObject replayManagerObject;

    // リプレイのシステムを制御するクラス
    private ReplayObjectManager replayObjectManager;

    // アニメーションを記録するためのPrefab
    [SerializeField]
    private GameObject animationRecorderPrefab;

    // プレイヤと対戦相手を格納
    private readonly GameObject[] players = new GameObject[2];
    private readonly GameObject[] playerRecorders = new GameObject[2];

    // ボールを格納
    private GameObject ball;

    // リプレイ再生用のコピー元に使うボールのPrefab
    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private CameraTypeManager cameraTypeManager;

    [SerializeField]
    private ReplayCamera replayCamera;

    [SerializeField]
    private Image fadePanel;

    private Animator animator;

    private bool canCancelReplay;

    // Start is called before the first frame update
    void Start()
    {
        replayObjectManager = replayManagerObject.GetComponent<ReplayObjectManager>();
        animator = GetComponent<Animator>();

        // プレイヤのオブジェクトに、ReplayObjectControllerをアタッチする
        SetPlayersReplay();

        // プレイヤのオブジェクトごとに、AnimationRecorderを生成
        GenerateRecorders();

        // リプレイ再生用のボールを生成
        SetBallReplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.gameState == GameState.Playing && replayObjectManager.state != ReplayState.Recording) { replayObjectManager.StartRecord(); }
        else if (GameData.gameState == GameState.Prepare && replayObjectManager.state == ReplayState.Recording) { replayObjectManager.StopRecord(); }
    }

    public void Manage()
    {
        if (replayObjectManager.state == ReplayState.Recorded)
        {
            // リプレイを再生していない場合、自動で再生を開始する
            StartCoroutine(StartReplay());
        }
        else if (replayObjectManager.state == ReplayState.Replay)
        {
            // リプレイのキャンセル
            if (canCancelReplay 
                && (GameData.replayCancel || (Parameters.inputMethod[0] == InputMethod.none && Parameters.inputMethod[1] == InputMethod.none
                && Input.anyKeyDown))) { replayObjectManager.CancelReplay(); }
        }
        else if (replayObjectManager.state == ReplayState.End)
        {
            // リプレイの再生が終了したら、リプレイモードを終了する
            StartCoroutine(EscapeReplay());
            return;
        }
    }

    private void SetPlayersReplay()
    {
        // プレイヤ情報の読み込み
        players[0] = GameData.character1;
        players[1] = GameData.character2;

        for (int i = 0; i < players.Length; i++)
        {
            // ReplayObjectControllerをアタッチ
            players[i].AddComponent<ReplayObjectController>();

            // ReplayObjectManagerに、アタッチしたReplayObjectControllerを登録
            replayObjectManager.replayObjectControllers[i] = players[i].GetComponent<ReplayObjectController>();
        }
    }

    private void SetBallReplay()
    {
        ball = Instantiate(ballPrefab);
        
        replayObjectManager.replayBallController = replayManagerObject.GetComponent<ReplayBallController>();
        replayObjectManager.replayBallController.replayBall = ball;

        // リプレイ中のカメラの追跡対象を設定
        replayCamera.SetTrackingObject(replayObjectManager.replayBallController.replayBall);

        replayObjectManager.replayBallController.replayBall.SetActive(false);
    }

    private void GenerateRecorders()
    {
        for (int i = 0; i < playerRecorders.Length; i++)
        {
            // Prefabからオブジェクトを生成し、子オブジェクトに設定
            playerRecorders[i] = Instantiate(animationRecorderPrefab);
            playerRecorders[i].transform.parent = replayManagerObject.transform;

            // 生成したAnimationRecorderに、プレイヤが持つAnimatorを登録
            AnimationRecorder animationRecorder = playerRecorders[i].GetComponent<AnimationRecorder>();
            animationRecorder.animator = players[i].GetComponent<Animator>();

            // プレイヤが持つReplayObjectControllerに、生成したAnimationRecorderを登録
            replayObjectManager.replayObjectControllers[i].animationRecorder = playerRecorders[i].GetComponent<AnimationRecorder>();
        }
    }

    private IEnumerator StartReplay()
    {
        canCancelReplay = false;

        // リプレイの状態を一時的にIdleに設定
        replayObjectManager.state = ReplayState.Idle;

        // 暗転する
        fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        // 演出のため、0.25秒だけ暗転したまま待機
        yield return new WaitForSeconds(0.25f);

        // カメラを通常用からリプレイ用に切り替える
        animator.SetTrigger("ChangeMainToReplay");
        if (Parameters.playMode == PlayMode.competition) { cameraTypeManager.SwitchCameraType(PlayMode.normal); }

        // リプレイの再生を開始
        replayObjectManager.StartReplay();

        // 1フレーム待機
        yield return null;

        // 明転の演出
        for (int i = 0; i < Application.targetFrameRate / 2; i++)
        {
            fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - i * (float)(2.0f / Application.targetFrameRate));

            // 1フレーム待機
            yield return null;
        }

        // 明転状態で固定
        fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        canCancelReplay = true;
    }

    private IEnumerator EscapeReplay()
    {
        // リプレイの状態を一時的にIdleに設定
        replayObjectManager.state = ReplayState.Idle;

        // リプレイ再生用のボールオブジェクトを非アクティブ状態にする
        replayObjectManager.replayBallController.replayBall.SetActive(false);

        // 暗転する
        fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        // カメラをリプレイ用から通常用に切り替える
        animator.SetTrigger("ChangeReplayToMain");
        if (Parameters.playMode == PlayMode.competition) { cameraTypeManager.SwitchCameraType(Parameters.playMode); }

        // 1フレーム待機
        yield return null;

        // プレイヤの再配置時の移動にカメラがぴったり付いていくようにする
        cameraTypeManager.TeleportCamera(Parameters.playMode);

        if (GameData.character1GameCount != (int)Parameters.gameSize && GameData.character2GameCount != (int)Parameters.gameSize)
        {
            GameData.character1.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(8.0f * (int)GameData.servePosition, 0.0f, -49.0f));
            GameData.character2.GetComponent<ICharacterMover>().ResetCharacterPosition(new Vector3(-8.0f * (int)GameData.servePosition, 0.0f, 49.0f));
        }

        // 演出のため、0.25秒だけ暗転したまま待機
        yield return new WaitForSeconds(0.25f);

        // ゲームの進行に応じて、次に遷移するゲームの状態を切り替える
        if (GameData.character1GameCount == (int)Parameters.gameSize || GameData.character2GameCount == (int)Parameters.gameSize) { GameData.gameState = GameState.End; }
        else { GameData.gameState = GameState.Playing; }

        // 明転の演出
        for (int i = 0; i < Application.targetFrameRate / 2; i++)
        {
            fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - i * (float)(2.0f / Application.targetFrameRate));

            // 1フレーム待機
            yield return null;
        }

        // 明転状態で固定
        fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        GameData.replayCancel = false;
    }
}
