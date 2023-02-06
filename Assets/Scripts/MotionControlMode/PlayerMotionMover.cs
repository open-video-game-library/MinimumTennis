using UnityEngine;

public class PlayerMotionMover : MonoBehaviour, ICharacterMover
{
    private float playerMaximumSpeed;
    private float playerAcceleration;

    private float speedX = 0.0f;
    private float speedZ = 0.0f;

    private PlayerAutoControllAI ai;
    private Vector3 movementRangeBegin = new Vector3(10.0f, 0.0f, -32.0f);
    private Vector3 movementRangeEnd = new Vector3(80.0f, 0.0f, 32.0f);

    [SerializeField]
    private Vector3 defaultRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerMaximumSpeed = PlayerMaximumSpeed.playerMaximumSpeed;
        playerAcceleration = PlayerAcceleration.playerAcceleration;
        ai = GetComponent<PlayerAutoControllAI>();
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
            MovePlayerAuto(movementRangeBegin, movementRangeEnd, ai.targetPosition);
        }
    }

    public void ResetCharacterPosition(Vector3 position)
    {
        // Reset position
        Vector3 currentPosition = transform.position;
        transform.Translate(position - currentPosition, Space.World);
        ai.targetPosition = position;
        // Reset rotation
        Vector3 currentRotation = transform.eulerAngles;
        transform.Rotate(defaultRotation - currentRotation);
    }

    private void MovePlayerAuto(Vector3 begin, Vector3 end, Vector3 ballArrivalPoint)
    {
        float arrivalRange = 5.0f;

        if (!ai.isArrivedZ && !(ballArrivalPoint.z - arrivalRange <= transform.position.z
              && transform.position.z <= ballArrivalPoint.z + arrivalRange))
        {
            if (transform.position.z >= ballArrivalPoint.z)
            {
                // move left
                if (speedZ - playerAcceleration > -playerMaximumSpeed) { speedZ -= playerAcceleration; }
                else { speedZ = -playerMaximumSpeed; }
            }
            else if (transform.position.z < ballArrivalPoint.z)
            {
                // move right
                if (speedZ + playerAcceleration < playerMaximumSpeed) { speedZ += playerAcceleration; }
                else { speedZ = playerMaximumSpeed; }
            }
        }
        else
        {
            ai.isArrivedZ = true;
            speedZ *= 0.90f;
        }

        if (begin.z < transform.position.z + speedZ * Time.deltaTime
            && transform.position.z + speedZ * Time.deltaTime < end.z)
        {
            // move left or right
            transform.Translate(0.0f, 0.0f, speedZ * Time.deltaTime, Space.World);
            RotateCharacter(speedZ, playerMaximumSpeed);
        }
        else { speedZ = 0.0f; }

        if (!ai.isArrivedX && !(ballArrivalPoint.x - arrivalRange <= transform.position.x
              && transform.position.x <= ballArrivalPoint.x + arrivalRange))
        {
            if (transform.position.x >= ballArrivalPoint.x)
            {
                // move front
                if (speedX - playerAcceleration > -playerMaximumSpeed / 2.0f) { speedX -= playerAcceleration; }
                else { speedX = -playerMaximumSpeed; }
            }
            else if (transform.position.x < ballArrivalPoint.x)
            {
                // move back
                if (speedX + playerAcceleration < playerMaximumSpeed) { speedX += playerAcceleration; }
                else { speedX = playerMaximumSpeed / 2.0f; }
            }
        }
        else
        {
            ai.isArrivedX = true;
            speedX *= 0.90f;
        }

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
        if (currentYAngle > 180.0f) { currentYAngle = currentYAngle - 360.0f; }

        float maxAngle = 45.0f;
        float angularAcceleration = maxAngle * (currentSpeed / maxSpeed);

        if (!ai.isArrivedZ)
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
