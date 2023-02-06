using UnityEngine;

public class RacketMotionAnimationController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void AnimateForeTakeback()
    {
        animator.SetBool("bool_takeback_right_fore", true);
        animator.SetBool("bool_takeback_right_back", false);
    }

    public void AnimateBackTakeback()
    {
        animator.SetBool("bool_takeback_right_fore", false);
        animator.SetBool("bool_takeback_right_back", true);
    }

    public void ResetTakeback()
    {
        animator.SetBool("bool_takeback_right_fore", false);
        animator.SetBool("bool_takeback_right_back", false);
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
