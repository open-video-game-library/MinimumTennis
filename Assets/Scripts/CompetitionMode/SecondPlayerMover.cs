using UnityEngine;

public class SecondPlayerMover : MonoBehaviour, ICharacterMover
{
    private float playerMaximumSpeed;
    private float playerAcceleration;

    private float speedX = 0.0f;
    private float speedZ = 0.0f;

    private Vector3 movementRangeBegin = new Vector3(-80.0f, 0.0f, -32.0f);
    private Vector3 movementRangeEnd = new Vector3(-10.0f, 0.0f, 32.0f);

    [SerializeField]
    private Vector3 defaultRotation;

    [SerializeField]
    private GameObject inputManager;
    private MultipleGamepadManager gamepad;

    // Start is called before the first frame update
    void Start()
    {
        playerMaximumSpeed = PlayerMaximumSpeed.playerMaximumSpeed;
        playerAcceleration = PlayerAcceleration.playerAcceleration;
        gamepad = inputManager.GetComponent<MultipleGamepadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.inPlay)
        {
            speedX = 0.0f;
            speedZ = 0.0f;
            return;
        }

        playerMaximumSpeed = PlayerMaximumSpeed.playerMaximumSpeed;
        playerAcceleration = PlayerAcceleration.playerAcceleration;

        playerMaximumSpeed = PlayerMaximumSpeed.playerMaximumSpeed;

        if ((!GameManager.isToss && GameManager.ballAmount != 0) || GameManager.server != name)
        {
            if (gamepad.isConnected[1]) { MovePlayerGamepad(movementRangeBegin, movementRangeEnd); }
        }
    }

    public void ResetCharacterPosition(Vector3 position)
    {
        Vector3 currentPosition = transform.position;
        transform.Translate(position - currentPosition, Space.World);
        Vector3 currentRotation = transform.eulerAngles;
        transform.Rotate(defaultRotation - currentRotation);
    }

    private void MovePlayerGamepad(Vector3 begin, Vector3 end)
    {
        if (gamepad.leftStickValue[1].x > 0.10f)
        {
            // move left
            if (speedZ - playerAcceleration > -playerMaximumSpeed * gamepad.leftStickAbsValue[1].x) { speedZ -= playerAcceleration; }
            else { speedZ = -playerMaximumSpeed * gamepad.leftStickAbsValue[1].x; }
            
        }
        else if (gamepad.leftStickValue[1].x < -0.10f)
        {
            // move right
            if (speedZ + playerAcceleration < playerMaximumSpeed * gamepad.leftStickAbsValue[1].x) { speedZ += playerAcceleration; }
            else { speedZ = playerMaximumSpeed * gamepad.leftStickAbsValue[1].x; }
        }
        else { speedZ *= 0.90f; }

        if (begin.z < transform.position.z + speedZ * Time.deltaTime
            && transform.position.z + speedZ * Time.deltaTime < end.z)
        {
            // move left or right
            transform.Translate(0.0f, 0.0f, speedZ * Time.deltaTime, Space.World);
            RotateCharacter(speedZ, playerMaximumSpeed);
        }
        else { speedZ = 0.0f; }

        if (gamepad.leftStickValue[1].y > 0.10f)
        {
            // move forward
            if (speedX + playerAcceleration < playerMaximumSpeed * gamepad.leftStickAbsValue[1].y) { speedX += playerAcceleration; }
            else { speedX = playerMaximumSpeed * gamepad.leftStickAbsValue[1].y; }
        }
        else if (gamepad.leftStickValue[1].y < -0.10f)
        {
            // move back
            if (speedX - playerAcceleration > -playerMaximumSpeed * gamepad.leftStickAbsValue[1].y / 2.0f) { speedX -= playerAcceleration; }
            else { speedX = -playerMaximumSpeed * gamepad.leftStickAbsValue[1].y / 2.0f; }
        }
        else { speedX *= 0.90f; }

        if (begin.x < transform.position.x + speedX * Time.deltaTime
            && transform.position.x + speedX * Time.deltaTime < end.x)
        {
            // move forward or back
            transform.Translate(speedX * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        else { speedX = 0.0f; }
    }

    private void RotateCharacter(float currentSpeed, float maxSpeed)
    {
        float currentYAngle = transform.eulerAngles.y;

        float maxAngle = 45.0f;
        float angularAcceleration = maxAngle * (currentSpeed / maxSpeed);

        if (gamepad.leftStickValue[1].x < -0.10f || gamepad.leftStickValue[1].x > 0.10f)
        {
            if (defaultRotation.y - maxAngle < currentYAngle - 5.0f * angularAcceleration * Time.deltaTime
                && currentYAngle - 5.0f * angularAcceleration * Time.deltaTime < defaultRotation.y + maxAngle)
            {
                // turn left or right
                transform.Rotate(new Vector3(0.0f, -5.0f * angularAcceleration * Time.deltaTime, 0.0f));
            }
        }
        else
        {
            if (currentYAngle > defaultRotation.y + 1.0f) { transform.Rotate(0.0f, -120.0f * Time.deltaTime, 0.0f); }
            if (currentYAngle < defaultRotation.y - 1.0f) { transform.Rotate(0.0f, 120.0f * Time.deltaTime, 0.0f); }
        }
    }
}