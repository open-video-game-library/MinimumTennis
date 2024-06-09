using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    private bool isPreviousFramePaused;

    // Update is called once per frame
    void Update()
    {
        if (GameData.pause)
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }

        if (isPreviousFramePaused && !GameData.pause)
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }

        isPreviousFramePaused = GameData.pause;
    }

    public void ExitPause()
    {
        GameData.pause = false;
    }
}
