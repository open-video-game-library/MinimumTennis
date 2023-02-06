using UnityEngine;

public class BallController : MonoBehaviour
{
    [System.NonSerialized]
    public readonly float gravity = 49.0f;
    [System.NonSerialized]
    public float speedX = 0;
    [System.NonSerialized]
    public float speedY = 0;
    [System.NonSerialized]
    public float speedZ = 0;
    [System.NonSerialized]
    public float time = 0.0f;
    [System.NonSerialized]
    public float ballSpeed = 1.0f;

    private GameManager gameManager;
    private float moveSpeedX;
    private float moveSpeedY;
    private float moveSpeedZ;

    private Vector3 courtAreaBegin;
    private Vector3 courtAreaEnd;
    private bool isCourtIn;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        courtAreaBegin = GameManager.courtAreaBegin;
        courtAreaEnd = GameManager.courtAreaEnd;
    }

    // Update is called once per frame
    void Update()
    {
        isCourtIn = courtAreaBegin.x <= transform.position.x && transform.position.x <= courtAreaEnd.x
            && courtAreaBegin.z <= transform.position.z && transform.position.z <= courtAreaEnd.z;
        
        moveSpeedX = speedX;
        moveSpeedY = (speedY - gravity * time);
        moveSpeedZ = speedZ;

        transform.Translate(moveSpeedX * ballSpeed * Time.deltaTime, 
            moveSpeedY * ballSpeed * Time.deltaTime,
            moveSpeedZ * ballSpeed * Time.deltaTime,
            Space.World);
        time += Time.deltaTime * ballSpeed;

        if (GameManager.isToss && transform.position.y < 5.0f)
        {
            GameManager.isToss = false;
            Destroy(gameObject);
        }

        if (transform.position.y < -10.0f)
        {
            if (GameManager.ballBoundCount == 0) { gameManager.ReportBallOut(); }
            GameManager.ballBoundCount++;
            Destroy(gameObject);
        }

        if (GameManager.ballBoundCount > 3) { Destroy(gameObject); }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Court") && isCourtIn)
        {
            Bound();

            string whoseCourt = null;
            if (transform.position.x > 0.0f) { whoseCourt = "Player"; }
            else if (transform.position.x < 0.0f) { whoseCourt = "Opponent"; }

            if (GameManager.inPlay)
            {
                if (GameManager.lastShooter == whoseCourt) { gameManager.ReportBallNet(); }
                else { GameManager.isServeIn = true; }
            }
        }
        else if (collision.gameObject.CompareTag("Court") && !isCourtIn)
        {
            if (GameManager.ballBoundCount == 0) { gameManager.ReportBallOut(); }
            Bound();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (GameManager.ballBoundCount == 0) { gameManager.ReportBallOut(); }
            GameManager.ballBoundCount++;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Net"))
        {
            gameManager.ReportBallNet();
            speedX = -0.10f * moveSpeedX;
        }
    }

    private void Bound()
    {
        speedY = -0.70f * moveSpeedY;
        time = 0.0f;
        GameManager.ballBoundCount++;
    }
}
