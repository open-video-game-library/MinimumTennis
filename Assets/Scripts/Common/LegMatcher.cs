using UnityEngine;

public class LegMatcher : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = new Vector3(0.0f, 0.50f, 0.0f);
    [SerializeField]
    private float rayRange = 1.0f;
    [SerializeField]
    private string fieldLayerName = "Field";

    private Animator animator;
    private Transform _transform;

    ICalculateSpeed calculateSpeed;
    private float weight = 0.0f;

    public new Transform transform
    {
        get
        {
            if (_transform == null) { _transform = gameObject.transform; }
            return _transform;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        calculateSpeed = GetComponent<ICalculateSpeed>();
    }

    void OnAnimatorIK()
    {
        if (calculateSpeed != null) { weight = 0.70f * (1.0f - 3.0f * calculateSpeed.CalculateSpeed()) * animator.GetLayerWeight(1); }
        else { weight = 0.70f * animator.GetLayerWeight(1); }

        //âEë´
        var ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot), -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayRange, LayerMask.GetMask(fieldLayerName)))
        {
            Quaternion rightRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + offset);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightRotation);
        }

        //ç∂ë´
        ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot), -transform.up);
        if (Physics.Raycast(ray, out hit, rayRange, LayerMask.GetMask(fieldLayerName)))
        {
            Quaternion leftRotaion = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + offset);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftRotaion);
        }
    }
}