using UnityEngine;

public class PlayerNormalController : MonoBehaviour
{
    // 異なる入力デバイスから共通の入力信号を受け取るための受け口
    private IInputDevice inputDevice;

    // 自動操作時の移動・ショットのアルゴリズムを管理するAI
    private PlayerNormalAI ai;

    // 自身が1Pか2Pかを格納
    [System.NonSerialized]
    public Players player;

    // 移動入力用パラメータ
    private float x;
    private float z;

    // ショット入力用パラメータ
    private bool normalShot;
    private bool lobShot;
    private bool fastShot;
    private bool dropShot;
    private bool aiShot;

    // 自動操作時専用の変数
    private Vector3 aiShotPower;

    // サーブ入力用パラメータ
    private bool toss;
    private bool serve;

    // ポーズ画面起動用
    private bool escape;

    // 移動制御用パラメータ
    private Move move;

    // ショット制御用パラメータ
    private Shot shot;
    private bool isHit;
    private string lastShooter;

    private bool previousToss;
    private readonly float coolTime = 0.10f;
    private float coolTimeCount = 0.0f;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        inputDevice = GetComponent<IInputDevice>();

        ai = GetComponent<PlayerNormalAI>();

        move = GetComponent<Move>();
        move.movableArea = GameData.movableArea[(int)player];

        shot = GetComponent<Shot>();
        if (player == Players.p1) { shot.opponentCourtArea = GameData.playersCourtArea[(int)Players.p2]; }
        else if (player == Players.p2) { shot.opponentCourtArea = GameData.playersCourtArea[(int)Players.p1]; }
        
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 自身がサーバーではなく、サーブが入っていない状態かつ、まだ誰もボールを打っていない状態の場合
        // レシーバー専用の待機モーションに切り替える
        animator.SetBool("Receiver", GameData.server != name && !GameData.isServeIn && GameData.lastShooter == null);

        // Shotクラスが持つテイクバック用の変数に、AIの処理結果を書き込む
        shot.Takeback(ai.takebackFore, ai.takebackBack, ai.autoMoveLateralDirection);

        // プレイヤがコントロール可能状態にあるとき
        if (GameData.controllable)
        {
            // ポーズ入力があったら、ポーズ画面を開くor閉じる
            if (escape) { GameData.pause = !GameData.pause; }

            // 自身がサーバーでない場合
            if (GameData.server != name)
            {
                // animatorの移動アニメーションの重みを1.0fに固定
                animator.SetLayerWeight(1, 1.0f);
            }

            // 移動する処理
            Move();

            // トスを上げる処理
            Toss();
        }
        else
        {
            // 移動を止める
            move.StopPlayer();

            // ボールを打っていない状態にする
            isHit = false;

            // トスを上げてから、サーブを打てるまでの時間をリセットする
            coolTimeCount = coolTime;
        }

        // リプレイを再生している間
        if (GameData.gameState == GameState.Replay && inputDevice != null)
        {
            // 何かしらボタンを入力することで、リプレイの再生をキャンセルするための信号を送る
            GameData.replayCancel = GameData.replayCancel 
                || normalShot || lobShot || fastShot || dropShot 
                || toss || serve || escape;
        }
    }

    void LateUpdate()
    {
        // 誰かが失点した場合、もしくは、プレイヤがコントロール可能状態にないとき
        if (GameData.foul != FoulState.NoFoul || !GameData.controllable)
        {
            lastShooter = null;
            return;
        }

        // GameDataが持つlastShooterと、自身が持つlastShooterが異なるとき
        if (GameData.lastShooter != name && lastShooter == name)
        {
            // GameData側のlastShooterに自身が持つlastShooterを上書きして更新する
            GameData.lastShooter = lastShooter;
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

            toss = ai.toss;
            serve = ai.serve;
        }
        // 自力でショット操作をする場合
        else
        {
            normalShot = inputDevice.GetNormalShotInput(player);
            lobShot = inputDevice.GetLobShotInput(player);
            fastShot = inputDevice.GetFastShotInput(player);
            dropShot = inputDevice.GetDropShotInput(player);

            toss = inputDevice.GetTossInput(player);
            serve = inputDevice.GetServeInput(player);
        }

        if (inputDevice != null) { escape = inputDevice.GetEscapeInput(player); }
        else if (player == Players.p1) { escape = Input.GetKeyDown(KeyCode.Escape); }
    }

    void OnTriggerStay(Collider other)
    {
        // 衝突したObjectがBallでない場合、もしくは、すでにボールを打っていた場合
        if (!GameData.controllable && !other.gameObject.CompareTag("Ball") || isHit) { return; }

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
        // 自身がサーバーかつトス状態以外でボールが存在する場合、もしくは、自身がサーバーではない場合は，プレイヤを移動させる
        if ((!GameData.isToss && GameData.ballAmount != 0) || GameData.server != name) { move.MovePlayer(x, z); }
    }

    private void Toss()
    {
        // コントロール可能になったら、トスを上げてからサーブを打てるまでの時間を加算していく
        if (coolTimeCount < coolTime) { coolTimeCount += Time.deltaTime; }

        // 自身がサーバーで、ボールが存在せず、サーブが入っていない状態の場合
        if (GameData.server == name && GameData.ballAmount == 0 && !GameData.isServeIn)
        {
            // トスを上げてからサーブが打てるようになるまでのクールタイムが終わっており、トスの入力があった場合
            if (toss)
            {
                GameData.isToss = true;
                coolTimeCount = 0.0f;

                shot.Toss();
            }

            if (!previousToss && GameData.isToss) { animator.SetLayerWeight(1, 0.0f); }
            else if (previousToss && !GameData.isToss && GameData.ballAmount == 0)
            {
                animator.SetTrigger("Idle");
                animator.SetLayerWeight(1, 1.0f);
            }
        }
        previousToss = GameData.isToss;
    }

    private void Shot(GameObject ballObject)
    {
        if (!GameData.isToss && GameData.isServeIn && GameData.lastShooter != name)
        {
            float ballHight = ballObject.transform.position.y;

            // 自動でショットを行う場合
            if (aiShot) { shot.AIShot(ballObject, aiShotPower, Parameters.charactersDominantHand[(int)player]); }
            // プレイヤが前衛ポジションにいるとき
            else if (GameData.courtArea.zNegativeLimit / 2.0f < transform.position.z && transform.position.z < GameData.courtArea.zPositiveLimit / 2.0f)
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

                GameData.ballBoundCount = 0;
                GameData.rallyCount++;

                if (GameData.foul == FoulState.NoFoul) { lastShooter = name; }
            }
        }
        else if (GameData.isToss)
        {
            // トスを上げてからのクールタイム経過後、サーブを打つ
            if (coolTimeCount >= coolTime && serve)
            {
                shot.Serve(ballObject, GameData.servePosition, x);

                isHit = true;

                GameData.isToss = false;

                if (GameData.foul == FoulState.NoFoul) { lastShooter = name; }
            }
        }
    }
}
