using UnityEngine;

public class SelectionCharacterController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Receiver", true);
    }
}
