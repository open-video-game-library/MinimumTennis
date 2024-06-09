using UnityEngine;

public class Move : MonoBehaviour, ICalculateSpeed, ICharacterMover
{
    [System.NonSerialized]
    public Players player;

    private readonly float baseMaximumSpeed = 30.0f;

    // Editable Parameters
    private float maximumSpeed = 30.0f;
    private float acceleration = 1.0f;

    private float speedX = 0.0f;
    private float speedZ = 0.0f;

    [System.NonSerialized]
    public Area movableArea;

    private Vector3 defaultRotation;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        maximumSpeed = Parameters.maximumSpeed[(int)player];
        acceleration = Parameters.acceleration[(int)player];

        defaultRotation = transform.eulerAngles;

        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        maximumSpeed = Parameters.maximumSpeed[(int)player];
        acceleration = Parameters.acceleration[(int)player];
    }

    void FixedUpdate()
    {
        // 移動速度と移動方向に応じて、アニメーションのパラメータを変更
        animator.SetFloat("SpeedX", speedX / maximumSpeed);
        animator.SetFloat("SpeedY", speedZ / maximumSpeed);
        animator.SetFloat("AnimationSpeed", maximumSpeed / baseMaximumSpeed);
    }

    // キャラクタの足のIK最適化用に用いる関数
    public float CalculateSpeed()
    {
        float speedRateX = Mathf.Abs(speedX / maximumSpeed);
        float speedRateZ;

        if (speedZ < 0) { speedRateZ = 2.0f * Mathf.Abs(speedZ / maximumSpeed); }
        else { speedRateZ = Mathf.Abs(speedZ / maximumSpeed); }

        return Mathf.Max(speedRateX, speedRateZ);
    }

    public void ResetCharacterPosition(Vector3 position)
    {
        // Reset position
        Vector3 currentPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
        transform.Translate(position - currentPosition, Space.World);

        // Reset rotation
        Vector3 currentRotation = transform.eulerAngles;
        transform.Rotate(defaultRotation - currentRotation);
    }

    public void MovePlayer(float x, float z)
    {
        // 左方向への移動
        if (x < -0.10f)
        {
            if (speedX - acceleration > maximumSpeed * x) { speedX -= acceleration; }
            else { speedX = maximumSpeed * x; }
        }
        // 右方向への移動
        else if (x > 0.10f)
        {
            if (speedX + acceleration < maximumSpeed * x) { speedX += acceleration; }
            else { speedX = maximumSpeed * x; }
        }
        // 何も入力されていない場合は減速
        else { speedX *= 0.90f; }

        // 前方向への移動
        if (z > 0.10f)
        {
            if (speedZ + acceleration < maximumSpeed * z) { speedZ += acceleration; }
            else { speedZ = maximumSpeed * z; }
        }
        // 後方向への移動
        else if (z < -0.10f)
        {
            if (speedZ - acceleration > maximumSpeed * z) { speedZ -= acceleration; }
            else { speedZ = maximumSpeed * z; }
        }
        // 何も入力されていない場合は減速
        else { speedZ *= 0.90f; }

        // 移動する
        transform.Translate(speedX * Time.deltaTime, 0.0f, speedZ * Time.deltaTime);

        // 移動可能範囲を超えないようにする
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, movableArea.xNegativeLimit, movableArea.xPositiveLimit);
        currentPosition.z = Mathf.Clamp(currentPosition.z, movableArea.zNegativeLimit, movableArea.zPositiveLimit);
        transform.position = currentPosition;
    }

    public void StopPlayer()
    {
        speedX *= 0.90f;
        speedZ *= 0.90f;
    }
}
