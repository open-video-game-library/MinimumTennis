using UnityEngine;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Manage()
    {
        animator.SetBool("Multiple", Parameters.playMode == PlayMode.competition);
    }
}
