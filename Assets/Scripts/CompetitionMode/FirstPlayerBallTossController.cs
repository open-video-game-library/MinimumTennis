using UnityEngine;

public class FirstPlayerBallTossController : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    private BallController ballController;

    [SerializeField]
    private GameObject inputManager;
    private MultipleGamepadManager gamepad;

    // Start is called before the first frame update
    void Start()
    {
        gamepad = inputManager.GetComponent<MultipleGamepadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.ballAmount != 0 || !GameManager.inPlay) { return; }

        if (GameManager.server == name)
        {
            float posX = transform.position.x;
            float posZ = transform.position.z;

            if (!gamepad.isConnected[0]) { return; }
            if (gamepad.InputEastThisFrame(0))
            {
                GameManager.isToss = true;
                GameObject ball = Instantiate(ballPrefab);
                ball.transform.position = new Vector3(posX - 5.0f, 6.0f, posZ);
                ballController = ball.GetComponent<BallController>();
                ballController.speedY = 28.0f;
                ballController.ballSpeed = PlayerBallSpeed.playerBallSpeed;
            }
        }
    }
}
