using UnityEngine;

public class PlayerBallTossController : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    private BallController ballController;

    [SerializeField]
    private GameObject gamepadInputManager;
    private GamepadInputManager gamepad;

    // Start is called before the first frame update
    void Start()
    {
        gamepad = gamepadInputManager.GetComponent<GamepadInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.ballAmount != 0 || !GameManager.inPlay) { return; }

        if (GameManager.server == name)
        {
            float posX = transform.position.x;
            float posZ = transform.position.z;

            if (Input.GetKeyDown(KeyCode.Space) || gamepad.InputEastThisFrame())
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