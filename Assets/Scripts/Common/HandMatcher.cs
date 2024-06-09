using UnityEngine;

public class HandMatcher : MonoBehaviour
{
    [SerializeField]
    private DominantHand dominantHand;

    // IK制御を行う体の部分
    private AvatarIKGoal ikGoal;

    [SerializeField]
    private Transform handPoint;

    private Animator animator;

    private float ikStrength;

    // IK制御有効化フラグ
    public bool isEnableIK = true;

    // Start is called before the first frame update
    void Start()
    {
        if (dominantHand == DominantHand.right) { ikGoal = AvatarIKGoal.LeftHand; }
        else if (dominantHand == DominantHand.left) { ikGoal = AvatarIKGoal.RightHand; }
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if ((animator.GetCurrentAnimatorStateInfo(0).IsName("ReceiverMovingBlendTree")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("MovingBlendTree"))
            && animator.GetCurrentAnimatorStateInfo(1).IsName("humanoid_idle"))
        {
            // OnAnimatorIKはIK制御情報を更新する際に呼ばれるコールバック
            if (!isEnableIK) { return; }

            ikStrength += Time.deltaTime;
            if (ikStrength > 1.0f) { ikStrength = 1.0f; }

            // 武器に作成したLeftHandPointに、左手を移動させる
            animator.SetIKPositionWeight(ikGoal, ikStrength);
            animator.SetIKRotationWeight(ikGoal, ikStrength);
            animator.SetIKPosition(ikGoal, handPoint.position);
            animator.SetIKRotation(ikGoal, handPoint.rotation);
        }
        else { ikStrength = 0.0f; }
    }
}
