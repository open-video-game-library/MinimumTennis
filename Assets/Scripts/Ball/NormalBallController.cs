using UnityEngine;

public class NormalBallController : MonoBehaviour, IBallController
{
    public float Gravity { get; set; }

    public float SpeedX { get; set; }

    public float SpeedY { get; set; }

    public float SpeedZ { get; set; }

    public float Time { get; set; }

    public float BallSpeed { get; set; }

    private float moveSpeedX;
    private float moveSpeedY;
    private float moveSpeedZ;

    private bool isCourtIn;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip boundSound;

    private void Awake()
    {
        Gravity = 49.0f;
        Time = 0.0f;
        BallSpeed = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isCourtIn = GameData.courtArea.CheckInside(transform.position.x, transform.position.z);

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
        
        if (GameData.isToss && transform.position.y < 5.0f)
        {
            GameData.isToss = false;
            Destroy(gameObject);
        }

        if (transform.position.y < -10.0f)
        {
            if (GameData.ballBoundCount == 0 && GameData.controllable) { GameData.isOut = true; }
            GameData.ballBoundCount++;
            Destroy(gameObject);
        }

        if (GameData.ballBoundCount > 3) { Destroy(gameObject); }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (GameData.ballBoundCount == 0 && GameData.controllable) { GameData.isOut = true; }
            GameData.ballBoundCount++;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Net"))
        {
            if (GameData.controllable) { GameData.isNet = true; }
            SpeedZ = -0.10f * moveSpeedZ;
        }
    }

    private void JudgeBound()
    {
        // コート内でバウンドした場合
        if (isCourtIn)
        {
            string whoseCourt = null;
            if (transform.position.z < 0.0f) { whoseCourt = GameData.character1.name; }
            else if (transform.position.z > 0.0f) { whoseCourt = GameData.character2.name; }

            if (GameData.controllable)
            {
                // 自分が打ったボールが、自分のコートでバウンドした場合は、ネット判定にする
                if (GameData.lastShooter == whoseCourt) { GameData.isNet = true; }
                // 自分が打ったボールが、サーブが入っていない状態で相手のコートでバウンドした場合は、サーブが入った状態にする
                else if (GameData.lastShooter != whoseCourt && !GameData.isServeIn) { GameData.isServeIn = true; }
            }
        }
        // コート外でバウンドした場合
        else
        {
            // プレイヤたちが制御可能状態で、バウンド回数が0回のとき、アウト判定にする
            if (GameData.controllable && GameData.ballBoundCount == 0) { GameData.isOut = true; }
        }
    }

    private void Bound()
    {
        SpeedY = -0.70f * moveSpeedY;
        Time = 0.0f;
        GameData.ballBoundCount++;

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
}
