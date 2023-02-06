using UnityEngine;

public class PlayerMover : MonoBehaviour, ICharacterMover
{
    private float playerMaximumSpeed;
    private float playerAcceleration;

    private float speedX = 0.0f;
    private float speedZ = 0.0f;
    
    private Vector3 movementRangeBegin = new Vector3(10.0f, 0.0f, -32.0f);
    private Vector3 movementRangeEnd = new Vector3(80.0f, 0.0f, 32.0f);

    [SerializeField]
    private Vector3 defaultRotation;

    [SerializeField]
    private GameObject inputManager;
    private KeyboardInputManager keyboard;
    private GamepadInputManager gamepad;

    // Start is called before the first frame update
    void Start()
    {
        playerMaximumSpeed = PlayerMaximumSpeed.playerMaximumSpeed;
        playerAcceleration = PlayerAcceleration.playerAcceleration;
        keyboard = inputManager.GetComponent<KeyboardInputManager>();
        gamepad = inputManager.GetComponent<GamepadInputManager>();
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

        if ((!GameManager.isToss && GameManager.ballAmount != 0) || GameManager.server != name)
        {
            if (gamepad.isConnected) { MovePlayerGamepad(movementRangeBegin, movementRangeEnd); }
            else { MovePlayerKeyboard(movementRangeBegin, movementRangeEnd); }
        }
    }

    public void ResetCharacterPosition(Vector3 position)
    {
        Vector3 currentPosition = transform.position;
        transform.Translate(position - currentPosition, Space.World);
        Vector3 currentRotation = transform.eulerAngles;
        transform.Rotate(defaultRotation - currentRotation);
    }

    private void MovePlayerKeyboard(Vector3 begin, Vector3 end)
    {
        if (keyboard.isPressedA)
        {
            // move left
            if (speedZ - playerAcceleration > -playerMaximumSpeed) { speedZ -= playerAcceleration; }
            else { speedZ = -playerMaximumSpeed; }
        }
        else if (keyboard.isPressedD)
        {
            // move right
            if (speedZ + playerAcceleration < playerMaximumSpeed) { speedZ += playerAcceleration; }
            else { speedZ = playerMaximumSpeed; }
        }
        else { speedZ *= 0.90f; }

        if (begin.z < transform.position.z + speedZ * Time.deltaTime
            && transform.position.z + speedZ * Time.deltaTime < end.z)
        {
            transform.Translate(0.0f, 0.0f, speedZ * Time.deltaTime, Space.World);
            RotateCharacter(speedZ, playerMaximumSpeed);
        }
        else { speedZ = 0.0f; }
        

        if (keyboard.isPressedW)
        {
            // move forward
            if (speedX - playerAcceleration > -playerMaximumSpeed) { speedX -= playerAcceleration; }
            else { speedX = -playerMaximumSpeed; }
        }
        else if (keyboard.isPressedS)
        {
            // move back
            if (speedX + playerAcceleration < playerMaximumSpeed / 2.0f) { speedX += playerAcceleration; }
            else { speedX = playerMaximumSpeed / 2.0f; }
        }
        else { speedX *= 0.90f; }

        if (begin.x < transform.position.x + speedX * Time.deltaTime
            && transform.position.x + speedX * Time.deltaTime < end.x)
        {
            transform.Translate(speedX * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        else { speedX = 0.0f; }
    }

    private void MovePlayerGamepad(Vector3 begin, Vector3 end)
    {
        if (gamepad.leftStickValue.x < -0.10f)
        {
            // move left
            if (speedZ - playerAcceleration > -playerMaximumSpeed * gamepad.leftStickAbsValue.x) { speedZ -= playerAcceleration; }
            else { speedZ = -playerMaximumSpeed * gamepad.leftStickAbsValue.x; }
        }
        else if (gamepad.leftStickValue.x > 0.10f)
        {
            // move right
            if (speedZ + playerAcceleration < playerMaximumSpeed * gamepad.leftStickAbsValue.x) { speedZ += playerAcceleration; }
            else { speedZ = playerMaximumSpeed * gamepad.leftStickAbsValue.x; }
        }
        else { speedZ *= 0.90f; }

        if (begin.z < transform.position.z + speedZ * Time.deltaTime
            && transform.position.z + speedZ * Time.deltaTime < end.z)
        {
            transform.Translate(0.0f, 0.0f, speedZ * Time.deltaTime, Space.World);
            RotateCharacter(speedZ, playerMaximumSpeed);
        }
        else { speedZ = 0.0f; }

        if (gamepad.leftStickValue.y > 0.10f)
        {
            // move forward
            if (speedX - playerAcceleration > -playerMaximumSpeed * gamepad.leftStickAbsValue.y) { speedX -= playerAcceleration; }
            else { speedX = -playerMaximumSpeed * gamepad.leftStickAbsValue.y; }
        }
        else if (gamepad.leftStickValue.y < -0.10f)
        {
            // move back
            if (speedX + playerAcceleration < playerMaximumSpeed * gamepad.leftStickAbsValue.y / 2.0f) { speedX += playerAcceleration; }
            else { speedX = playerMaximumSpeed * gamepad.leftStickAbsValue.y / 2.0f; }
        }
        else { speedX *= 0.90f; }

        if (begin.x < transform.position.x + speedX * Time.deltaTime
            && transform.position.x + speedX * Time.deltaTime < end.x)
        {
            transform.Translate(speedX * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        else { speedX = 0.0f; }
    }

    private void RotateCharacter(float currentSpeed, float maxSpeed)
    {
        float currentYAngle = transform.eulerAngles.y;
        if (currentYAngle > 180.0f) { currentYAngle = currentYAngle - 360.0f; }

        float maxAngle = 45.0f;
        float angularAcceleration = maxAngle * (currentSpeed / maxSpeed);

        if ((keyboard.isPressedA || keyboard.isPressedD) || gamepad.leftStickAbsValue.x > 0.10f)
        {
            if (defaultRotation.y - maxAngle < currentYAngle + 5.0f * angularAcceleration * Time.deltaTime
                && currentYAngle + 5.0f * angularAcceleration * Time.deltaTime < defaultRotation.y + maxAngle)
            {
                // turn left or right
                transform.Rotate(new Vector3(0.0f, 5.0f * angularAcceleration * Time.deltaTime, 0.0f));
            }
        }
        else
        {
            if (currentYAngle > defaultRotation.y + 1.0f) { transform.Rotate(0.0f, -120.0f * Time.deltaTime, 0.0f); }
            if (currentYAngle < defaultRotation.y - 1.0f) { transform.Rotate(0.0f, 120.0f * Time.deltaTime, 0.0f); }
        }
    }
}
