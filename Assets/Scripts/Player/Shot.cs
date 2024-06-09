using System.Collections;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [System.NonSerialized]
    public Players player;

    // Editable Parameter
    private float ballSpeed = 1.0f;

    [SerializeField]
    private GameObject normalBallPrefab;
    [SerializeField]
    private GameObject taskBallPrefab;

    [System.NonSerialized]
    public Area opponentCourtArea;

    // 自動移動時に、打つ方向を決める変数
    private float autoMoveLateralDirection;

    private AudioSource audioSource;
    public AudioClip[] hitSounds = new AudioClip[3];
    public AudioClip serveSound;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        ballSpeed = Parameters.ballSpeed[(int)player];

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ballSpeed = Parameters.ballSpeed[(int)player];
    }

    public void Toss()
    {
        float powerY = 28.0f;

        animator.SetTrigger("Toss");

        GameObject ballObject = Instantiate(normalBallPrefab);

        float posX = transform.position.x;
        float posZ = transform.position.z;

        // プレイヤの位置に対して、「正面」の「高さ6.0f」の「前に5.0f」の位置にボールを生成する
        ballObject.transform.position = new Vector3(posX, 6.0f, posZ + 5.0f * transform.forward.z);

        IBallController ball = ballObject.GetComponent<IBallController>();

        ball.SpeedY = powerY;

        ball.BallSpeed = Parameters.ballSpeed[(int)player];
    }

    public void Serve(GameObject ballObject, ServePosition servePosition, float x)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        float powerX = 25.0f;
        float powerY = -6.0f;
        float powerZ = 100.0f;

        animator.SetTrigger("Serve");
        audioSource.PlayOneShot(serveSound);

        ball.Time = 0.0f;
        ball.SpeedX = powerX * -(int)servePosition * transform.forward.z + 6.0f * x;
        ball.SpeedY = powerY;
        ball.SpeedZ = powerZ * transform.forward.z;

        ball.BallSpeed = ballSpeed;
    }

    public void NormalShot(GameObject ballObject, DominantHand dominantHand)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        float powerX = 2.0f;
        float powerY = 25.0f;
        float powerZ = 65.0f;

        // ボールの左右のスピードを決めるための変数
        float lateralDirection;

        // 自動移動時に打つ方向を決める変数が0.0f（値が入っていない）場合
        if (autoMoveLateralDirection == 0.0f)
        {
            // プレイヤとボールの相対的な位置関係に応じて、ボールの横方向の向きを決める
            Vector3 objectPositionInPlayerLocal = transform.InverseTransformPoint(ballObject.transform.position);
            lateralDirection = objectPositionInPlayerLocal.x > 0 ? -1.0f : 1.0f;
        }
        else { lateralDirection = autoMoveLateralDirection; }

        if (lateralDirection == (int)dominantHand) { animator.SetTrigger("Forehand"); }
        else if (lateralDirection == -(int)dominantHand) { animator.SetTrigger("Backhand"); }

        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballObject.transform.position.z - transform.position.z);

        ball.Time = 0.0f;
        ball.SpeedX = powerX * lateralSpeedMagnification;
        ball.SpeedY = powerY;
        ball.SpeedZ = powerZ * transform.forward.z;

        AdjustBallSpeed(ballObject);

        ball.BallSpeed = ballSpeed;
    }

    public void LobShot(GameObject ballObject, DominantHand dominantHand)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        float powerX = 1.25f;
        float powerY = 45.0f;
        float powerZ = 45.0f;

        // ボールの左右のスピードを決めるための変数
        float lateralDirection;

        // 自動移動時に打つ方向を決める変数が0.0f（値が入っていない）場合
        if (autoMoveLateralDirection == 0.0f)
        {
            // プレイヤとボールの相対的な位置関係に応じて、ボールの横方向の向きを決める
            Vector3 objectPositionInPlayerLocal = transform.InverseTransformPoint(ballObject.transform.position);
            lateralDirection = objectPositionInPlayerLocal.x > 0 ? -1.0f : 1.0f;
        }
        else { lateralDirection = autoMoveLateralDirection; }

        if (lateralDirection == (int)dominantHand) { animator.SetTrigger("Forehand"); }
        else if (lateralDirection == -(int)dominantHand) { animator.SetTrigger("Backhand"); }

        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballObject.transform.position.z - transform.position.z);

        ball.Time = 0.0f;
        ball.SpeedX = powerX * lateralSpeedMagnification;
        ball.SpeedY = powerY;
        ball.SpeedZ = powerZ * transform.forward.z;

        AdjustBallSpeed(ballObject);

        ball.BallSpeed = ballSpeed;
    }

    public void FastShot(GameObject ballObject, DominantHand dominantHand)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        float powerX = 2.5f;
        float powerY = 14.0f;
        float powerZ = 100.0f;

        // ボールの左右のスピードを決めるための変数
        float lateralDirection;

        // 自動移動時に打つ方向を決める変数が0.0f（値が入っていない）場合
        if (autoMoveLateralDirection == 0.0f)
        {
            // プレイヤとボールの相対的な位置関係に応じて、ボールの横方向の向きを決める
            Vector3 objectPositionInPlayerLocal = transform.InverseTransformPoint(ballObject.transform.position);
            lateralDirection = objectPositionInPlayerLocal.x > 0 ? -1.0f : 1.0f;
        }
        else { lateralDirection = autoMoveLateralDirection; }

        if (lateralDirection == (int)dominantHand) { animator.SetTrigger("Forehand"); }
        else if (lateralDirection == -(int)dominantHand) { animator.SetTrigger("Backhand"); }

        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballObject.transform.position.z - transform.position.z);

        ball.Time = 0.0f;
        ball.SpeedX = powerX * lateralSpeedMagnification;
        ball.SpeedY = powerY;
        ball.SpeedZ = powerZ * transform.forward.z;

        AdjustUpwardSpeed(ballObject);
        AdjustBallSpeed(ballObject);

        ball.BallSpeed = ballSpeed;
    }

    public void DropShot(GameObject ballObject, DominantHand dominantHand)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        float powerX = 2.0f;
        float powerY = 25.0f;
        float powerZ = 48.0f;

        // ボールの左右のスピードを決めるための変数
        float lateralDirection;

        // 自動移動時に打つ方向を決める変数が0.0f（値が入っていない）場合
        if (autoMoveLateralDirection == 0.0f)
        {
            // プレイヤとボールの相対的な位置関係に応じて、ボールの横方向の向きを決める
            Vector3 objectPositionInPlayerLocal = transform.InverseTransformPoint(ballObject.transform.position);
            lateralDirection = objectPositionInPlayerLocal.x > 0 ? -1.0f : 1.0f;
        }
        else { lateralDirection = autoMoveLateralDirection; }

        if (lateralDirection == (int)dominantHand) { animator.SetTrigger("Forehand"); }
        else if (lateralDirection == -(int)dominantHand) { animator.SetTrigger("Backhand"); }

        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballObject.transform.position.z - transform.position.z);

        ball.Time = 0.0f;
        ball.SpeedX = powerX * lateralSpeedMagnification;
        ball.SpeedY = powerY;
        ball.SpeedZ = powerZ * transform.forward.z;

        AdjustBallSpeed(ballObject);

        ball.BallSpeed = ballSpeed;
    }

    public void Smash(GameObject ballObject)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        float powerX = 4.0f;
        float powerY = -20.0f;
        float powerZ = 120.0f;

        animator.SetTrigger("Smash");

        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        // ボールの左右のスピードを決めるための変数
        float lateralDirection;

        // 自動移動時に打つ方向を決める変数が0.0f（値が入っていない）場合
        if (autoMoveLateralDirection == 0.0f)
        {
            // プレイヤとボールの相対的な位置関係に応じて、ボールの横方向の向きを決める
            Vector3 objectPositionInPlayerLocal = transform.InverseTransformPoint(ballObject.transform.position);
            lateralDirection = objectPositionInPlayerLocal.x > 0 ? -1.0f : 1.0f;
        }
        else { lateralDirection = autoMoveLateralDirection; }

        float lateralSpeedMagnification = lateralDirection * (ballObject.transform.position.z - transform.position.z);

        ball.Time = 0.0f;
        ball.SpeedX = powerX * lateralSpeedMagnification;
        ball.SpeedY = powerY;
        ball.SpeedZ = powerZ * transform.forward.z;

        AdjustUpwardSpeed(ballObject);
        AdjustBallSpeed(ballObject);

        ball.BallSpeed = ballSpeed;
    }

    public void Volley(GameObject ballObject, DominantHand dominantHand)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        float powerX = 2.0f;
        float powerY = 5.0f;
        float powerZ = 55.0f;

        // ボールの左右のスピードを決めるための変数
        float lateralDirection;

        // 自動移動時に打つ方向を決める変数が0.0f（値が入っていない）場合
        if (autoMoveLateralDirection == 0.0f)
        {
            // プレイヤとボールの相対的な位置関係に応じて、ボールの横方向の向きを決める
            Vector3 objectPositionInPlayerLocal = transform.InverseTransformPoint(ballObject.transform.position);
            lateralDirection = objectPositionInPlayerLocal.x > 0 ? -1.0f : 1.0f;
        }
        else { lateralDirection = autoMoveLateralDirection; }

        if (lateralDirection == (int)dominantHand) { animator.SetTrigger("ForeVolley"); }
        else if (lateralDirection == -(int)dominantHand) { animator.SetTrigger("BackVolley"); }

        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        float lateralSpeedMagnification = lateralDirection * (ballObject.transform.position.z - transform.position.z);

        ball.Time = 0.0f;
        ball.SpeedX = powerX * lateralSpeedMagnification;
        ball.SpeedY = powerY;
        ball.SpeedZ = powerZ * transform.forward.z;

        AdjustUpwardSpeed(ballObject);
        AdjustBallSpeed(ballObject);

        ball.BallSpeed = ballSpeed;
    }

    public void AIShot(GameObject ballObject, Vector3 power, DominantHand dominantHand)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        float powerX = power.x;
        float powerY = power.y;
        float powerZ = power.z;

        Vector3 objectPositionInPlayerLocal = transform.InverseTransformPoint(ballObject.transform.position);
        float lateralDirection = objectPositionInPlayerLocal.x > 0 ? -1.0f : 1.0f;

        if (lateralDirection == (int)dominantHand) { animator.SetTrigger("Forehand"); }
        else if (lateralDirection == -(int)dominantHand) { animator.SetTrigger("Backhand"); }

        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ball.Time = 0.0f;
        ball.SpeedX = powerX;
        ball.SpeedY = powerY;
        ball.SpeedZ = powerZ;

        ball.BallSpeed = ballSpeed;
    }

    // Task・Settingモード専用処理
    public void ServeBall()
    {
        Vector3 position = transform.position;

        GameObject ballObject = Instantiate(taskBallPrefab);
        ballObject.transform.position = new Vector3(position.x - 1.0f, position.y + 5.0f, position.z - 3.0f);

        IBallController ball = ballObject.GetComponent<IBallController>();

        animator.SetTrigger("Forehand");

        int num = Random.Range(0, 3);
        audioSource.PlayOneShot(hitSounds[num]);

        ball.Time = 0.0f;

        Vector3 ballSpeed = DecideBallSpeed(ballObject);
        ball.SpeedX = ballSpeed.x;
        ball.SpeedY = ballSpeed.y;
        ball.SpeedZ = -ballSpeed.z;

        ball.BallSpeed = Parameters.ballSpeed[(int)player];
    }

    private void AdjustUpwardSpeed(GameObject ballObject)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        Vector3 ballPosition = ballObject.transform.position;
        float netDistance = Mathf.Abs(ballPosition.z);

        float gravity = ball.Gravity;

        float netArrivalTime = netDistance / Mathf.Abs(ball.SpeedZ);
        float netArrivalHight = ball.SpeedY * netArrivalTime - 0.50f * gravity * netArrivalTime * netArrivalTime + ballPosition.y;

        // (GameData内で定義されているnetHightを使用しているが、TaskData内で定義されているものを使ってもよい)
        if (GameData.netHight / 2.0f < netArrivalHight && netArrivalHight < GameData.netHight)
        {
            float targetHight = 7.0f;
            ball.SpeedZ *= 0.60f;

            float adjustedNetArrivalTime = netDistance / Mathf.Abs(ball.SpeedZ);
            float adjustedSpeedY = ((targetHight - ballPosition.y) + 0.50f * gravity * adjustedNetArrivalTime * adjustedNetArrivalTime) / adjustedNetArrivalTime;

            ball.SpeedY = adjustedSpeedY;
        }
    }

    private void AdjustBallSpeed(GameObject ballObject)
    {
        IBallController ball = ballObject.GetComponent<IBallController>();

        Vector3 ballPosition = ballObject.transform.position;

        float speedY = ball.SpeedY;
        float gravity = ball.Gravity;
        float arrivalTime = (speedY + Mathf.Sqrt(speedY * speedY + 2.0f * gravity * ballPosition.y)) / gravity;

        float arrivalPointX = ball.SpeedX * arrivalTime + ballPosition.x;
        float arrivalPointZ = ball.SpeedZ * arrivalTime + ballPosition.z;

        Area area = opponentCourtArea;
        float margin = 3.0f;

        if (area.xPositiveLimit < arrivalPointX && arrivalPointX < area.xPositiveLimit + 10.0f)
        {
            // X軸の正方向へのアウトを補正する
            ball.SpeedX = ((area.xPositiveLimit - margin) - ballPosition.x) / arrivalTime;
        }
        else if (area.xNegativeLimit - 10.0f < arrivalPointX && arrivalPointX < area.xNegativeLimit)
        {
            // X軸の負方向へのアウトを補正する
            ball.SpeedX = ((area.xNegativeLimit + margin) - ballPosition.x) / arrivalTime;
        }

        if (area.zPositiveLimit < arrivalPointZ && arrivalPointZ < area.zPositiveLimit + 10.0f)
        {
            // Z軸の正方向へのアウトを補正する
            ball.SpeedZ = ((area.zPositiveLimit - margin) - ballPosition.z) / arrivalTime;
        }
        else if (area.zNegativeLimit - 10.0f < arrivalPointZ && arrivalPointZ < area.zNegativeLimit)
        {
            // Z軸の負方向へのアウトを補正する
            ball.SpeedZ = ((area.zNegativeLimit + margin) - ballPosition.z) / arrivalTime;
        }
    }

    // Task・Settingモード専用処理
    private Vector3 DecideBallSpeed(GameObject ballObject)
    {
        TaskBallController ball = ballObject.GetComponent<TaskBallController>();
        ITaskManager taskManagerInterface = TaskData.taskManagerInterface;

        Vector3 arrivalPosition = taskManagerInterface.DecideArrivalPosition(ball);
        Vector3 decidedBallSpeed;

        decidedBallSpeed.y = 25.0f;
        decidedBallSpeed.x = taskManagerInterface.DecideLateralSpeed(ballObject, ball, decidedBallSpeed.y, arrivalPosition);
        decidedBallSpeed.z = taskManagerInterface.DecideDepthSpeed(ballObject, ball, decidedBallSpeed.y, arrivalPosition);

        return decidedBallSpeed;
    }

    public void Takeback(bool fore, bool back, float lateralDirection)
    {
        autoMoveLateralDirection = lateralDirection;
        animator.SetBool("TakebackFore", fore);
        animator.SetBool("TakebackBack", back);
    }

    public void ExitServeAnimation()
    {
        if (GameData.server != name) { return; }
        StartCoroutine(LayerWeightCoroutine());
    }

    private IEnumerator LayerWeightCoroutine()
    {
        float weight = 0.0f;
        float weightDelta = 0.010f;

        while (animator.GetLayerWeight(1) < 1.0f)
        {
            weight += weightDelta;
            animator.SetLayerWeight(1, weight);

            yield return new WaitForEndOfFrame();
        }
    }
}
