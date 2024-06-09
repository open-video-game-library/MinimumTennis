using UnityEngine;

public class ParameterPanelManager : MonoBehaviour
{
    private Animator animator;

    private bool isPreviousPanelOpened;
    private bool isPanelOpend;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TaskData.pause && !isPreviousPanelOpened)
        {
            animator.SetTrigger("Open");
            isPanelOpend = true;
        }
        else if (!TaskData.pause && isPreviousPanelOpened)
        {
            animator.SetTrigger("Close");
            isPanelOpend = false;
        }

        isPreviousPanelOpened = isPanelOpend;
    }
}
