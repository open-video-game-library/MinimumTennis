using UnityEngine;

public class PlayerTaskController : MonoBehaviour
{
    // 異なる入力デバイスから共通の入力信号を受け取るための受け口
    private IInputDevice inputDevice;

    // 自動操作時の移動・ショットのアルゴリズムを管理するAI
    private PlayerTaskAI ai;

    // 自身が1Pか2Pかを格納
    [System.NonSerialized]
    public Players player;

    // 移動入力用パラメータ
    private float x;
    private float z;

    // ショット用パラメータ
    private bool normalShot;
    private bool lobShot;
    private bool fastShot;
    private bool dropShot;
    private bool aiShot;

    // 自動操作時専用の変数
    private Vector3 aiShotPower;

    // ポーズ画面起動用
    private bool escape;

    // 移動制御用パラメータ
    private Move move;

    // ショット制御用パラメータ
    private Shot shot;
    private bool isHit;
    private string lastShooter;

    // Start is called before the first frame update
    void Start()
    {
        inputDevice = GetComponent<IInputDevice>();

        ai = GetComponent<PlayerTaskAI>();

        move = GetComponent<Move>();
        move.movableArea = TaskData.movableArea[(int)player];

        shot = GetComponent<Shot>();
        if (player == Players.p1) { shot.opponentCourtArea = TaskData.playersCourtArea[(int)Players.p2]; }
        else if (player == Players.p2) { shot.opponentCourtArea = TaskData.playersCourtArea[(int)Players.p1]; }
    }

    // Update is called once per frame
    void Update()
    {
        // Shotクラスが持つテイクバック用の変数に、AIの処理結果を書き込む
        shot.Takeback(ai.takebackFore, ai.takebackBack, ai.autoMoveLateralDirection);

        // プレイヤがコントロール可能状態にあるとき
        if (TaskData.controllable)
        {
            // ポーズ入力があったら、ポーズ画面を開くor閉じる
            if (escape) { TaskData.pause = !TaskData.pause; }

            // 移動する処理
            Move();
        }
        else
        {
            // 移動を止める
            move.StopPlayer();

            // ボールを打っていない状態にする
            isHit = false;
        }
    }

    void LateUpdate()
    {
        // 誰かが失点した場合、もしくは、プレイヤがコントロール可能状態にないとき
        if (TaskData.foul != FoulState.NoFoul || !TaskData.controllable)
        {
            lastShooter = null;
            return;
        }

        // TaskDataが持つlastShooterと、自身が持つlastShooterが異なるとき
        if (TaskData.lastShooter != name && lastShooter == name)
        {
            // TaskData側のlastShooterに自身が持つlastShooterを上書きして更新する
            TaskData.lastShooter = lastShooter;
            lastShooter = null;
        }
        else { lastShooter = null; }
    }

    void FixedUpdate()
    {
        // 自動で移動操作をする場合
        if (ai.autoMove)
        {
            x = ai.x;
            z = ai.z;
        }
        // 自力で移動操作をする場合
        else
        {
            x = inputDevice.GetMoveInput(player).x;
            z = inputDevice.GetMoveInput(player).y;
        }

        // 自動でショット操作をする場合
        if (ai.autoShot)
        {
            aiShot = ai.shot;
            aiShotPower = ai.shotPower;
        }
        // 自力でショット操作をする場合
        else
        {
            normalShot = inputDevice.GetNormalShotInput(player);
            lobShot = inputDevice.GetLobShotInput(player);
            fastShot = inputDevice.GetFastShotInput(player);
            dropShot = inputDevice.GetDropShotInput(player);
        }

        if (inputDevice != null) { escape = inputDevice.GetEscapeInput(player); }
        else if (player == Players.p1) { escape = Input.GetKeyDown(KeyCode.Escape); }
    }

    void OnTriggerStay(Collider other)
    {
        // 衝突したObjectがBallでない場合、もしくは、すでにボールを打っていた場合
        if (!TaskData.controllable && !other.gameObject.CompareTag("Ball") || isHit) { return; }

        GameObject ballObject = other.gameObject;

        // ショットを打つ処理
        Shot(ballObject);
    }

    void OnTriggerExit(Collider other)
    {
        // Colliderから出ていったObjectがBallの場合、ボールを打っていない状態にする
        if (other.gameObject.CompareTag("Ball")) { isHit = false; }
    }

    private void Move()
    {
        move.MovePlayer(x, z);
    }

    private void Shot(GameObject ballObject)
    {
        if (TaskData.lastShooter != name)
        {
            float ballHight = ballObject.transform.position.y;

            // 自動でショットを行う場合
            if (aiShot) { shot.AIShot(ballObject, aiShotPower, Parameters.charactersDominantHand[(int)player]); }
            // プレイヤが前衛ポジションにいるとき
            else if (TaskData.courtArea.zNegativeLimit / 2.0f < transform.position.z && transform.position.z < TaskData.courtArea.zPositiveLimit / 2.0f)
            {
                if (ballHight > 10.0f)
                {
                    // スマッシュを打つ
                    if (normalShot || lobShot || fastShot || dropShot) { shot.Smash(ballObject); }
                }
                else if (ballHight > 2.50f)
                {
                    // ボレーを打つ
                    if (normalShot || lobShot || fastShot || dropShot) { shot.Volley(ballObject, Parameters.charactersDominantHand[(int)player]); }
                }
            }
            else
            {
                // 入力に応じて異なるショットを打つ
                if (lobShot) { shot.LobShot(ballObject, Parameters.charactersDominantHand[(int)player]); }
                else if (fastShot) { shot.FastShot(ballObject, Parameters.charactersDominantHand[(int)player]); }
                else if (dropShot) { shot.DropShot(ballObject, Parameters.charactersDominantHand[(int)player]); }
                else if (normalShot) { shot.NormalShot(ballObject, Parameters.charactersDominantHand[(int)player]); }
            }

            // 全ショット共通の処理
            if (normalShot || lobShot || fastShot || dropShot || aiShot)
            {
                isHit = true;

                TaskData.ballBoundCount = 0;
                TaskData.rallyCount++;

                if (Parameters.taskType == TaskType.moving || Parameters.taskType == TaskType.hitting)
                {
                    // TaskBallControllerに1Pプレイヤによってボールが打たれたことを書き込む
                    ballObject.GetComponent<TaskBallController>().isHitByPlayer = true;
                }

                if (TaskData.foul == FoulState.NoFoul) { lastShooter = name; }
            }
        }
    }
}
