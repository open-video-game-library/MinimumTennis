using UnityEngine;

public class RacketAnimationController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void AnimateServe()
    {
        animator.SetTrigger("trigger_right_serve");
    }

    public void AnimateFore()
    {
        animator.SetTrigger("trigger_right_fore");
    }

    public void AnimateBack()
    {
        animator.SetTrigger("trigger_right_back");
    }
}
