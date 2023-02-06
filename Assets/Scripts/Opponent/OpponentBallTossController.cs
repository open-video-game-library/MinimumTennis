using UnityEngine;

public class OpponentBallTossController : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    private BallController ballController;

    private int autoServeCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.ballAmount != 0 || !GameManager.inPlay) { return; }

        if (GameManager.server == name)
        {
            float posX = transform.position.x;
            float posZ = transform.position.z;

            autoServeCount++;
            if (autoServeCount * Time.deltaTime > 2)
            {
                GameManager.isToss = true;
                GameObject ball = Instantiate(ballPrefab);
                ball.transform.position = new Vector3(posX + 5.0f, 6.0f, posZ);
                ballController = ball.GetComponent<BallController>();
                ballController.speedY = 28.0f;
                ballController.ballSpeed = OpponentBallSpeed.opponentBallSpeed;
                autoServeCount = 0;
            }
        }
    }
}