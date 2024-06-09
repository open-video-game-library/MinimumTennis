using UnityEngine;

public class TaskPauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    private bool isPreviousFramePaused;

    // Update is called once per frame
    void Update()
    {
        if (TaskData.pause)
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }

        if (isPreviousFramePaused && !TaskData.pause)
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }

        isPreviousFramePaused = TaskData.pause;
    }

    public void ExitPause()
    {
        TaskData.pause = false;
    }
}